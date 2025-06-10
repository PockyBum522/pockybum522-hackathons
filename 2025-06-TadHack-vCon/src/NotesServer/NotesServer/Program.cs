using System.Net.Http.Headers;
using System.Text;
using JetBrains.Annotations;
using Newtonsoft.Json;
using NotesServer.AppLogistics;
using NotesServer.Models;
using Serilog;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace NotesServer;

static class Program
{
    private static ILogger _logger = BuildLogger();
    private static ApplicationPathBuilder _appPathBuilder;

    internal static void Main(string[] args)
    {
        // New up things that we need to inject logger into
        _appPathBuilder = new ApplicationPathBuilder(_logger);
        var exifExtractor = new ExifDataExtractor(_logger);
        var noteVconBuilder = new NoteVconBuilder(_logger);
        
        var imagePath = _appPathBuilder.GetExampleImagePathPerMachine(Environment.UserName);
        
        // Working deserialization of fake OpenAI Query JSON so we don't use tokens while experimenting:
        // var jsonResponse = await QueryOpenAi(imagePath);
        // var jsonResponse = ExampleData.ExampleOpenAiJsonResponse;
        // var nativeResponse = JsonConvert.DeserializeObject<OpenAiResponse>(jsonResponse);
        // Console.WriteLine(nativeResponse.Choices.FirstOrDefault().Message.Content); // David is a bad influence
        
        // Working deserialization to native vCon:
        // var nativeVcon = JsonConvert.DeserializeObject<VconRoot>(ExampleData.ExampleVcon);
        // Console.WriteLine(nativeVcon.Parties);
        
        // EXIF Data extraction from image (like extracting vanilla)
        var exifData = exifExtractor.FromImage(imagePath);
        _logger.Debug("lat: {GpsLatitude}, long: {GpsLongitude}, date: {TakenAt}",
            exifData.GpsLatitude, exifData.GpsLongitude, exifData.TakenAt);
        
        var testVcon = noteVconBuilder.GenerateInitialVconForNote(
            "P/NO. SN20P34496     FRU NO. 01YP680\\nLCFC P/N  PK131671B00\\nSG-90850-XUA  01  NUM-BL US", exifData);
        
        var testVconJson = JsonConvert.SerializeObject(testVcon, Formatting.Indented);
        _logger.Debug("Test vCon JSON: {TestVconJson}", testVconJson);
    }
    
    private static ILogger BuildLogger()
    {
        var appLogger = new LoggerConfiguration()
            .Enrich.WithProperty("VconNotesServerApplication", "VconNotesServerSerilogContext")
            //.MinimumLevel.Information()
            .MinimumLevel.Debug()
            .WriteTo.File(
                Path.Join(_appPathBuilder.GetLogPathPerMachine(Environment.UserName), "log_.log"), rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();

        return appLogger;
    }
}