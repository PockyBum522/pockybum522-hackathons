#ifndef INC_STACUITY_MQTT_GPS_TRACKING_SDCARDMANAGER_H
#define INC_STACUITY_MQTT_GPS_TRACKING_SDCARDMANAGER_H


#define SD_MISO     2
#define SD_MOSI     15
#define SD_SCLK     14
#define SD_CS       13

class SdCardManager
{
public:
    static void sdCardSetup()
    {
        SPI.begin(SD_SCLK, SD_MISO, SD_MOSI, SD_CS);
        if (!SD.begin(SD_CS)) {
            Serial.println("SDCard MOUNT FAIL");
        } else {
            uint32_t cardSize = SD.cardSize() / (1024 * 1024);
            String str = "SDCard Size: " + String(cardSize) + "MB";
            Serial.println(str);
        }
    }
};


#endif //INC_STACUITY_MQTT_GPS_TRACKING_SDCARDMANAGER_H
