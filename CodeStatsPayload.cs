using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Celeste.Mod.CeleStats;

[Serializable]
public class Exp
{
    public Exp(string language, int experience)
    {
        Language = language;
        Experience = experience;
    }
    [JsonProperty("language")] string Language;
    [JsonProperty("xp")] int Experience;
}

[Serializable]
public class Experiences : List<Exp>
{
    public static Experiences Of(KeyValuePair<string, int>[] keyValuePairs)
    {
        var exp = new Experiences();
        foreach (var keyValuePair in keyValuePairs)
        {
            exp.Add(new Exp(keyValuePair.Key, keyValuePair.Value));
        }
        return exp;
    }
}

[Serializable]
public class CodeStatsPayload
{
    [JsonProperty("coded_at")] public string Time;
    [JsonProperty("xps")] public Experiences Experiences;
}