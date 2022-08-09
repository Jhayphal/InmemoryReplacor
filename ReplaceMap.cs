using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InmemoryReplacor
{
  public sealed class ReplaceMap
  {
    [JsonProperty("cases", Required = Required.Always)]
    public Case[] Cases { get; set; }

    public static ReplaceMap FromJson(string json) => JsonConvert.DeserializeObject<ReplaceMap>(json, InmemoryReplacor.Converter.Settings);
  }

  public sealed class Case
  {
    [JsonProperty("expression", Required = Required.Always)]
    public string Expression { get; set; }

    [JsonProperty("replace_group_id", Required = Required.Always)]
    public int ReplaceGroupId { get; set; }

    [JsonProperty("replace_to", Required = Required.Always)]
    public string ReplaceTo { get; set; }
  }

  internal static class Converter
  {
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
      MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
      DateParseHandling = DateParseHandling.None,
      Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
  }
}
