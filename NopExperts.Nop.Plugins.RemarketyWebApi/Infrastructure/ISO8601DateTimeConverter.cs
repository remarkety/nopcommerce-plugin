using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NopExperts.Nop.Plugins.RemarketyWebApi.Infrastructure
{
    public class Iso8601DateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.Parse(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("s"));
        }
    }
}