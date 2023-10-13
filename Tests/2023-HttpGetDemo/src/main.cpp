#include <Arduino.h>
#include "libraries/Hardware/ModemManager.h"
#include "libraries/Hardware/SdCardManager.h"
#include "libraries/HttpConnection.h"
#include "libraries/Hardware/GpsManager.h"
#include <elapsedMillis.h>

elapsedMillis modemInitializationDelay;

#define LED_PIN     12

void setup()
{
    Serial.begin(115200);

    delay(10);

    // Set LED OFF
    pinMode(LED_PIN, OUTPUT);
    digitalWrite(LED_PIN, HIGH);

    SdCardManager::sdCardSetup();

    ModemManager::powerOnModem();
}

void loop()
{
    ModemManager::restartModemAndConnect();

    ModemManager::printGprsConnectionInfoToSerial();

    HttpConnection::makeGetRequest();

    ModemManager::disconnectGprs();

    // These are probably messed up and should be thoroughly checked for bugs and issues when enabled
    // GpsManager::initializeGps();
    // GpsManager::printGpsDataForever();
    // GpsManager::powerOffGps();

    ModemManager::powerOffModem();

    // Halt
    while (true)
    {
        delay(400);
        yield();
    }
}
