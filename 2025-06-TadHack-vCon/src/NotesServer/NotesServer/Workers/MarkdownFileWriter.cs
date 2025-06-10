using System.Text;
using NotesServer.Models;
using Serilog;

namespace NotesServer;

public class MarkdownFileWriter
{
    private readonly ILogger _logger;

    public MarkdownFileWriter(ILogger logger)
    {
        _logger = logger;
    }

    public void WriteNewNote(string path, List<VconRoot> noteContent)
    {
        // Make sure this exists
        Directory.CreateDirectory(path);
        
        // Get date/time of oldest vcon
        var sortedByDate = noteContent.OrderBy(x => x.CreatedAt);

        var fileName = "ERROR-GENERATING-NAME.md";
        
        var oldestVcon = noteContent.FirstOrDefault() ?? throw new NullReferenceException("Could not get oldest vcon information");
        
        var safeOldestTimestamp = oldestVcon.CreatedAt.ToString("dddd_d").Replace('/', '-');
        
        if (noteContent.Count < 1) throw new Exception("Could not get note content as there are no elements in passed list");
            
        // Name the note with weekday and date/time range
        if (noteContent.Count > 1)
        {
            var newestVcon = noteContent.LastOrDefault() ?? throw new NullReferenceException("Could not get oldest vcon information");
            
            var safeNewestTimestamp = newestVcon.CreatedAt.ToString("dddd_d").Replace('/', '-');
            
            fileName = $"Notes_From-{safeOldestTimestamp}_To-{safeNewestTimestamp}.md";
            
            path = Path.Join(path, fileName);
        }
        
        if (noteContent.Count == 1)
        {
            fileName = $"Notes_From-{safeOldestTimestamp}.md";
            
            path = Path.Join(path, fileName);
        }

        var noteStringBuilder = new StringBuilder(512);
        
        foreach (var noteVcon in noteContent)
        {
            var vconDialog = noteVcon.Dialog.FirstOrDefault() ?? throw new NullReferenceException("vCon first dialog element was null");

            var dialogBody = vconDialog.Body;

            var thisVconFormattedDateTime = noteVcon.CreatedAt.ToString("D");

            var firstNoteLine = $"From picture taken on {thisVconFormattedDateTime}:";
            
            noteStringBuilder.AppendLine(firstNoteLine);
            noteStringBuilder.AppendLine();
            noteStringBuilder.AppendLine(dialogBody);
            noteStringBuilder.AppendLine(Environment.NewLine + Environment.NewLine);
            
            File.WriteAllText(path, noteStringBuilder.ToString());
        }
    }
}