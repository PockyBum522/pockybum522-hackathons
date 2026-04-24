using System.Collections.Generic;
using Newtonsoft.Json;

namespace ApparitionsAndroidClient.Models;

public class ScenarioFile
{
    [JsonProperty("scenario_version")]
    public int ScenarioVersion { get; set; }

    [JsonProperty("vcons")] public List<VconRoot> VCons { get; set; } = [];
}