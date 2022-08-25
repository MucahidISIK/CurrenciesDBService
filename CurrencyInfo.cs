using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CurrenciesDBService
{
    public partial class CurrencyInfo
    {
        [JsonProperty("Developer", NullValueHandling = NullValueHandling.Ignore)]
        public Developer Developer { get; set; }

        [JsonProperty("TCMB_AnlikKurBilgileri", NullValueHandling = NullValueHandling.Ignore)]
        public TcmbAnlikKurBilgileri[] TcmbAnlikKurBilgileri { get; set; }
    }

    public partial class Developer
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("website", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Website { get; set; }

        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
    }

    public partial class TcmbAnlikKurBilgileri
    {
        [JsonProperty("Isim", NullValueHandling = NullValueHandling.Ignore)]
        public string Isim { get; set; }

        [JsonProperty("CurrencyName", NullValueHandling = NullValueHandling.Ignore)]
        public string CurrencyName { get; set; }

        [JsonProperty("ForexBuying", NullValueHandling = NullValueHandling.Ignore)]
        public double? ForexBuying { get; set; }

        [JsonProperty("ForexSelling", NullValueHandling = NullValueHandling.Ignore)]
        public double? ForexSelling { get; set; }

        [JsonProperty("BanknoteBuying", NullValueHandling = NullValueHandling.Ignore)]
        public BanknoteBuying? BanknoteBuying { get; set; }

        [JsonProperty("BanknoteSelling", NullValueHandling = NullValueHandling.Ignore)]
        public BanknoteBuying? BanknoteSelling { get; set; }

        [JsonProperty("CrossRateUSD", NullValueHandling = NullValueHandling.Ignore)]
        public BanknoteBuying? CrossRateUsd { get; set; }

        [JsonProperty("CrossRateOther", NullValueHandling = NullValueHandling.Ignore)]
        public BanknoteBuying? CrossRateOther { get; set; }
    }

    public partial struct BanknoteBuying
    {
        public double? Double;
        public string String;

        public static implicit operator BanknoteBuying(double Double) => new BanknoteBuying { Double = Double };
        public static implicit operator BanknoteBuying(string String) => new BanknoteBuying { String = String };
    }

    public partial class CurrencyInfo
    {
        public static CurrencyInfo FromJson(string json) => JsonConvert.DeserializeObject<CurrencyInfo>(json, CurrenciesDBService.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CurrencyInfo self) => JsonConvert.SerializeObject(self, CurrenciesDBService.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                BanknoteBuyingConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class BanknoteBuyingConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(BanknoteBuying) || t == typeof(BanknoteBuying?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                case JsonToken.Float:
                    var doubleValue = serializer.Deserialize<double>(reader);
                    return new BanknoteBuying { Double = doubleValue };
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    return new BanknoteBuying { String = stringValue };
            }
            throw new Exception("Cannot unmarshal type BanknoteBuying");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (BanknoteBuying)untypedValue;
            if (value.Double != null)
            {
                serializer.Serialize(writer, value.Double.Value);
                return;
            }
            if (value.String != null)
            {
                serializer.Serialize(writer, value.String);
                return;
            }
            throw new Exception("Cannot marshal type BanknoteBuying");
        }

        public static readonly BanknoteBuyingConverter Singleton = new BanknoteBuyingConverter();
    }
}