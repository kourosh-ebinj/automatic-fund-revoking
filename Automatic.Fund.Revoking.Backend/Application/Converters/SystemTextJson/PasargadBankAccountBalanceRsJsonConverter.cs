using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Models.Responses.ThirdParties.Pasargad;

namespace Application.Converters.SystemTextJson
{
    //public class PasargadBankAccountBalanceRsJsonConverter : JsonConverter<PasargadBankAccountBalanceRs>
    //{
    //    private readonly JsonConverter<PasargadBankAccountBalanceRs> _valueConverter;

    //    public override PasargadBankAccountBalanceRs Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    //        if (reader.TokenType != JsonTokenType.StartObject)
    //        {
    //            throw new JsonException();
    //        }

    //        var result = new PasargadBankAccountBalanceRs();

    //        while (reader.Read())
    //        {
    //            if (reader.TokenType == JsonTokenType.EndObject)
    //                return result;

    //            // Get the key.
    //            if (reader.TokenType != JsonTokenType.PropertyName)
    //                throw new JsonException();

    //            using (JsonDocument document = JsonDocument.ParseValue(ref reader))
    //            {
    //                result.StatusCode = document.RootElement.GetProperty("").GetInt32();
    //            }

    //            string? propertyName = reader.GetString();

    //            //if (!Enum.TryParse(propertyName, ignoreCase: false, out TKey key) &&
    //            //    !Enum.TryParse(propertyName, ignoreCase: true, out key))
    //            //{
    //            //    throw new JsonException(
    //            //        $"Unable to convert \"{propertyName}\" to Enum \"{_keyType}\".");
    //            //}


    //            // Get the value.
    //            reader.Read();
    //            var value = _valueConverter.Read(ref reader, typeof(int), options)!;

    //            // Add to dictionary.
    //            result.;
    //        }

    //        throw new JsonException();
    //    }

    //    public override void Write(
    //        Utf8JsonWriter writer,
    //        PasargadBankAccountBalanceRs value,
    //        JsonSerializerOptions options) => throw new NotImplementedException();

    //    //public override bool CanConvert(Type typeToConvert) => true;
    //}
}
