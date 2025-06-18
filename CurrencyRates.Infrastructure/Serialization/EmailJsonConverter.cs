using System.Text.Json;
using System.Text.Json.Serialization;
using CurrencyRates.Domain.Currency.ValueObjects.Auth;

namespace CurrencyRates.Infrastructure.Serialization;

public class EmailJsonConverter : JsonConverter<Email>
{
    public override Email Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var emailStr = reader.GetString();
        return Email.Create(emailStr).Value; 
    }

    public override void Write(Utf8JsonWriter writer, Email value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Value);
    }
}