using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;
using Bitmap = Avalonia.Media.Imaging.Bitmap;


namespace Aoc2023CSharp.Logic;

public static class AvaloniaImageHelper
{
    [SupportedOSPlatform("windows")]
    public static Bitmap ConvertToAvaloniaBitmap(Image bitmap)
    {
        var bitmapTmp = new System.Drawing.Bitmap(bitmap);
        
        var bitmapdata = bitmapTmp.LockBits(
            new Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        
        var bitmap1 = new Bitmap(Avalonia.Platform.PixelFormat.Bgra8888, Avalonia.Platform.AlphaFormat.Premul,
            bitmapdata.Scan0,
            new Avalonia.PixelSize(bitmapdata.Width, bitmapdata.Height),
            new Avalonia.Vector(96, 96),
            bitmapdata.Stride);
        
        bitmapTmp.UnlockBits(bitmapdata);
        bitmapTmp.Dispose();
        
        return bitmap1;
    }
    
    [SupportedOSPlatform("windows")]
    public static Bitmap GetEmptyBitmap()
    {
        var bitmapTmp = new System.Drawing.Bitmap(10, 10);
        
        var bitmapdata = bitmapTmp.LockBits(
            new Rectangle(0, 0, bitmapTmp.Width, bitmapTmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
        
        var bitmap1 = new Bitmap(Avalonia.Platform.PixelFormat.Bgra8888, Avalonia.Platform.AlphaFormat.Premul,
            bitmapdata.Scan0,
            new Avalonia.PixelSize(bitmapdata.Width, bitmapdata.Height),
            new Avalonia.Vector(96, 96),
            bitmapdata.Stride);
        
        bitmapTmp.UnlockBits(bitmapdata);
        bitmapTmp.Dispose();
        
        return bitmap1;
    }
}