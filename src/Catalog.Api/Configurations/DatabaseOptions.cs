using System.Data.Common;

namespace Catalog.Api.Configurations
{
    public class DatabaseOptions
    {
        public const string SectionName = "Database";

        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 5432;
        public string DatabaseName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Propiedad que genera automáticamente la cadena de conexión para Npgsql
        public string ConnectionString =>
            $"Host={Host};Port={Port};Database={DatabaseName};Username={Username};Password={Password}"; 
        
        public void Configure(String servername, String databaseName, Int16 port, String username, String password)
        {
            this.Host = servername;
            this.DatabaseName = databaseName;
            this.Port = port;
            this.Username = username;
            this.Password = password;
        }
    }
}
