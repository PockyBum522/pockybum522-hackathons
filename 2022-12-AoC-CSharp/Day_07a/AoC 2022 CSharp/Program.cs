using AoC_2022_CSharp.Models;
using Serilog;

namespace AoC_2022_CSharp;

internal static class Program
{
    private static int _totalFolderSizesForAnswer = 0;
    
    private static readonly ILogger Logger = LoggerSetup.BuildLogger(); 
    
    public static void Main()
    {
        var rawLines = RawData.ActualData
            .Split(Environment.NewLine);
        
        var readingLsLinesMode = false;

        var rootNode = new FilesystemObject("/", null, FilesystemObjectType.Folder);
        var currentFolder = rootNode;
        
        foreach (var line in rawLines)
        {
            if (line.StartsWith("$ "))
                readingLsLinesMode = false;
            
            if (line.StartsWith("$ cd"))
            {
                var cdFolderName = line.Replace("$ cd ", "");
                
                if (cdFolderName == "/") continue; // Since we make root above

                if (cdFolderName == "..")
                {
                    currentFolder = currentFolder?.Parent;
                    
                }
                else
                {
                    // Make new directory object to represent dir we're cd'ing to
                    var folderToCdInto =
                        new FilesystemObject(cdFolderName, currentFolder, FilesystemObjectType.Folder);

                    currentFolder?.Children.Add(folderToCdInto);

                    currentFolder = folderToCdInto;   
                }
            }

            if (readingLsLinesMode)
            {
                // Parse line that is file or folder data into current folder
                if (line.StartsWith("dir"))
                {
                    var dirName = line.Split(' ')[1];
                    
                    // If dir, add folder if it's not already present in children (Check names)
                    if (!DirectoryPresentIn(currentFolder?.Children!, dirName))
                    {
                        currentFolder?.Children.Add(
                            new FilesystemObject(dirName, currentFolder, FilesystemObjectType.Folder));
                    }
                }
                else
                {
                    // It's a file
                    var fileSize = int.Parse(line.Split(' ')[0]);
                    var fileName = line.Split(' ')[1];
                    
                    currentFolder?.Children.Add(
                        new FilesystemObject(fileName, currentFolder, FilesystemObjectType.File, fileSize));
                }
            }
            
            if (line.StartsWith("$ ls"))
            {
                // Get ready to read some file and folder info on the next lines
                readingLsLinesMode = true;
            }

            var parentName = currentFolder?.Parent?.Name ?? "None";
            Logger.Debug("Raw line: {RawLine} | Which makes currentFolder {CurrentFolderName} | Current folder parent: {CurrentFolderParent}", line, currentFolder.Name, parentName);
        }

        // Just checking things
        foreach (var currentFolderChild in currentFolder.Children)
        {
            Logger.Debug("Current folder child: {Name} | Type: {ObjectType} | Size: {Size}", currentFolderChild.Name, currentFolderChild.ObjectType, currentFolderChild.FileSize);
        }

        var totalDiskCapacity = 70000000;

        var spaceNeededForUpdate = 30000000;

        var usedSpace = CalculateDirectorySize(rootNode);

        var currentSpaceFree = totalDiskCapacity - usedSpace;

        var smallestFolderSize = spaceNeededForUpdate - currentSpaceFree;
        
        // Now calculate and show the sizes of all folders in the filesystem
        WalkFolderAndSizeAllSubfolders(rootNode);
    }

    private static bool DirectoryPresentIn(List<FilesystemObject> currentFolderChildren, string dirName)
    {
        foreach (var currentFolderChild in currentFolderChildren)
        {
            if (currentFolderChild.Name == dirName)
            {
                return true;
            }
        }

        return false;
    }
    
    private static int CalculateDirectorySize(FilesystemObject folder)
    {
        var totalSize = 0;
        
        foreach (var currentChild in folder.Children)
        {
            totalSize += currentChild.FileSize;

            if (currentChild.ObjectType == FilesystemObjectType.Folder)
            {
                totalSize += CalculateDirectorySize(currentChild);
            }
        }

        return totalSize;
    }
    
    private static void WalkFolderAndSizeAllSubfolders(FilesystemObject folder)
    {
        foreach (var currentChild in folder.Children)
        {
            if (currentChild.ObjectType != FilesystemObjectType.Folder) continue;

            var folderSize = CalculateDirectorySize(currentChild);

            if (folderSize <= 100000) _totalFolderSizesForAnswer += folderSize;
            
            Logger.Debug("Folder: {FolderName} contains {Size} bytes", currentChild.Name, folderSize);

            // Now do any subfolders
            WalkFolderAndSizeAllSubfolders(currentChild);
        }
        
        Logger.Information("Total cumulative size of any folders under 10,000 bytes: {Answer}", _totalFolderSizesForAnswer);
    }
}