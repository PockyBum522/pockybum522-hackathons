namespace AoC_2022_CSharp.Models;

public class FilesystemObject
{
    public FilesystemObject(string name, FilesystemObject? parent, FilesystemObjectType objectType, int fileSize = 0)
    {
        Name = name;
        Parent = parent;
        ObjectType = objectType;
        FileSize = fileSize;
    }
    
    public string Name { get; set; } = "";
    
    public FilesystemObject? Parent { get; set; }

    public List<FilesystemObject> Children { get; set; } = new();
    
    public readonly FilesystemObjectType ObjectType;

    public int FileSize { get; set; } 
}