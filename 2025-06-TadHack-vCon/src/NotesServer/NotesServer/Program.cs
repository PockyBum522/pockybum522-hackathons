using Newtonsoft.Json;
using NotesServer.AppLogistics;
using NotesServer.Models;
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
        var markdownFileWriter = new MarkdownFileWriter(_logger);
        
        var imagePath = ApplicationPathBuilder.GetExampleImagePathPerMachine(Environment.UserName);
        
        // Working deserialization of fake OpenAI Query JSON so we don't use tokens while experimenting:
        // var jsonResponse = await QueryOpenAi(imagePath);      // Uncomment this if you want to use tokens and get a real response
        var jsonResponse = ExampleData.ExampleOpenAiJsonResponse;
        var nativeResponse = JsonConvert.DeserializeObject<OpenAiResponse>(jsonResponse);
        _logger.Debug("Response from OpenAI: {ResponseContent}", nativeResponse.Choices.FirstOrDefault().Message.Content); // David is a bad influence
        
        // EXIF Data extraction from image (like extracting vanilla)
        var exifData = exifExtractor.FromImage(imagePath);
        _logger.Debug("lat: {GpsLatitude}, long: {GpsLongitude}, date: {TakenAt}",
            exifData.GpsLatitude, exifData.GpsLongitude, exifData.TakenAt);
        
        var testVcon = noteVconBuilder.GenerateInitialVconForNote(nativeResponse.Choices.FirstOrDefault().Message.Content, exifData);
        
        var testVconJson = JsonConvert.SerializeObject(testVcon, Formatting.Indented);
        _logger.Debug("Test vCon JSON: {TestVconJson}", testVconJson);

        var notesFolderPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Notes");
        
        // We could put more vcons into this list we're passing here, and it should handle that gracefully:
        markdownFileWriter.WriteNewNote(notesFolderPath, [testVcon]);
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
