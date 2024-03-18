using System.Text.Json;
using System.Text.Json.Serialization;

namespace $safeprojectname$.Modules.Api.Converter
{
    public class CustomStringToDateTime : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String && reader.ValueTextEquals(string.Empty))
            {
                return null;
            }

            return JsonSerializer.Deserialize<DateTime?>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value is not null ? value.Value : (DateTime?)null, options);
        }
    }
}