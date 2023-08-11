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
};


#endif //INC_2023_STACUITYTESTS_MODEMMANAGER_H
