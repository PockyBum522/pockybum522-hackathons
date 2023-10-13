#ifndef INC_2023_STACUITYTESTS_MODEMMANAGER_H
#define INC_2023_STACUITYTESTS_MODEMMANAGER_H


#include <Arduino.h>

#define TINY_GSM_MODEM_SIM7000
#define TINY_GSM_RX_BUFFER 1024 // Set RX buffer to 1Kb

#define SerialAT Serial1

#include "TinyGsmClient.h"

#define MODEM_UART_BAUD           115200
#define MODEM_PIN_TX              27
#define MODEM_PIN_RX              26
#define MODEM_PWR_PIN             4

// See all AT commands, if wanted
// #define DUMP_AT_COMMANDS

// set GSM PIN, if any
#define GSM_PIN ""

// Your GPRS credentials, if any
const char apn[]  = "";     //SET TO YOUR APN
const char gprsUser[] = "";
const char gprsPass[] = "";

#ifdef DUMP_AT_COMMANDS
    #include <StreamDebugger.h>
    StreamDebugger debugger(SerialAT, SerialMon);
    TinyGsm modem(debugger);
#else
    TinyGsm modem(SerialAT);
#endif

class ModemManager
{
public:
    static void modemPowerOn()
    {
        pinMode(MODEM_PWR_PIN, OUTPUT);

        digitalWrite(MODEM_PWR_PIN, LOW);
        delay(1000);
        digitalWrite(MODEM_PWR_PIN, HIGH);
    }

    static void modemPowerOff()
    {
        pinMode(MODEM_PWR_PIN, OUTPUT);

        digitalWrite(MODEM_PWR_PIN, LOW);
        delay(1500);
        digitalWrite(MODEM_PWR_PIN, HIGH);
    }

    static void modemRestart()
    {
        modemPowerOff();
        delay(1000);
        modemPowerOn();
    }

    static void beginModemConnection() {
        SerialAT.begin(MODEM_UART_BAUD, SERIAL_8N1, MODEM_PIN_RX, MODEM_PIN_TX);

        Serial.println("/**********************************************************/");
        Serial.println("To initialize the network test, please make sure your LTE ");
        Serial.println("antenna has been connected to the SIM interface on the board.");
        Serial.println("/**********************************************************/\n\n");
    }

    static void connectToCellNetwork(const int ledPin)
    {
        String res;

        Serial.println("========INIT========");

        if (!modem.init())
        {
            ModemManager::modemRestart();

            for (int i = 0; i < 4; i++)
            {
                delay(500);
                yield();
            }

            Serial.println("Failed to restart modem, attempting to continue without restarting");
            return;
        }

        Serial.println("========SIMCOMATI======");
        modem.sendAT("+SIMCOMATI");
        modem.waitResponse(1000L, res);

        res.replace(GSM_NL "OK" GSM_NL, "");
        Serial.println(res);

        res = "";
        Serial.println("=======================");
        Serial.println("=====Preferred mode selection=====");

        modem.sendAT("+CNMP?");

        if (modem.waitResponse(1000L, res) == 1)
        {
            res.replace(GSM_NL "OK" GSM_NL, "");
            Serial.println(res);
        }

        res = "";
        Serial.println("=======================");
        Serial.println("=====Preferred selection between CAT-M and NB-IoT=====");

        modem.sendAT("+CMNB?");

        if (modem.waitResponse(1000L, res) == 1)
        {
            res.replace(GSM_NL "OK" GSM_NL, "");
            Serial.println(res);
        }
        res = "";
        Serial.println("=======================");

        String name = modem.getModemName();
        Serial.println("Modem Name: " + name);

        String modemInfo = modem.getModemInfo();
        Serial.println("Modem Info: " + modemInfo);

        for (int i = 0; i <= 4; i++)
        {
            uint8_t network[] = {
                    2,  /*Automatic*/
                    13, /*GSM only*/
                    38, /*LTE only*/
                    51  /*GSM and LTE only*/
            };

            Serial.printf("Try %d method\n", network[i]);
            modem.setNetworkMode(network[i]);

            delay(3000);

            bool isConnected = false;
            int tryCount = 60;

            while (tryCount--)
            {
                int16_t signal =  modem.getSignalQuality();

                Serial.print("Signal: ");
                Serial.print(signal);
                Serial.print(" ");
                Serial.print("isNetworkConnected: ");

                isConnected = modem.isNetworkConnected();

                Serial.println( isConnected ? "CONNECT" : "NO CONNECT");

                if (isConnected)
                {
                    break;
                }

                delay(1000);
                digitalWrite(ledPin, !digitalRead(ledPin));
            }

            if (isConnected)
            {
                break;
            }
        }

        digitalWrite(ledPin, HIGH);

        Serial.println();
        Serial.println("Device is connected .");
        Serial.println();

        Serial.println("=====Inquiring UE system information=====");

        modem.sendAT("+CPSI?");

        if (modem.waitResponse(1000L, res) == 1)
        {
            res.replace(GSM_NL "OK" GSM_NL, "");
            Serial.println(res);
        }
    }
};


#endif //INC_2023_STACUITYTESTS_MODEMMANAGER_H
