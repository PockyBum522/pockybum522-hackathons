using Newtonsoft.Json;
using NotesServer.AppLogistics;
using Serilog;

namespace NotesServer;

internal static class Program
{
    private static ILogger? _logger;

    internal static void Main(string[] args)
    {
        _logger = BuildLogger();
        
        // New up things that we need to inject logger into
        var exifExtractor = new ExifDataExtractor(_logger);
        var noteVconBuilder = new NoteVconBuilder(_logger);
        
        var imagePath = ApplicationPathBuilder.GetExampleImagePathPerMachine(Environment.UserName);
        
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
                Path.Join(ApplicationPathBuilder.GetLogPathPerMachine(Environment.UserName), "log_.log"), rollingInterval: RollingInterval.Day)
            .WriteTo.Debug()
            .CreateLogger();

        return appLogger;
    }
}