﻿using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;
using webapi.Common;

namespace webapi.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = Consts.DateTimeFormatPattern;

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.ParseExact(reader.GetString()!, Format, CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
