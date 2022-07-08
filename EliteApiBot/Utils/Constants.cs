using System.Globalization;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace Elite_API_Discord.Utils;

public static class Constants
{
    public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
    public const string ShortLink = "https://146.66.200.10/squads/now/by-tag/short/{0}";
    public const string ExtendedLink = "https://146.66.200.10/squads/now/by-tag/extended/{0}?resolve_tags=true";
    public const string CsvLink = "https://146.66.200.10/a31/CEC-list-monitoring/raw/branch/master/list.csv";

    public static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };

    public static readonly CsvConfiguration CsvConfiguration = new(CultureInfo.InvariantCulture)
    {
        Delimiter = ",",
        HasHeaderRecord = true
    };
}