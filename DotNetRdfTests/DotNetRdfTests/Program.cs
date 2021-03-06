﻿using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Imports.Newtonsoft.Json;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Writing;
using Xunit;

namespace Tests
{
    [JsonObject(IsReference = true)] 
    class Person
    {
        public string Id { get; set; }

        public Person()
        {
            Parent = null;
            Children = new List<Person>();
        }

        public Person Parent { get; set; }
        public List<Person> Children { get; set; }
    }

    public class Program
    {
        private static void Main(string[] args)
        {
            var p = new Program();

            p.TestRdfSaveToRaven();

            //TestRavenCircural();

            Console.WriteLine("Press any key to exit..");
            Console.ReadLine();
        }
        
        [Fact]
        public void TestRdfSaveToRaven()
        {
            IGraph g = new Graph();
            RdfJsonParser rdfJsonParser = new RdfJsonParser();
            rdfJsonParser.Load(g, @"rdfFiles\dbPediaCities.rdfjson");

            var documentStore = new EmbeddableDocumentStore()
            {
                UseEmbeddedHttpServer = true,
            };

            documentStore.Configuration.Port = 8081;

            documentStore.Initialize();

            using (var session = documentStore.OpenSession())
            {
                session.Store(g, @"http://dbpedia.org/resource/United_States");

                session.SaveChanges();
            }

            //using (var session = documentStore.OpenSession())
            //{
            //    var graph = session.Load<IGraph>(@"http://dbpedia.org/resource/United_States");

            //    RdfJsonWriter writer = new RdfJsonWriter();
            //    writer.Save(graph, "test.rdfJsonWriter");
            //}

        }

        [Fact]
        public void TestRavenCircural()
        {
            try
            {

                var documentStore = new EmbeddableDocumentStore()
                {
                    UseEmbeddedHttpServer = true,
                };

                documentStore.Configuration.Port = 8081;

                documentStore.Initialize();

                using (var session = documentStore.OpenSession())
                {
                    var parent = new Person();
                    var child = new Person() {Parent = parent};
                    parent.Children.Add(child);

                    session.Store(parent);

                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        [Fact]
        public void SparqlRawRemoteQuery()
        {
            var queryParser = new SparqlQueryParser();

            var queryString = @"DESCRIBE <http://linked.opendata.cz/resource/ATC/M01AE01>";
            SparqlQuery query = queryParser.ParseFromString(queryString);
            query.ToString();
            var datasetUri = new Uri(@"http://linked.opendata.cz/resource/dataset/ATC");
            var sparqlEndpointUri = new Uri(@"http://linked.opendata.cz/sparql");
            var endpoint = new SparqlRemoteEndpoint(sparqlEndpointUri, datasetUri);

            var results = endpoint.QueryRaw(query.ToString());
            var stream = results.GetResponseStream();
            var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            Console.WriteLine(content);
        }


    }
}
