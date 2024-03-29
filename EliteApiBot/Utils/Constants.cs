﻿using System.Globalization;
using CsvHelper.Configuration;
using Newtonsoft.Json;

namespace EliteApiBot.Utils;

public static class Constants
{
    public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
    public const string ShortLink = "https://sapi.demb.uk/squads/now/by-tag/short/{0}";
    public const string ExtendedLink  = "https://sapi.demb.uk/squads/now/by-tag/extended/{0}?resolve_tags=true";

    public static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };

    public static readonly CsvConfiguration CsvConfiguration = new(CultureInfo.InvariantCulture)
    {
        Delimiter = ",",
        HasHeaderRecord = true,
        HeaderValidated = null,
        MissingFieldFound = null
    };
}