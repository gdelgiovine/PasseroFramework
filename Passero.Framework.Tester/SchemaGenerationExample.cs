using System;
using System.Data;
using System.Data.Common;
using Passero.Framework;
using PasseroDemo.Models;

namespace Passero.Framework.Tester
{
    internal static class SchemaGenerationExample
    {
        public static void Run()
        {
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PasseroDemo;Integrated Security=True;Connect Timeout=30;";
            using (DbConnection connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    var statements = DataBaseHelper.GetCreateSchemaStatementsForModel<TestTable>(connection, "PasseroDemo", "dbo", true, true, true, true);
                    foreach (var statement in statements)
                    {
                        Console.WriteLine(statement);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
