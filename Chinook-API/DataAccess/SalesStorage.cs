using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chinook_API.Models;
using System.Data.SqlClient;


namespace Chinook_API.DataAccess
{
    public class SalesStorage
    {

        private const string ConnectionString = "Server=(local);Database=Chinook;Trusted_Connection=True;";

        public List<SalesAgent> GetById()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"select FullName = e.FirstName + ' ' + e.LastName, Invoice.InvoiceId
                                      from Customer c
                                      inner join Employee e
                                      on c.SupportRepId = e.EmployeeId
                                      join Invoice
                                      on c.CustomerId = Invoice.CustomerId";

                var reader = command.ExecuteReader();
                var team = new List<SalesAgent>();
                while (reader.Read())
                {
                    var salesAgent = new SalesAgent
                    {
                        InvoiceId = (int)reader["InvoiceId"],
                        FullName = reader["FullName"].ToString()
                    };
                    team.Add(salesAgent);
                }
                return team;
            }
        }
        public List<CustomerTotal> GetByTotalId()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"select i.Total, CustomerName = c.FirstName + ' ' + c.LastName, c.Country, SalesAgent = e.FirstName + ' '+ e.LastName
                                        from Customer c
                                        inner join Employee e
                                        on c.SupportRepId = e.EmployeeId
                                        join Invoice i
                                        on c.CustomerId = i.CustomerId";

                var reader = command.ExecuteReader();
                var customer = new List<CustomerTotal>();
                while (reader.Read())
                {
                    var customerTotal = new CustomerTotal
                    {
                        Total = (decimal) reader ["Total"],
                        CustomerName = reader ["CustomerName"].ToString(),
                        Country = reader ["Country"].ToString(),
                        SalesAgent = reader["SalesAgent"].ToString()
                    };
                    customer.Add(customerTotal);
                }
                return customer;
            }
        }

        public int GetByLineId(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            { 
                connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"select count(*) InvoiceLineId
                                    from InvoiceLine i
                                    where InvoiceId = @id";

            command.Parameters.AddWithValue("@id", id);

            int count = Convert.ToInt32(command.ExecuteScalar());

                return count;
            }
        } 

        public bool AddInvoice(Invoice invoice)
        {
            using (var connection= new SqlConnection(ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO [dbo].[Invoice]([CustomerId],[InvoiceDate],[BillingAddress],[BillingCity], [BillingState],[BillingCountry],[BillingPostalCode],[Total])
                                        VALUES (@CustomerId, @InvoiceDate, @BillingAddress, @BillingCity, @BillingState, @BillingCountry, @BillingPostalCode, @Total)";

                command.Parameters.AddWithValue(@"customerId", invoice.CustomerId);
                command.Parameters.AddWithValue(@"invoiceDate", invoice.InvoiceDate);
                command.Parameters.AddWithValue(@"billingAddress", invoice.BillingAddress);
                command.Parameters.AddWithValue(@"billingCity", invoice.BillingCity);
                command.Parameters.AddWithValue(@"billingState",invoice.BillingState);
                command.Parameters.AddWithValue(@"billingCountry", invoice.BillingCountry);
                command.Parameters.AddWithValue(@"billingPostalCode", invoice.BillingPostalCode);
                command.Parameters.AddWithValue(@"total", invoice.Total);



                var result = command.ExecuteNonQuery();

                return result == 1;
            }
        }

        public bool UpdateEmployeesName(int id, string fName, string lName)
        {
            using (var ls = new SqlConnection(ConnectionString))
            {
                ls.Open();
                var command = ls.CreateCommand();

                command.CommandText = @"UPDATE [dbo].Employee
                                      set FirstName = @fName,  LastName = @lName
                                      where EmployeeId = @id";

                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@fName", fName);
                command.Parameters.AddWithValue("@lName", lName);

                int result = command.ExecuteNonQuery();
                return result == 1;
            }
        }
    }
}
