#include <Arduino.h>
#include "libraries/Hardware/ModemManager.h"
#include "libraries/Hardware/SdCardManager.h"
#include <elapsedMillis.h>

const int LED_PIN = 12;

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

    ModemManager::connectToCellNetwork(LED_PIN);

    Serial.println("/**********************************************************/");
    Serial.println("After the network test is complete, please enter the  ");
    Serial.println("AT command in the serial terminal.");
    Serial.println("/**********************************************************/\n\n");

    while (true)
    {
        while (SerialAT.available())
        {
            Serial.write(SerialAT.read());
        }

        while (Serial.available())
        {
            SerialAT.write(Serial.read());
        }
    }
}
