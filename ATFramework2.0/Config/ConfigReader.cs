namespace ATFramework2._0.Config
{
    public class ConfigReader
    {
        public static TestSettings ReadConfig()
        {
            var configFile = File.ReadAllText("C:\\Users\\Danyil.Lipskyi\\Documents\\GitHub\\ATFramework2.0\\Demo\\appsettings.json");

            var jsonSerializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            jsonSerializeOptions.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<TestSettings>(configFile, jsonSerializeOptions);
        }
    }
}
