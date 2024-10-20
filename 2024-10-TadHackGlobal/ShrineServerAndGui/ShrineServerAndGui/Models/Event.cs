using System;

namespace ShrineServerAndGui.Models;

public class Event
{
    public string Timestamp { get; set; } = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff");
    
    public string Type { get; set; }
    
    public string Data { get; set; }
}