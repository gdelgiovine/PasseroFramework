using System;

namespace Passero.Framework.Base
{
    public class DbContextOptionsBuilder
    {
        private string _connectionString;

        /// <summary>
        /// Imposta la stringa di connessione.
        /// </summary>
        /// <param name="connectionString">La stringa di connessione.</param>
        /// <returns></returns>
        public DbContextOptionsBuilder UseConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }

        /// <summary>
        /// Ottiene la stringa di connessione configurata.
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return _connectionString;
        }

        /// <summary>
        /// Configura il contesto per utilizzare SQL Server.
        /// </summary>
        /// <param name="server">Il nome del server.</param>
        /// <param name="database">Il nome del database.</param>
        /// <param name="userId">L'ID utente (opzionale).</param>
        /// <param name="password">La password (opzionale).</param>
        /// <returns></returns>
        public DbContextOptionsBuilder UseSqlServer(string server, string database, string userId = null, string password = null, string otherparams =null)
        {
            if (string.IsNullOrEmpty(server))
                throw new ArgumentException("Sever Name cannot be empty.", nameof(server));
            if (string.IsNullOrEmpty(database))
                throw new ArgumentException("DataBase Name cannot be empty", nameof(database));

            // Costruisce la stringa di connessione
            var connectionString = $"Server={server};Database={database};";
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(password))
            {
                connectionString += $"User Id={userId};Password={password};";
            }
            else
            {
                connectionString += "Integrated Security=True;";
            }
            connectionString += string.IsNullOrEmpty(otherparams) ? "" : otherparams;
            return UseConnectionString(connectionString);
        }

        /// <summary>
        /// Configures the context to use MySQL.
        /// </summary>
        /// <param name="server">The name of the MySQL server.</param>
        /// <param name="database">The name of the MySQL database.</param>
        /// <param name="userId">The user ID for authentication.</param>
        /// <param name="password">The password for authentication.</param>
        /// <param name="port">The port number for the MySQL server (optional, default is 3306).</param>
        /// <returns>An instance of <see cref="DbContextOptionsBuilder"/> with the configured MySQL connection string.</returns>
        public DbContextOptionsBuilder UseMySql(string server, string database, string userId, string password, int port = 3306)
        {
            if (string.IsNullOrEmpty(server))
                throw new ArgumentException("Server name cannot be empty.", nameof(server));
            if (string.IsNullOrEmpty(database))
                throw new ArgumentException("Database name cannot be empty.", nameof(database));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            // Build the connection string for MySQL
            var connectionString = $"Server={server};Port={port};Database={database};User={userId};Password={password};";
            return UseConnectionString(connectionString);
        }
    }
}