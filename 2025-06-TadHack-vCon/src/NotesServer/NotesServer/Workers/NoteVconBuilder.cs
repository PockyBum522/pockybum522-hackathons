using NotesServer.Models;
using Serilog;

namespace NotesServer;

public class NoteVconBuilder(ILogger logger)
{
    private readonly ILogger _logger = logger;

    public VconRoot GenerateInitialVconForNote(string vconBody, ImageExifData exifData)
    {
        var generatedVcon = new VconRoot();

        generatedVcon.Parties.Add(
            new Party()
            {
                Name = "OpenAi",
                Role = "Transcriber"
            }
        );
        
        generatedVcon.Dialog.Add(
            new Dialog()
            {
                Body = vconBody,
                MimeType = "text/plain",
                Parties = [0],
                Start = exifData.TakenAt,
                Type = "text"
            }
        );
        
        generatedVcon.Attachments.Add(
            new Attachment()
            {
                Type = "tags",
                Body = [$"location:{exifData.GpsLatitude}, -{exifData.GpsLongitude}"],
                Encoding = "json"
            }
        );

        _logger.Debug("Generated initial note vcon: {@VconRoot}", generatedVcon);
        
        return generatedVcon;
    }
}