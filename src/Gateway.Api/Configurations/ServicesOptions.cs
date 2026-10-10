namespace Gateway.Api.Configurations
{
    public class ServicesOptions
    {
        /// <summary>
        /// Esta constante esta asociada al appSettingJson
        /// </summary>
        public const string SectionName = "Services";

        public string Catalog { get; set; } = string.Empty;
        public string Orders { get; set; } = string.Empty;

    }
}
