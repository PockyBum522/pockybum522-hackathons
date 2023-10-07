using System.Drawing;
using WindowsInput;
using WindowsInput.Native;
using Clipboard;

namespace SunCtfDdr;

public static class Program
{
    private static readonly InputSimulator InputSim = new InputSimulator();

    public static void Main()
    {
        // --------------------========== CONFIGURATION START ==========--------------------
        
        // You might need to change the DPI scaling X and Y in MoveMouseScaled if mouse movements aren't landing where expected
        var pointInsideTerminalWindow = new Point(849, 1767);

        // Increase/decrease this if the first line of unicode arrows takes less or more time to show up in the terminal
        const int firstDelay = 120;

        // Increase/decrease this if each successive line of unicode arrows takes less or more time to show up in the terminal
        const int repeatedDelay = 850;
        
        // Point in the terminal that the mouse will start dragging at, should be just to the left of the leftmost unicode arrow
        var startDragAt = new Point(117, 2037);
        
        // Point in the terminal that should be just to the *right* of the *rightmost* unicode arrow
        var stopDragAt = new Point(825, 2037);
        
        // --------------------========== CONFIGURATION STOP ==========--------------------

        // Give focus to terminal window
        MoveMouseScaled(pointInsideTerminalWindow);
        InputSim.Mouse.LeftButtonClick();
        Thread.Sleep(500);

        // Run last command again
        InputSim.Keyboard.KeyPress(VirtualKeyCode.UP);
        InputSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        Thread.Sleep(1000);
        
        // Press ENTER to resume the terminal window
        InputSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        
        // Press ENTER to start the game
        InputSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        
        
        // Wait for data to come in
        Thread.Sleep(firstDelay);
        CopyArrowsAndPlayKeystrokes(startDragAt, stopDragAt);

        for (var i = 0; i < 254; i++)
        {
            // Play it again, Sam
            Thread.Sleep(repeatedDelay);
            
            CopyArrowsAndPlayKeystrokes(startDragAt, stopDragAt);
            
            Console.WriteLine($"On loop: {i}");
        }

        // Exit game and get back to prompt
        Thread.Sleep(2000);
        InputSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
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