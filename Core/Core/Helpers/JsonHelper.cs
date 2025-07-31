using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace Core.Helpers
{
    public static class JsonHelper
    {
        private static Newtonsoft.Json.JsonSerializerSettings _newtonSoftSettings => new Newtonsoft.Json.JsonSerializerSettings
        {
            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat,
            DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include,
            MetadataPropertyHandling = Newtonsoft.Json.MetadataPropertyHandling.Default,
            MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore,
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None,
            //ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore,
            //MaxDepth = 2,
            
        };

        private static System.Text.Json.JsonSerializerOptions _SystemTextSettings => new System.Text.Json.JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never,
            PropertyNameCaseInsensitive = true,
            MaxDepth = 2,
            ReferenceHandler = ReferenceHandler.Preserve,
            //PropertyNamingPolicy = JsonNamingPolicy.()
        };

        public static async Task<System.IO.Stream> ToJsonStream(System.IO.Stream stream, object obj)
        {
            if (obj == null) return null;

            try
            {
                await System.Text.Json.JsonSerializer.SerializeAsync(stream, obj, _SystemTextSettings);
            }
            catch (Exception ex)
            {
                throw;
            }
            return stream;
        }

        public static string ToJsonString(object obj, System.Text.Json.Serialization.JsonConverter[] jsonConverters = null)
        {
            if (obj == null) return string.Empty;

            try
            {
                if (jsonConverters is not null)
                {
                    var systemTextSettings = _SystemTextSettings;
                    foreach (var converter in jsonConverters)
                        systemTextSettings.Converters.Add(converter);

                    return System.Text.Json.JsonSerializer.Serialize(obj, obj.GetType(), systemTextSettings);

                }

                return System.Text.Json.JsonSerializer.Serialize(obj, obj.GetType(), _SystemTextSettings);
                //return Utf8Json.JsonSerializer.ToJsonString(obj);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj, _newtonSoftSettings);
            }
        }

        public static T FromJsonString<T>(string json, System.Text.Json.Serialization.JsonConverter[] jsonConverters = null)
        {

            try
            {
                if (jsonConverters is not null)
                {
                    var systemTextSettings = _SystemTextSettings;
                    foreach (var converter in jsonConverters)
                        systemTextSettings.Converters.Add(converter);

                    return System.Text.Json.JsonSerializer.Deserialize<T>(json, systemTextSettings);
                }

                return System.Text.Json.JsonSerializer.Deserialize<T>(json, _SystemTextSettings);
                //return Utf8Json.JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, _newtonSoftSettings);
            }
        }

        public static string GetItemByPathAsString(string jsonData, string jsonPath)
        {
            var result = GetItemByPath(jsonData, jsonPath);
            return (string)result;
        }

        public static IEnumerable<string> GetItemByPathAsArray(this string jsonData, string jsonPath)
        {
            var jtoken = GetItemByPath(jsonData, jsonPath);

            if (jtoken is null) return Enumerable.Empty<string>();

            var result = new List<string>();
            foreach (var item in jtoken.ToList())
                result.Add(item.ToString());

            return result;
        }

        public static JToken GetItemByPath(string jsonData, string jsonPath)
        {
            var root = Newtonsoft.Json.Linq.JToken.Parse(jsonData);
            return root.SelectToken(jsonPath);
        }

        public static IEnumerable<string> GetItemsByPath(string jsonData, string jsonPath)
        {
            var root = Newtonsoft.Json.Linq.JToken.Parse(jsonData);
            return root.SelectToken(jsonPath).Select(e => (string)e).ToList();
        }
    }

}
