using System.Drawing;
using WindowsInput;
using WindowsInput.Native;
using Clipboard;
using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace SunCtfSimonProgrammer1;

public static class Program
{
    private static readonly InputSimulator InputSim = new InputSimulator();
    
    // Audio related
    private static readonly MMDeviceEnumerator DevEnum = new MMDeviceEnumerator();
    private static MMDevice? DefaultAudioDevice = DevEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
    
    // From https://github.com/swharden/FftSharp
    private static int SAMPLE_RATE = 48000;
    private static WaveInEvent wvin;
    private static double MaxLevel = 0;
    
    // --------------------========== CONFIGURATION START ==========--------------------
        
    // 6000hz Hyperlink location on screen. All the rest of the hyperlink locations should be self explanatory
    private static Point _location6000hz = new Point(1877, 540);
    private static Point _location9000hz = new Point(1877, 575);
    private static Point _location4000hz = new Point(1877, 606);
    private static Point _location5000hz = new Point(1877, 639);
    private static Point _location9999hz = new Point(1877, 671);
    private static Point _location1000hz = new Point(1877, 705);
    private static Point _location8000hz = new Point(1877, 739);
    private static Point _location60hz = new Point(1877, 775);
    private static Point _location3000hz = new Point(1877, 806);
    private static Point _location7000hz = new Point(1877, 840);
    private static Point _location2000hz = new Point(1877, 874);
        
    private static Point _locationPlay = new Point(1971, 476);

    // List of what to compare, just the list of frequencies on the page
    private static List<int> _frequencies = new List<int>() { 60, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 9999 };
        
    // Give focus to website by clicking on taskbar icon. Browser should have the simon says tab up
    private static Point _locationBrowserInTaskbar = new Point(1044, 2129);
        
    // --------------------========== CONFIGURATION STOP ==========--------------------
    
    public static void Main()
    {
        //GetLocalAudioDevices();   // Use this if you need to change what audio input the program uses, will list devices, update the number in ConstantlyCheckCurrentPeakFrequency()
        
        // Set up to record from mic
        wvin?.Dispose();
        wvin = new NAudio.Wave.WaveInEvent();
        wvin.DeviceNumber = 4;
        wvin.WaveFormat = new NAudio.Wave.WaveFormat(rate: SAMPLE_RATE, bits: 16, channels: 1);
        wvin.DataAvailable += OnDataAvailable;
        wvin.BufferMilliseconds = 20;
        wvin.StartRecording();
        
         while (true)
         {
            var sequence = ConstantlyCheckCurrentPeakFrequency();

            var countSequenceInstances = CountSequenceInstances(sequence);
        
            foreach (var instance in countSequenceInstances)
            {
                Console.WriteLine($"Recorded: {instance.Frequency} \t with count: {instance.HitsCount}");
            }
            
            ClickHyperlinks(countSequenceInstances);
            
            Thread.Sleep(350);
         }
    }

    public class SequenceInstance
    {
        public int Frequency { get; set; }
        public int HitsCount { get; set; }
    }
    
    private static List<SequenceInstance> CountSequenceInstances(List<int> sequence)
    {
        var lastFrequency = -1;
        var outputList = new List<SequenceInstance>();
        var instancesCount = 1;
        
        foreach (var frequencySeen in sequence)
        {
            if (frequencySeen != lastFrequency)
            {
                // Found a change in frequency
                
                if (lastFrequency != -1)
                {
                    // Add it if it's not the initial -1
                    var instanceToAdd = new SequenceInstance();
                    instanceToAdd.HitsCount = instancesCount;
                    instanceToAdd.Frequency = lastFrequency;
                    
                    outputList.Add(instanceToAdd);
                }
                
                // Reset for the new instance
                instancesCount = 1;
                lastFrequency = frequencySeen;
            }
            else
            {
                // Next instance in list is a repeat of something we've already seen
                instancesCount++;
            }
        }
        
        // Since there's no change for the last instance, since it's...the last instance, just add it
        var lastInstanceToAdd = new SequenceInstance();
        lastInstanceToAdd.HitsCount = instancesCount;
        lastInstanceToAdd.Frequency = lastFrequency;
                    
        outputList.Add(lastInstanceToAdd);
        
        return outputList;
    }

