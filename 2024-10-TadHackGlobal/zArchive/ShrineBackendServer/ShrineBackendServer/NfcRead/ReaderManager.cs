using System.Security.Authentication;
using ManagedLibnfc;

// ReSharper disable CognitiveComplexity because it's not my code and I don't care

namespace ShrineBackendServer.NfcRead;

internal class ReaderManager
{
    private readonly byte[] _abtReqa = [0x26];
    private readonly byte[] _abtSelectAll = [0x93, 0x20];
    private readonly byte[] _abtSelectTag = [0x93, 0x70, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00];
    private readonly byte[] _abtRats = [0xe0, 0x50, 0x00, 0x00];
    private readonly byte[] _abtHalt = [0x50, 0x00, 0x00, 0x00];
    
    private NfcDevice? _nfcDevice;
    private const int MaxFrameLen = 264;
    private const byte SakFlagAtsSupported = 0x20;

    private readonly byte[] _abtRx = new byte[MaxFrameLen];

    internal string GetUid()
    {
        var abtRawUid = new byte[12];
        var abtAtqa = new byte[2];
        var abtAts = new byte[MaxFrameLen];
        var forceRats = false;

        using var context = new NfcContext();
        using var nfcDevice = _nfcDevice = context.OpenDevice();
        
        if (_nfcDevice is null) throw new NullReferenceException();
        
        // Initialise NFC device as "initiator"
        _nfcDevice.InitiatorInit();
     
        // Configure the CRC
        _nfcDevice.DeviceSetPropertyBool(NfcProperty.HandleCrc, false);
        
        // Use raw send/receive methods
        _nfcDevice.DeviceSetPropertyBool(NfcProperty.EasyFraming, false);
        
        // Disable 14443-4 auto-switching
        _nfcDevice.DeviceSetPropertyBool(NfcProperty.AutoIso14443_4, false);
        
        // Console.WriteLine("NFC reader: {0} opened", device.Type);
        // Console.WriteLine();
        
        // Send the 7 bits request command specified in ISO 14443A (0x26)
        TransmitBits(_abtReqa, 7);
        Array.Copy(_abtRx, abtAtqa, 2);
        
        // Anti-collision
        TransmitBytes(_abtSelectAll, 2);
        
        // Check answer
        // if ((abtRx[0] ^ abtRx[1] ^ abtRx[2] ^ abtRx[3] ^ abtRx[4]) != 0)
        //     Console.WriteLine("WARNING: BCC check failed!");
        
        // Save the UID CL1
        Array.Copy(_abtRx, abtRawUid, 4);
        
        //Prepare and send CL1 Select-Command
        Array.Copy(_abtRx, 0, _abtSelectTag, 2, 5);
        ManagedLibnfc.PInvoke.Libnfc.Iso14443aCrcAppend(_abtSelectTag, 7);
        TransmitBytes(_abtSelectTag, 9);
        
        // We have to do the anti-collision for cascade level 2
    
        // Prepare CL2 commands
        _abtSelectAll[0] = 0x95;
    
        // Anti-collision
        TransmitBytes(_abtSelectAll, 2);
    
        // Save UID CL2
        Array.Copy(_abtRx, 0, abtRawUid, 4, 4);
    
        // Selection
        _abtSelectTag[0] = 0x95;
        Array.Copy(_abtRx, 0, _abtSelectTag, 2, 5);
        ManagedLibnfc.PInvoke.Libnfc.Iso14443aCrcAppend(_abtSelectTag, 7);
        
        TransmitBytes(_abtSelectTag, 9);
        
        // Request ATS, this only applies to tags that support ISO 14443A-4
        if ((_abtRx[0] & SakFlagAtsSupported) != 0)
        {
        }

        if ((_abtRx[0] & SakFlagAtsSupported) != 0 || forceRats)
        {
            ManagedLibnfc.PInvoke.Libnfc.Iso14443aCrcAppend(_abtRats, 2);
            var szRx = TransmitBytes(_abtRats, 4);
            if (szRx >= 0)
            {
                Array.Copy(_abtRx, abtAts, szRx);
            }
        }
        
        // device.DeviceSetPropertyBool(NfcProperty.HandleParity, true);
        // Done, halt the tag now
        ManagedLibnfc.PInvoke.Libnfc.Iso14443aCrcAppend(_abtHalt, 2);
        TransmitBytes(_abtHalt, 4);

        var returnString = abtRawUid[1].ToString("X2").ToUpper();
        returnString += abtRawUid[2].ToString("X2").ToUpper();
        returnString += abtRawUid[3].ToString("X2").ToUpper();
        returnString += abtRawUid[4].ToString("X2").ToUpper();
        returnString += abtRawUid[5].ToString("X2").ToUpper();
        returnString += abtRawUid[6].ToString("X2").ToUpper();
        returnString += abtRawUid[7].ToString("X2").ToUpper();

        return returnString;
    }

    private readonly bool _quietOutput = true;
    private readonly bool _timed = false;

    private void TransmitBits(byte[] pbtTx, uint szTxBits)
    {
        if (_nfcDevice is null) throw new NullReferenceException();
        
        // Show transmitted command
        if (!_quietOutput)
        {
            Console.Write("Sent bits");
        }

        // Transmit the bit frame command, we don't use the arbitrary parity feature
        if (_timed)
        {
            uint cycles = 0;
            int szRxBits;
            
            if ((szRxBits = _nfcDevice.InitiatorTransceiveBitsTimed(pbtTx, szTxBits, null, _abtRx, MaxFrameLen, null, ref cycles)) < 0)
                throw new AuthenticationException("Error: No tag available");
            if ((!_quietOutput) && (szRxBits > 0))
            {
                Console.WriteLine("Response after {0} cycles", cycles);
            }
        }
        else
        {
            if ((_ = _nfcDevice.InitiatorTransceiveBits(pbtTx, szTxBits, null, _abtRx, MaxFrameLen, null)) < 0)
                throw new AuthenticationException("Error: No tag available");
        }
        // Show received answer
        if (!_quietOutput)
        {
            Console.Write("Received bits");
        }
    }

    private int TransmitBytes(byte[] pbtTx, uint szTx)
    {
        if (_nfcDevice is null) throw new NullReferenceException();
        
        // Show transmitted command
        if (!_quietOutput)
        {
            Console.Write("Sent bits");
        }
        int szRx;
        // Transmit the command bytes
        if (_timed)
        {
            uint cycles = 0;
            if ((szRx = _nfcDevice.InitiatorTransceiveBytesTimed(pbtTx, szTx, _abtRx, MaxFrameLen, ref cycles)) < 0)
                return szRx;
            if ((!_quietOutput) && (szRx > 0))
            {
                Console.WriteLine("Response after {0} cycles", cycles);
            }
        }
        else
        {
            if ((szRx = _nfcDevice.InitiatorTransceiveBytes(pbtTx, szTx, _abtRx, MaxFrameLen, 0)) < 0)
                return szRx;
        }
        // Show received answer
        if (!_quietOutput)
        {
            Console.Write("Received bits");
        }
        // Succesful transfer
        return szRx;
    }
}