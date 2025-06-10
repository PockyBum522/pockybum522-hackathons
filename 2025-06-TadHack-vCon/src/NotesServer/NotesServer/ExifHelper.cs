using System.Globalization;
using ExifLibrary;
using NotesServer.Models;

namespace NotesServer;

public static class ExifHelper
{
    public static ImageExifData ExtractFromImage(string filePath)
    {
        var returnData =  new ImageExifData();
        var file = ImageFile.FromFile(filePath);
        
        // the flash tag's value is an enum
        var dateTimeTaken = file.Properties.Get<ExifDateTime>(ExifTag.DateTime);

        returnData.TakenAt =  dateTimeTaken;
        // GPS latitude is a custom type with three rational values
        // representing degrees/minutes/seconds of the latitude 
        var latTag = file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLatitude);
        returnData.GpsLatitude = latTag.ToFloat().ToString(CultureInfo.InvariantCulture);
        var longTag = file.Properties.Get<GPSLatitudeLongitude>(ExifTag.GPSLongitude);
        returnData.GpsLongitude = longTag.ToFloat().ToString(CultureInfo.InvariantCulture);
        
        return returnData;
    }
}