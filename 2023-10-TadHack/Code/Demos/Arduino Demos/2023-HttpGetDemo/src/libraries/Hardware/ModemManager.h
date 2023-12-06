#ifndef INC_STACUITY_MQTT_GPS_TRACKING_MODEMMANAGER_H
#define INC_STACUITY_MQTT_GPS_TRACKING_MODEMMANAGER_H


#define TINY_GSM_MODEM_SIM7000
#define TINY_GSM_RX_BUFFER 1024 // Set RX buffer to 1Kb

#define SerialAT Serial1

// See all AT commands, if wanted
#define DUMP_AT_COMMANDS

// set GSM PIN, if any
#define GSM_PIN ""

// Your GPRS credentials, if any
const char apn[]  = "stacuity.flex";     //SET TO YOUR APN
const char gprsUser[] = "";
const char gprsPass[] = "";

#include <TinyGsmClient.h>
#include <SPI.h>
#include <SD.h>
#include <Ticker.h>

#ifdef DUMP_AT_COMMANDS  // if enabled it requires the streamDebugger lib
    #include <StreamDebugger.h>
    #include "HttpClient.h"

    StreamDebugger debugger(SerialAT, Serial);
    TinyGsm modem(debugger);
#else
    TinyGsm modem(SerialAT);
#endif

TinyGsmClient client(modem);

#define UART_BAUD   115200
#define PIN_DTR     25
#define PIN_TX      27
#define PIN_RX      26
#define PWR_PIN     4

int counter, lastIndex, numberOfPieces = 24;
String pieces[24], input;

class ModemManager
{
public:
    static void powerOnModem()
    {
        pinMode(PWR_PIN, OUTPUT);
        digitalWrite(PWR_PIN, HIGH);
        delay(300);
        digitalWrite(PWR_PIN, LOW);

        Serial.println("\nWait...");

        delay(1000);

        SerialAT.begin(UART_BAUD, SERIAL_8N1, PIN_RX, PIN_TX);

        // Restart takes quite some time
        // To skip it, call init() instead of restart()
        Serial.println("Initializing modem...");
        if (!modem.restart()) {
            Serial.println("Failed to restart modem, attempting to continue without restarting");
        }
    }

