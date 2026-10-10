using System.Data.Common;

namespace Orders.Api.Configurations
{
    public class DatabaseOptions
    {
        public const string SectionName = "Database";

        public string ConnectionString { get; set; } = string.Empty;
    }
}
