using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Database.Server;
using Raven.Tests.Helpers;
using Xunit;

namespace Tests
{
    public class RavenTests
    {
        private const int RavenWebUiPort = 8081;
        private readonly EmbeddableDocumentStore _documentStore;

        public RavenTests()
        {
            var docStore = new EmbeddableDocumentStore
            {
                RunInMemory = true,
                Configuration = {Port = RavenWebUiPort}
            };
            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(RavenWebUiPort);

            docStore.Initialize();

            _documentStore = docStore;
        }

        [Fact]
        public async Task AnyChildLucieTestQuery()
        {
            await StoreTestData();

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                IQueryable<string> petrQuery =
                    session.Query<Parent>()
                        .Where(parent => parent.Children.Any(child => child.Name == "Lucie"))
                        .Select(parent => parent.Name);
                
                IList<string> names = await petrQuery.ToListAsync();

                RavenTestBase.WaitForUserToContinueTheTest(_documentStore);

                Assert.NotEmpty(names);
                Assert.Equal(names.Count, 1);
                Assert.Equal(names.First(), "Petr");
            }
        }

        [Fact]
        public async Task AnyChildLucieOrScheldonTest()
        {
            await StoreTestData();

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                IQueryable<string> petrQuery =
                    session.Query<Parent>()
                        .Where(parent => parent.Children.Any(child => child.Name == "Lucie" || child.Name == "Scheldon"))
                        .Select(parent => parent.Name);

                IList<string> names = await petrQuery.ToListAsync();

                RavenTestBase.WaitForUserToContinueTheTest(_documentStore);

                Assert.NotEmpty(names);
                Assert.Equal(names.Count, 2);
                Assert.True(names.Contains("Petr"));
                Assert.True(names.Contains("Penny"));
            }
        }

        [Fact]
        public async Task HasMarieAndTomasChildrenTest()
        {
            await StoreTestData();

            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                

                RavenQueryStatistics statistics;
                IQueryable<string> petrQuery =
                    session.Query<Parent>()
                        .Statistics(out statistics)
                        .Where(parent => parent.Children.Any(child => child.Name == "Marie"))
                        .Where(parent => parent.Children.Any(child => child.Name == "Tomáš"))
                        .Select(parent => parent.Name);

                IList<string> names = await petrQuery.ToListAsync();

                RavenTestBase.WaitForUserToContinueTheTest(_documentStore);

                Assert.NotEmpty(names);
                Assert.Equal(names.Count, 1);
                Assert.True(names.Contains("Petr"));
            }            
        }

        private async Task StoreTestData()
        {
            using (IAsyncDocumentSession session = _documentStore.OpenAsyncSession())
            {
                IEnumerable<Parent> data = TestData();
                foreach (Parent parent in data)
                {
                    await session.StoreAsync(parent);
                }
                await session.SaveChangesAsync();
            }
        }

        public IEnumerable<Parent> TestData()
        {
            return new[]
            {
                new Parent
                {
                    Name = "Petr",
                    Children = new List<Child>
                    {
                        new Child {Name = "Lucie"},
                        new Child {Name = "Marie"},
                        new Child {Name = "Tomáš"},
                        new Child {Name = "Marta"},
                        new Child {Name = "Karel"}
                    }
                },
                new Parent
                {
                    Name = "Pavel",
                    Children = new List<Child>
                    {
                        new Child {Name = "Adam"},
                        new Child {Name = "Marie"},
                        new Child {Name = "Marek"},
                        new Child {Name = "Šárka"},
                        new Child {Name = "Klára"},
                    }
                },
                new Parent
                {
                    Name = "Jan",
                    Children = new List<Child>
                    {
                        new Child {Name = "Daniel"},
                        new Child {Name = "Denisa"},
                        new Child {Name = "Aleš"},
                    }
                },
                new Parent
                {
                    Name = "Penny",
                    Children = new List<Child>
                    {
                        new Child {Name = "Scheldon"},
                        new Child {Name = "Raj"},
                        new Child {Name = "Howard"}
                    }
                }
            };
        }
    }

    public class Parent
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Child> Children { get; set; }
    }

    public class Child
    {
        public string Name { get; set; }
    }
}