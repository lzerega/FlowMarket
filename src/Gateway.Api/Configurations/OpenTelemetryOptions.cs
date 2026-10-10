namespace Gateway.Api.Configurations
{
    public class OpenTelemetryOptions
    {
        /// <summary>
        /// Esta constante esta asociada al appSettingJson
        /// </summary>
        public const string SectionName = "OpenTelemetry";

        public string Endpoint { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;

    }
}
