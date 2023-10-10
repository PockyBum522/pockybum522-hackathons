// using System.Diagnostics;
// using System.Diagnostics.Tracing;
// using WindowsInput;
// using WindowsInput.Native;
// using Clipboard;
// using PuppeteerSharp;
// using Point = System.Drawing.Point;
//
// namespace SunCtfSimonProgrammer2;
//
// public static class Program
// {
//     private static readonly InputSimulator InputSim = new InputSimulator();
//
//     public static async Task Main()
//     {
//         // --------------------========== CONFIGURATION START ==========--------------------
//
//
//         // --------------------========== CONFIGURATION STOP ==========--------------------
//
//         foreach (var process in Process.GetProcessesByName("chrome"))
//         {
//             process.Kill();
//         }
//         
//         using var browserFetcher = new BrowserFetcher();
//         await browserFetcher.DownloadAsync();
//         var browser = await Puppeteer.LaunchAsync(new LaunchOptions
//         {
//             Headless = true
//         });
//
//         var page = await browser.NewPageAsync();
//
//         await page.GoToAsync("https://simon2.web.2023.sunshinectf.games/");
//
//         await page.SetViewportAsync(new ViewPortOptions
//         {
//             Width = 1024,
//             Height = 50000
//         });
//
//         await Task.Delay(9000);
//
//         //await page.ScreenshotAsync(@"D:\Dropbox\Documents\Desktop\testShot.png");
//
//         var playButton = await GetPlayButtonElement(page);
//
//         var allHrefElements = await GetAllHrefElements(page);
//
//         Console.WriteLine();
//         Console.WriteLine();
//         Console.WriteLine("NEXT DUMP:");
//         Console.WriteLine();
//         Console.WriteLine();
//         
//         var totalElementsCount = allHrefElements.Length;
//
//         var threadsCount = 10;
//         
//         var dividedTotalElementsCount = totalElementsCount / threadsCount;
//         
//         Console.WriteLine($"Divided work, each function will do: {dividedTotalElementsCount}");
//
//         var threadsTasks = new List<Task<IJSHandle>>();
//
//         var elementsChecker = new HtmlElementsChecker();
//         
//         var getOuterHtml = new List<string>();
//         
//         playButton?.ClickAsync();
//
//         var countdown = 200;
//
//         while (countdown-- > 0)
//         {
//             for (var i = 0; i < totalElementsCount; i++)
//             {
//                 threadsTasks.Add(allHrefElements[i].GetPropertyAsync("outerHTML"));
//             }
//             
//             await Task.WhenAll(threadsTasks);
//             
//             Console.WriteLine(countdown);
//         }
//         
//         var elementsInfo = await Task.WhenAll(threadsTasks);
//             
//         foreach (var elementInfo in elementsInfo)
//         {
//             var responseString = elementInfo.ToString();
//
//             if (!responseString!.Contains("color: yellow"))
//             {
//                 continue;
//             }
//                 
//             getOuterHtml.Add(responseString);
//             break;    
//         }
//
//         var counter = 0;
//         
//         foreach (var stringToCheck in getOuterHtml)
//         {
//             //Console.WriteLine($"Checking #{counter++} / {getOuterHtmlTasks.Count}");
//
//             Console.WriteLine(stringToCheck);
//             
//         }
//         
//         Console.WriteLine("Done.");
//         
//         await Task.Delay(9999);
//     }
//
//     
//
//     private static async Task<IElementHandle?> GetPlayButtonElement(IPage page)
//     {
//         var xpath = "//a";
//         IElementHandle[] allHrefElements = await ((IPage)page).XPathAsync(xpath);
//
//         IElementHandle? playButton = null;
//
//         foreach (var listItem in allHrefElements)
//         {
//             //Console.WriteLine((await listItem.GetPropertyAsync("textContent")).RemoteObject.Value.ToString());
//             var remoteObjectDescription = listItem.RemoteObject.Description;
//
//             // Console.WriteLine($"Desc: {remoteObjectDescription}");
//             // Console.WriteLine($"Class: {listItem.RemoteObject.ClassName}");
//             // Console.WriteLine();
//
//             if (remoteObjectDescription == "a#PLAY")
//             {
//                 playButton = listItem;
//                 break;
//             }
//         }
//
//         return playButton;
//     }    
//     
//     private static async Task<IElementHandle[]> GetAllHrefElements(IPage page)
//     {
//         var xpath = "//a";
//         
//         IElementHandle[] allHrefElements = await ((IPage)page).XPathAsync(xpath);
//
//         return allHrefElements;
//     }
//
//     private static Task<IElementHandle[]> QueryAndFilterAllHref(IPage page)
//     {
//         return page.QuerySelectorAllAsync("a");
//
//         
//     }
//
//     private static void MoveMouseScaled(Point mouseMoveTo)
//     {
//         var dpiScalingX = 17;
//         var dpiScalingY = 30.35;
//         
//         InputSim.Mouse.MoveMouseTo(
//             mouseMoveTo.X * dpiScalingX, 
//             mouseMoveTo.Y * dpiScalingY);
//     }   
//     
//     private static void PlayArrowsOnWASD(string unicodeArrows)
//     {
//         foreach (var arrowChar in unicodeArrows)
//         {
//             switch (arrowChar)
//             {
//                 // Up
//                 case '\u21e7':
//                     InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_W);
//                     break;
//                 
//                 // Down
//                 case '\u21e9':
//                     InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_S);
//                     break;
//                 
//                 // Left
//                 case '\u21e6':
//                     InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_A);
//                     break;
//                 
//                 // Right
//                 case '\u21e8':
//                     InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_D);
//                     break;
//             }
//         }
//     }
//     
//     private static void CopyArrowsAndPlayKeystrokes(Point startDrag, Point stopDrag)
//     {
//         // Select from unicode arrows line beginning
//         MoveMouseScaled(startDrag);
//         InputSim.Mouse.LeftButtonDown();
//
//         // To unicode arrows line end
//         MoveMouseScaled(stopDrag);
//         InputSim.Mouse.LeftButtonUp();
//
//         // Copy to clipboard
//         InputSim.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
//         InputSim.Keyboard.KeyPress(VirtualKeyCode.VK_C);
//         InputSim.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
//
//         // Clipboard unicode arrows get
//         Thread.Sleep(80);
//         var unicodeArrows = SystemClipboard.Instance.Read();
//
//         PlayArrowsOnWASD(unicodeArrows);
//         InputSim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
//     }
// }