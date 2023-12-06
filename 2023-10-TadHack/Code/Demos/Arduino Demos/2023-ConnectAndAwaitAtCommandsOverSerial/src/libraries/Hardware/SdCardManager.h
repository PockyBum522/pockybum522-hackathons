#ifndef INC_2023_STACUITYTESTS_SDCARDMANAGER_H
#define INC_2023_STACUITYTESTS_SDCARDMANAGER_H


#include <Arduino.h>
#include <SPI.h>
#include <SD.h>

#define SD_MISO             2
#define SD_MOSI             15
#define SD_SCLK             14
#define SD_CS               13

class SdCardManager
{
public:
    static void sdCardSetup()
    {
        Serial.println("========SDCard Detect.======");

        SPI.begin(SD_SCLK, SD_MISO, SD_MOSI);

        if (!SD.begin(SD_CS))
        {
            Serial.println("SDCard MOUNT FAIL");
        }
        else
        {
            uint32_t cardSize = SD.cardSize() / (1024 * 1024);
            String str = "SDCard Size: " + String(cardSize) + "MB";
            Serial.println(str);
        }

        Serial.println("===========================");
    }
};


#endif //INC_2023_STACUITYTESTS_SDCARDMANAGER_H
