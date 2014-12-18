using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Northwind.Entities;

namespace EntityFrameworkTests
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestNotIncludedQuery();

            using (var context = new NORTHWNDEntities())
            {
                context.Configuration.LazyLoadingEnabled = false;
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                var ordersQuery = context.Orders.Where(o => o.Customers.Region != o.Employees.Region);
                var orders = ordersQuery.ToList();

                foreach (var order in orders)
                {
                    PrintOrder(order);
                }
            }

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();

        }

        private static void TestNotIncludedQuery()
        {
            using (var context = new NORTHWNDEntities())
            {
                context.Configuration.LazyLoadingEnabled = false;
                context.Configuration.ProxyCreationEnabled = false;
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                //var customer = context.Customers.FirstOrDefault();
                var currentCustomer = context.Customers.Attach(new Customers() {CustomerID = "ALFKI"});

                var currentCustomerOrdersQuery = context.Orders.Where(o => o.Customers.CustomerID == currentCustomer.CustomerID);
                var currentCustomerOrders = currentCustomerOrdersQuery.ToList();

                foreach (var currentCustomerOrder in currentCustomerOrders)
                {
                    PrintOrder(currentCustomerOrder);
                }
            }
        }

        private static void PrintOrder(Orders currentCustomerOrder)
        {
            Console.WriteLine("Order Id {0}, shipped to: {4} {1} {2} {3}, on date {5}", currentCustomerOrder.OrderID,
                currentCustomerOrder.ShipAddress, currentCustomerOrder.ShipCity, currentCustomerOrder.ShipCountry,
                currentCustomerOrder.ShipName, currentCustomerOrder.ShippedDate);
        }
    }
}