    private static void ClickHyperlinks(List<SequenceInstance> sequenceInstancesWithCounts)
    {
        foreach (var frequencyHeard in sequenceInstancesWithCounts)
        {
            var hitsLeft = frequencyHeard.HitsCount;

            while (hitsLeft > 7)
            {
                Thread.Sleep(200);
                
                Console.WriteLine($"Sending click for: {frequencyHeard.Frequency} Hz \t Count: {frequencyHeard.HitsCount}");

                hitsLeft = hitsLeft - 15;
            
                switch (frequencyHeard.Frequency)
                {
                    case 60:
                        MoveMouseScaled(_location60hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;
                
                    case 1000:
                        MoveMouseScaled(_location1000hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;
                
                    case 2000:
                        MoveMouseScaled(_location2000hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;
                	
                    case 3000:
                        MoveMouseScaled(_location3000hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;
                
                    case 4000:
                        MoveMouseScaled(_location4000hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;

                    case 5000:
                        MoveMouseScaled(_location5000hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;
                
                    case 6000:
                        MoveMouseScaled(_location6000hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;

                    case 7000:
                        MoveMouseScaled(_location7000hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;

                    case 8000:
                        MoveMouseScaled(_location8000hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;

                    case 9000:
                        MoveMouseScaled(_location9000hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;
                
                    case 9999:
                        MoveMouseScaled(_location9999hz);
                        InputSim.Mouse.LeftButtonClick();
                        break;
                }
            }
        }
    }

    private static List<int> ConstantlyCheckCurrentPeakFrequency()
    {
        // ActivateWebsiteAndPlay(locationBrowserInTaskbar, locationPlay);

        var lastAudioSeenCounter = 200;
        
        var frequencySequence = new List<int>();

        Console.WriteLine("Listening now!");
        
        // Start listening and keeping notes about...which...notes...were played
        while (lastAudioSeenCounter > 0)
        {
            // What's the frequency, Kenneth?
            var toneFrequency = GetAudioPeakFrequency();

            Thread.Sleep(1);
            
            if (toneFrequency.PeakPower > 1)
            {
                Console.WriteLine($"Peak frequency: {toneFrequency.PeakFrequency} \t Power: {toneFrequency.PeakPower} \t Remaining Count: {lastAudioSeenCounter}");
            }
            else
            {
                Console.WriteLine($"Remaining Count: {lastAudioSeenCounter}");
                
                lastAudioSeenCounter--;
                continue;
            }
            
            Console.WriteLine($"Current peak frequency: {toneFrequency.PeakFrequency}");

            foreach (var frequency in _frequencies)
            {
                // Find within range
                if (!(toneFrequency.PeakFrequency > frequency - 500) ||
                    !(toneFrequency.PeakFrequency < frequency + 500)) continue;
                
                frequencySequence.Add(frequency);
            }
        }
        
        // If we haven't seen audio for a while
        foreach (var frequencySeen in frequencySequence)
        {
            Console.WriteLine($"Seen: {frequencySeen}");
        }

        return frequencySequence;
    }

    private static void GetLocalAudioDevices()
    {
        // Get audio device list 
        for (int i = 0; i < WaveIn.DeviceCount; i++)
        {
            Console.Write($"Device {i}: ");
            Console.WriteLine(WaveIn.GetCapabilities(i).ProductName);

            //Microphone Array (Realtek(R) Au
            //0
        }
    }

    private static void ActivateWebsiteAndPlay(Point locationBrowserInTaskbar, Point locationPlay)
    {
        // Give focus to website by clicking on taskbar icon. Browser should have the simon says tab up
        MoveMouseScaled(locationBrowserInTaskbar);
        InputSim.Mouse.LeftButtonClick();
        Thread.Sleep(1000);

        // Click play
        MoveMouseScaled(locationPlay);
        InputSim.Mouse.LeftButtonClick();
    }

    private static double[] lastBuffer;
    private static void OnDataAvailable(object sender, NAudio.Wave.WaveInEventArgs args)
    {
        int bytesPerSample = wvin.WaveFormat.BitsPerSample / 8;
        int samplesRecorded = args.BytesRecorded / bytesPerSample;
        if (lastBuffer is null || lastBuffer.Length != samplesRecorded)
            lastBuffer = new double[samplesRecorded];
        for (int i = 0; i < samplesRecorded; i++)
            lastBuffer[i] = BitConverter.ToInt16(args.Buffer, i * bytesPerSample);
    }

    private class PeakData
    {
        public PeakData()
        {
            PeakFrequency = 0;
            PeakPower = 0;
        }
        
        public PeakData(double peakFrequency, double peakPower)
        {
            PeakFrequency = peakFrequency;
            PeakPower = peakPower;
        }
        
        public double PeakFrequency { get; set; }
        public double PeakPower { get; set; }
    }
    
    private static PeakData GetAudioPeakFrequency()
    {
        if (lastBuffer is null) return new PeakData();

            var window = new FftSharp.Windows.Hanning();
            double[] windowed = window.Apply(lastBuffer);
            double[] zeroPadded = FftSharp.Pad.ZeroPad(windowed);
            System.Numerics.Complex[] spectrum = FftSharp.FFT.Forward(zeroPadded);
            
            double[] fftPower = FftSharp.FFT.Magnitude(spectrum);
                //FftSharp.FFT.Power(spectrum) :
                
            
            double[] fftFreq = FftSharp.FFT.FrequencyScale(fftPower.Length, SAMPLE_RATE);

            // determine peak frequency
            double peakFreq = 0;
            double peakPower = 0;
            for (int i = 0; i < fftPower.Length; i++)
            {
                if (fftPower[i] > peakPower)
                {
                    peakPower = fftPower[i];
                    peakFreq = fftFreq[i];
                }
            }
            
            return new PeakData(peakFreq, peakPower);
    }
    
    private static void MoveMouseScaled(Point mouseMoveTo)
    {
        var dpiScalingX = 17;
        var dpiScalingY = 30.35;
        
        InputSim.Mouse.MoveMouseTo(
            mouseMoveTo.X * dpiScalingX, 
            mouseMoveTo.Y * dpiScalingY);
    }

}