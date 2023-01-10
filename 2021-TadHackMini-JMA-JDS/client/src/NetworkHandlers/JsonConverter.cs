using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Ink;
using Newtonsoft.Json;

namespace TelePaperProject.NetworkHandlers;

public class JsonConverter
{
    public static string StrokesToJson(StrokeCollection strokes)
    {
        return JsonConvert.SerializeObject(strokes);
    }
    public static StrokeCollection JsonToStrokes(string strokesJson)
    {
        var returnStrokeCollection = new StrokeCollection();
        
        return JsonConvert.DeserializeObject<StrokeCollection>(strokesJson) ?? new StrokeCollection();
    }
}