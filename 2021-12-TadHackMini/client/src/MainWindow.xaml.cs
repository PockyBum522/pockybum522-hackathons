using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Threading;
using ConsoleApp1;

namespace TelePaperProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StrokeCollection CanvasOneStrokes { get; set; } = new();
        private Dispatcher _uiThreadDispatcher;
        
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            
            _uiThreadDispatcher = Dispatcher.CurrentDispatcher;
            
            Task.Run(StartStrokeUpdaterLoop);
        }

        private async Task StartStrokeUpdaterLoop()
        {
            TelePaperTcpClient.Connect("pockybum522.com");
            
            while (true)
            {
                var memoryStream = await TelePaperTcpClient.GetCurrentStrokeCollection();
                var currentStrokes = new StrokeCollection(memoryStream);
                
                _uiThreadDispatcher.Invoke(CanvasOneStrokes.Clear);

                _uiThreadDispatcher.Invoke(() =>
                {
                    foreach (var stroke in currentStrokes)
                        CanvasOneStrokes.Add(stroke);
                });
            }
        }

        private void InkCanvas01_OnStrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            new Thread(async () => 
            {
                Thread.CurrentThread.IsBackground = true;
                TelePaperTcpClient.Connect("pockybum522.com");
                TelePaperTcpClient.SendLatestStroke(CanvasOneStrokes.Last());
            
                try
                {
                    var memoryStream = await TelePaperTcpClient.GetCurrentStrokeCollection();
                    var currentStrokes = new StrokeCollection(memoryStream);
            
                    _uiThreadDispatcher.Invoke(CanvasOneStrokes.Clear);
            
                    _uiThreadDispatcher.Invoke(() =>
                    {
                        foreach (var stroke in currentStrokes)
                            CanvasOneStrokes.Add(stroke);
                    });
                }
                catch (Exception)
                {
                    Console.WriteLine("Exception trying to add strokes, will retry");
                }
                
            }).Start();
        }

        private void InkCanvas01_OnStrokeErased(object sender, RoutedEventArgs e)
        {
            
        }
    }
}