using System.Globalization;
using ExifLibrary;
using NotesServer.Models;
using Serilog;

namespace NotesServer;

public class ExifDataExtractor
{
    private readonly ILogger _logger;

    public ExifDataExtractor(ILogger logger)
    {
        _logger = logger;
    }
    
    public ImageExifData FromImage(string filePath)
    {
        var returnData =  new ImageExifData();
        var file = ImageFile.FromFile(filePath);
        
        var dateTimeTaken = file.Properties.Get<ExifDateTime>(ExifTag.DateTime);

        returnData.TakenAt =  dateTimeTaken;
        
        var latTag = file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLatitude);
        var longTag = file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLongitude);
        
        returnData.GpsLatitude = latTag.ToFloat().ToString(CultureInfo.InvariantCulture);
        returnData.GpsLongitude = longTag.ToFloat().ToString(CultureInfo.InvariantCulture);
        
        _logger.Debug("Extracted exif data is: {@ExifData}", returnData);
        
        return returnData;
    }
}