    static void restartModemAndConnect()
    {
// Restart takes quite some time. To skip it, call init() instead of restart()
        Serial.println("Initializing modem...");
        if (!modem.init()) {
            Serial.println("Failed to restart modem, attempting to continue without restarting");
        }

        String name = modem.getModemName();
        delay(500);
        Serial.println("Modem Name: " + name);

        String modemInfo = modem.getModemInfo();
        delay(500);
        Serial.println("Modem Info: " + modemInfo);


        // Unlock your SIM card with a PIN if needed
        if ( GSM_PIN && modem.getSimStatus() != 3 ) {
            modem.simUnlock(GSM_PIN);
        }
        modem.sendAT("+CFUN=0 ");
        if (modem.waitResponse(10000L) != 1) {
            DBG(" +CFUN=0  false ");
        }
        delay(200);

        /*
          2 Automatic
          13 GSM only
          38 LTE only
          51 GSM and LTE only
        * * * */
        String res;
        // CHANGE NETWORK MODE, IF NEEDED
        res = modem.setNetworkMode(2);
        if (res != "1") {
            DBG("setNetworkMode  false ");
            return ;
        }
        delay(200);

        /*
          1 CAT-M
          2 NB-Iot
          3 CAT-M and NB-IoT
        * * */
        // CHANGE PREFERRED MODE, IF NEEDED
        res = modem.setPreferredMode(1);
        if (res != "1") {
            DBG("setPreferredMode  false ");
            return ;
        }
        delay(200);

        /*AT+CBANDCFG=<mode>,<band>[,<band>…]
         * <mode> "CAT-M"   "NB-IOT"
         * <band>  The value of <band> must is in the band list of getting from  AT+CBANDCFG=?
         * For example, my SIM card carrier "NB-iot" supports B8.  I will configure +CBANDCFG= "Nb-iot ",8
         */
        /* modem.sendAT("+CBANDCFG=\"NB-IOT\",8 ");*/

        /* if (modem.waitResponse(10000L) != 1) {
             DBG(" +CBANDCFG=\"NB-IOT\" ");
         }*/
        delay(200);

        modem.sendAT("+CFUN=1 ");
        if (modem.waitResponse(10000L) != 1) {
            DBG(" +CFUN=1  false ");
        }
        delay(200);

        SerialAT.println("AT+CGDCONT?");
        delay(500);
        if (SerialAT.available()) {
            input = SerialAT.readString();
            for (int i = 0; i < input.length(); i++) {
                if (input.substring(i, i + 1) == "\n") {
                    pieces[counter] = input.substring(lastIndex, i);
                    lastIndex = i + 1;
                    counter++;
                }
                if (i == input.length() - 1) {
                    pieces[counter] = input.substring(lastIndex, i);
                }
            }
            // Reset for reuse
            input = "";
            counter = 0;
            lastIndex = 0;

            for ( int y = 0; y < numberOfPieces; y++) {
                for ( int x = 0; x < pieces[y].length(); x++) {
                    char c = pieces[y][x];  //gets one byte from buffer
                    if (c == ',') {
                        if (input.indexOf(": ") >= 0) {
                            String data = input.substring((input.indexOf(": ") + 1));
                            if ( data.toInt() > 0 && data.toInt() < 25) {
                                modem.sendAT("+CGDCONT=" + String(data.toInt()) + ",\"IP\",\"" + String(apn) + "\",\"0.0.0.0\",0,0,0,0");
                            }
                            input = "";
                            break;
                        }
                        // Reset for reuse
                        input = "";
                    } else {
                        input += c;
                    }
                }
            }
        } else {
            Serial.println("Failed to get PDP!");
        }

        Serial.println("\n\n\nWaiting for network...");
        if (!modem.waitForNetwork()) {
            delay(10000);
            return;
        }

        if (modem.isNetworkConnected()) {
            Serial.println("Network connected");
        }

        // --------TESTING GPRS--------
        Serial.println("\n---Starting GPRS TEST---\n");
        Serial.println("Connecting to: " + String(apn));
        if (!modem.gprsConnect(apn, gprsUser, gprsPass)) {
            delay(10000);
            return;
        }
    }

    static void printGprsConnectionInfoToSerial()
    {
        Serial.print("GPRS status: ");

        if (modem.isGprsConnected())
        {
            Serial.println("connected");
        }
        else
        {
            Serial.println("not connected");
        }

        String ccid = modem.getSimCCID();
        Serial.println("CCID: " + ccid);

        String imei = modem.getIMEI();
        Serial.println("IMEI: " + imei);

        String cop = modem.getOperator();
        Serial.println("Operator: " + cop);

        IPAddress local = modem.localIP();
        Serial.println("Local IP: " + String(local));

        int csq = modem.getSignalQuality();
        Serial.println("Signal quality: " + String(csq));

        SerialAT.println("AT+CPSI?");     //Get connection type and band

        delay(500);

        if (SerialAT.available())
        {
            String r = SerialAT.readString();
            Serial.println(r);
        }

    }

    static void disconnectGprs()
    {
        Serial.println("\n---End of GPRS TEST---\n");

        modem.gprsDisconnect();

        if (!modem.isGprsConnected())
        {
            Serial.println("GPRS disconnected");
        }
        else
        {
            Serial.println("GPRS disconnect: Failed.");
        }
    }

    static void powerOffModem()
    {

//
//    // --------TESTING POWER DONW--------
//
//    // Try to power-off (modem may decide to restart automatically)
//    // To turn off modem completely, please use Reset/Enable pins
//    modem.sendAT("+CPOWD=1");
//    if (modem.waitResponse(10000L) != 1) {
//        DBG("+CPOWD=1");
//    }
//    // The following command does the same as the previous lines
//    modem.poweroff();
//    Serial.println("Poweroff.");
//
    }
};


#endif //INC_STACUITY_MQTT_GPS_TRACKING_MODEMMANAGER_H
