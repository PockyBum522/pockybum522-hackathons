using AoC_2023_CSharp.Models;
using Serilog;
using Serilog.Events;

namespace AoC_2023_CSharp;

internal static class Program
{
    private static readonly ILogger Logger = LoggerSetup.ConfigureLogger()
            .MinimumLevel.Information()
            .CreateLogger(); 
    
    public static void Main()
    {
        Logger.Information("Starting!");
        
        var rawLines = RawData.ActualData01
            .Split(",");
        
        // Each step begins with a sequence of letters that indicate the label of the lens on which the step operates.
        // The result of running the HASH algorithm on the label indicates the correct box for that step.
        //
        //     The label will be immediately followed by a character that indicates the operation to perform: either an equals sign (=) or a dash (-).
        //
        //     If the operation character is a dash (-), go to the relevant box and remove the lens with the given label if it is present in
        //      the box. Then, move any remaining lenses as far forward in the box as they can go without changing their order, filling any space
        //      made by removing the indicated lens. (If no lens in that box has the given label, nothing happens.)
        //
        // If the operation character is an equals sign (=), it will be followed by a number indicating the focal length of the lens that
        // needs to go into the relevant box; be sure to use the label maker to mark the lens with the label given in the beginning of the
        // step so you can find it later. There are two possible situations:
        //
        // If there is already a lens in the box with the same label, replace the old lens with the new lens: remove the old lens and put
        // the new lens in its place, not moving any other lenses in the box.
        
        //     If there is not already a lens in the box with the same label, add the lens to the box immediately behind any lenses
        //      already in the box. Don't move any of the other lenses when you do this. If there aren't any lenses in the box, the new
        //      lens goes all the way to the front of the box.
        //
        //     Here is the contents of every box after each step in the example initialization sequence above:

        var boxes = new List<List<Lens>>();

        for (int i = 0; i < 256; i++)
        {
            boxes.Add(new List<Lens>());
        }
        
        foreach (var step in rawLines)
        {
            if (step.Contains("="))
            {
                var label = step.Split("=")[0];
                
                var boxNumber = HashAlgorithm(label);

                var focalLength = step.Split("=")[1];

                if (!BoxContainsLens(boxes[boxNumber], label))
                {
                    boxes[boxNumber].Add(new Lens()
                    {
                        FocalLength = int.Parse(focalLength),
                        Label = label
                    });    
                }
                else
                {
                    ReplaceLensInBox(boxes[boxNumber], label, focalLength);
                }
                    
            }
            
            if (step.Contains("-"))
            {
                var label = step.Split("-")[0];
                
                var boxNumber = HashAlgorithm(label);

                for (var i = 0; i < boxes[boxNumber].Count; i++)
                {
                    if (boxes[boxNumber][i].Label == label)
                    {
                        Logger.Debug("Removing lens with label {Label} from box {BoxNum}", label, boxNumber);
                        
                        boxes[boxNumber].RemoveAt(i);
                    }
                        
                }
            }
            
            //answerTotal += HashAlgorithm(step);

            //answerTotal += 1;
        }

        DebugLogBoxes(boxes);

        var answerTotal = CalculateTotal(boxes);
        
        Logger.Information("Answer: {AnswerTotal}", answerTotal);
        
        // Make sure if we log on other threads right before the program ends, we can see it
        Log.CloseAndFlush();
        Task.Delay(2000);
    }

    private static long CalculateTotal(List<List<Lens>> boxes)
    {
        var total = 0;
        
        for (var boxIndex = 0; boxIndex < boxes.Count; boxIndex++)
        {
            var box = boxes[boxIndex];
            
            for (var lensIndex = 0; lensIndex < box.Count; lensIndex++)
            {
                var thisLensTotal = 1;
                
                // One plus the box number of the lens in question.
                thisLensTotal *= (1 + boxIndex);

                // The slot number of the lens within the box: 1 for the first lens, 2 for the second lens, and so on.
                thisLensTotal *= (1 + lensIndex);

                // The focal length of the lens.
                thisLensTotal *= box[lensIndex].FocalLength;

                total += thisLensTotal;
            }
        }

        return total;
    }

    private static void ReplaceLensInBox(List<Lens> box, string label, string newFocalLength)
    {
        for (var i = 0; i < box.Count; i++)
        {
            var lens = box[i];

            if (lens.Label == label)
            {
                lens.FocalLength = int.Parse(newFocalLength);
                
                return;
            }
        }
    }

    private static bool BoxContainsLens(List<Lens> box, string label)
    {
        foreach (var lens in box)
        {
            if (lens.Label == label) 
                return true;
        }

        return false;
    }

    private static void DebugLogBoxes(List<List<Lens>> boxes)
    {
        for (var i = 0; i < boxes.Count; i++)
        {
            var box = boxes[i];
            
            Logger.Debug("In box: {I}", i);

            for (var lensIndex = 0; lensIndex < box.Count; lensIndex++)
            {
                var lens = box[lensIndex];
                
                Logger.Debug("[{Label} {Focal}]", lens.Label, lens.FocalLength);
            }
            
            Logger.Debug("");
        }
    }

    public static int HashAlgorithm(string stringToHash)
    {
        var returnValue = 0;

        foreach (var letter in stringToHash)
        {
            var letterValue = (int)letter;

            returnValue += letterValue;

            Logger.Debug("letter value: {R}", returnValue);

            returnValue *= 17;
            
            Logger.Debug("After * 17: {R}", returnValue);

            returnValue = returnValue % 256;
            
            Logger.Debug("After mod: {R}", returnValue);
        }

        return returnValue;
    }
}
