using System.Drawing;
using WindowsInput;
using WindowsInput.Native;
using Clipboard;

namespace SunCtfSimonProgrammer2;

public static class Program
{
    private static readonly InputSimulator InputSim = new InputSimulator();

    public static void Main()
    {
        // Run the following in firefox's JS console
        
        // var mutationObserver = new MutationObserver(function(mutations) {
        //     mutations.forEach(function(mutation) {
        //     if (mutation.oldValue == "color: yellow;"){
        //     console.log(mutation.target.innerHTML);
        // }
        // });
        // });
        //
        // mutationObserver.observe(document.documentElement, {
        //     attributes: true,
        //     characterData: true,
        //     childList: true,
        //     subtree: true,
        //     attributeOldValue: true,
        //     characterDataOldValue: true
        // });

        
        
        // --------------------========== CONFIGURATION START ==========--------------------

        
        // --------------------========== CONFIGURATION STOP ==========--------------------
        
        Thread.Sleep(100);
        
        var mouseYStart = 1868;
        
        //var initialSleep = 3200;
        
        // MoveMouseScaled(
        //     new Point(3868, mouseYStart));
        
        while (true)
        {
            // Right click in JS console > copy all messages
            MoveMouseScaled(
            new Point(5129, 1419));
            Thread.Sleep(80);
            InputSim.Mouse.LeftButtonClick();
            Thread.Sleep(80);
            InputSim.Mouse.RightButtonClick();
            Thread.Sleep(80);
            InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_M);
            
            // Read copied data
            Thread.Sleep(100);
            var labelsMultiline = SystemClipboard.Instance.Read();
        
            var labelsList = new List<string>();
        
            var startLinePos = 0;
        
            var currentLinePos = 0;
        
            var rawSplitLines = labelsMultiline.Split("\r\n");
            
            foreach (var line in rawSplitLines)
            {
                Console.WriteLine($"Raw: {line}");
            }
            
            Console.WriteLine();
            Console.WriteLine();
        
            for (var i = startLinePos; i < rawSplitLines.Length; i++)
            {
                if (!rawSplitLines[i].Contains(" debug")) continue;
                
                var lineFirstHalf = rawSplitLines[i].Split(" debug")[0];
            
                labelsList.Add(lineFirstHalf);
            }
            
            foreach (var line in labelsList)
            {
                Console.WriteLine($"Trimmed: {line}");
            }
            
            Console.WriteLine();
            Console.WriteLine();
        
            // If we didn't get any lines something broke. Stop trying to do stuff.
            if (labelsList.Count < 1) Environment.Exit(0);
            
            Thread.Sleep(100);
        
            // Focus the main browser window
            MoveMouseScaled(new Point(4009, 291));
            InputSim.Mouse.LeftButtonClick();
            
            foreach (var trimmedLine in labelsList)
            {
                Thread.Sleep(200);
                
                // Find first link
                InputSim.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
                InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_F);
                InputSim.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
            
                Thread.Sleep(100);
            
                InputSim.Keyboard.TextEntry(trimmedLine);
            
                Thread.Sleep(100);
                
                
                Thread.Sleep(100);
                
                // Click where it should be focused now
                MoveMouseScaled(new Point(4503, 345));
                Thread.Sleep(100);
                InputSim.Mouse.LeftButtonClick();
                
                // Change this one's timing so it doesn't pick up the last one clicked in the new clear console
                Thread.Sleep(250); // Was 300

                if (trimmedLine == labelsList.Last())
                {
                    // Clear the console in preparation for the next set
                    MoveMouseScaled(new Point(4074, 506));
                    InputSim.Mouse.LeftButtonClick();    
                }
                
                // Focus back in the browser
                MoveMouseScaled(new Point(4158, 263));
                InputSim.Mouse.LeftButtonClick();
            }
            
            // Go into loop to wait for audio to stop playing
            var noAudioCountdown = 2500;

            var audioHelper = new AudioHelper();

            var audioIsPlaying = false;

            do
            {
                audioIsPlaying = audioHelper.IsAudioPlayingOnDefaultAudioDevice();

                if (audioIsPlaying)
                {
                    // Reset it when audio heard
                    noAudioCountdown = 2500;

                    //Console.WriteLine("Audio heard, resetting!");
                }
                else
                {
                    noAudioCountdown--;

                    //Console.WriteLine(noAudioCountdown);    
                }
            }
            while (noAudioCountdown > 0);

            // Timed out due to enough time with audio not playing
            Console.WriteLine("No audio heard for a bit: OK to loop!");
            Console.WriteLine();
        }
    }


    private static void MoveMouseScaled(Point mouseMoveTo)
    {
        var dpiScalingX = 17;
        var dpiScalingY = 30.35;
        
        InputSim.Mouse.MoveMouseTo(
            mouseMoveTo.X * dpiScalingX, 
            mouseMoveTo.Y * dpiScalingY);
    }   
    
    private static void PlayArrowsOnWASD(string unicodeArrows)
    {
        foreach (var arrowChar in unicodeArrows)
        {
            switch (arrowChar)
            {
                // Up
                case '\u21e7':
                    InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_W);
                    break;
                
                // Down
                case '\u21e9':
                    InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_S);
                    break;
                
                // Left
                case '\u21e6':
                    InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_A);
                    break;
                
                // Right
                case '\u21e8':
                    InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_D);
                    break;
            }
        }
    }
    
    private static void CopyArrowsAndPlayKeystrokes(Point startDrag, Point stopDrag)
    {
        // Select from unicode arrows line beginning
        MoveMouseScaled(startDrag);
        InputSim.Mouse.LeftButtonDown();

        // To unicode arrows line end
        MoveMouseScaled(stopDrag);
        InputSim.Mouse.LeftButtonUp();

        // Copy to clipboard
        InputSim.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
        InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_C);
        InputSim.Keyboard.KeyUp(VirtualKeyCode.CONTROL);

        // Clipboard unicode arrows get
        Thread.Sleep(80);
        var unicodeArrows = SystemClipboard.Instance.Read();

        PlayArrowsOnWASD(unicodeArrows);
        InputSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
    }
}