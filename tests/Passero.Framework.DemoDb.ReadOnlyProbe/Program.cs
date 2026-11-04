using System.Data;
using Microsoft.Data.SqlClient;
using Passero.Framework.Base;
using PasseroDemo.Models;

// Connection credentials must NEVER be included in logs or in committed files.
var supplied = Environment.GetEnvironmentVariable("PASSERO_DEMO_DB_CONNECTION_STRING");
if (string.IsNullOrWhiteSpace(supplied))
{
    Console.Error.WriteLine("SKIPPED: PASSERO_DEMO_DB_CONNECTION_STRING is not configured. No database test was executed.");
    return 2;
}

int checks = 0;
try
{
    var builder = new SqlConnectionStringBuilder(supplied)
    {
        ApplicationName = "Passero.Model.ReadOnlyProbe",
        ConnectTimeout = 12
    };
    // The SQL connection is used exclusively for SELECT; a read-only login is recommended.
    using var connection = new SqlConnection(builder.ConnectionString);
    await connection.OpenAsync();
    if (!string.Equals(connection.Database, "pubs", StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException("The supplied connection does not target the expected demo database 'pubs'.");

    // Check expected Pubs schema before materializing models.
    using (var command = connection.CreateCommand())
    {
        command.CommandText = @"SELECT COUNT(*)
FROM sys.tables
WHERE name IN ('authors','publishers','titles')";
        var count = Convert.ToInt32(await command.ExecuteScalarAsync());
        if (count != 3)
            throw new InvalidOperationException("Demo database does not contain the expected authors/publishers/titles tables.");
    }
    checks++;
    Console.WriteLine("PASS demo Pubs schema");

    using (var db = new DapperDbContext(connection, ownsConnection: false))
    {
        var authors = db.SqlQuery<Author>("SELECT TOP (3) * FROM dbo.authors ORDER BY au_id").ToList();
        if (authors.Any(a => string.IsNullOrWhiteSpace(a.au_id)))
            throw new InvalidOperationException("Dapper Author materialization returned blank identifiers.");
        checks++;
        Console.WriteLine($"PASS Dapper Author SELECT (rows: {authors.Count})");

        var publishers = db.SqlQuery<Publisher>("SELECT TOP (3) * FROM dbo.publishers ORDER BY pub_id").ToList();
        if (publishers.Any(p => string.IsNullOrWhiteSpace(p.pub_id)))
            throw new InvalidOperationException("Dapper Publisher materialization returned blank identifiers.");
        checks++;
        Console.WriteLine($"PASS Dapper Publisher SELECT (rows: {publishers.Count})");
    }

    // A separate ORM context checks EF Core mapping on the identical demo model.
    // The context owns its connection; only reads are performed.
    using (var ef = ORMContextFactory.Create(ORMType.EntityFrameworkCore, builder.ConnectionString, new[] { typeof(Author) }))
    {
        var authors = await ef.ToListAsync(ef.Set<Author>().Take(3));
        if (authors.Any(a => string.IsNullOrWhiteSpace(a.au_id)))
            throw new InvalidOperationException("EF Core Author materialization returned blank identifiers.");
        checks++;
        Console.WriteLine($"PASS EF Core Author SELECT (rows: {authors.Count})");
    }

    Console.WriteLine($"DEMO DB READ-ONLY PROBE: Passed={checks}; Failed=0");
    return 0;
}
catch (Exception ex)
{
    // Do not echo exception messages: SQL provider errors can contain server details.
    Console.Error.WriteLine($"DEMO DB READ-ONLY PROBE FAILED at check {checks + 1}: {ex.GetType().Name}");
    return 1;
}
