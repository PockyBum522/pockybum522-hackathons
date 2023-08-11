#include <Arduino.h>
#include "libraries/Hardware/ModemManager.h"
#include "libraries/Hardware/SdCardManager.h"
#include <elapsedMillis.h>

#define LED_PIN             12

elapsedMillis modemInitializationDelay;

void setup()
{
    Serial.begin(115200);

    // Set LED OFF
    pinMode(LED_PIN, OUTPUT);
    digitalWrite(LED_PIN, HIGH);

    ModemManager::modemPowerOn();

    SdCardManager::sdCardSetup();

    ModemManager::beginModemConnection();

    Serial.println("Initialization done! Waiting ten seconds for modem init before continuing...");
}

void loop()
{
    // Don't start doing anything in loop() until 10 seconds in, giving the modem time to come up
    if (modemInitializationDelay < 10000) return;

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
    if (modem.waitResponse(1000L, res) == 1) {
        res.replace(GSM_NL "OK" GSM_NL, "");
        Serial.println(res);
    }
    res = "";
    Serial.println("=======================");


    Serial.println("=====Preferred selection between CAT-M and NB-IoT=====");
    modem.sendAT("+CMNB?");
    if (modem.waitResponse(1000L, res) == 1) {
        res.replace(GSM_NL "OK" GSM_NL, "");
        Serial.println(res);
    }
    res = "";
    Serial.println("=======================");


    String name = modem.getModemName();
    Serial.println("Modem Name: " + name);

    String modemInfo = modem.getModemInfo();
    Serial.println("Modem Info: " + modemInfo);

    for (int i = 0; i <= 4; i++) {
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
        while (tryCount--) {
            int16_t signal =  modem.getSignalQuality();
            Serial.print("Signal: ");
            Serial.print(signal);
            Serial.print(" ");
            Serial.print("isNetworkConnected: ");
            isConnected = modem.isNetworkConnected();
            Serial.println( isConnected ? "CONNECT" : "NO CONNECT");
            if (isConnected) {
                break;
            }
            delay(1000);
            digitalWrite(LED_PIN, !digitalRead(LED_PIN));
        }
        if (isConnected) {
            break;
        }
    }
    digitalWrite(LED_PIN, HIGH);

    Serial.println();
    Serial.println("Device is connected .");
    Serial.println();

    Serial.println("=====Inquiring UE system information=====");
    modem.sendAT("+CPSI?");
    if (modem.waitResponse(1000L, res) == 1) {
        res.replace(GSM_NL "OK" GSM_NL, "");
        Serial.println(res);
    }

    Serial.println("/**********************************************************/");
    Serial.println("After the network test is complete, please enter the  ");
    Serial.println("AT command in the serial terminal.");
    Serial.println("/**********************************************************/\n\n");

    while (1) {
        while (SerialAT.available()) {
            Serial.write(SerialAT.read());
        }
        while (Serial.available()) {
            SerialAT.write(Serial.read());
        }
    }
}
