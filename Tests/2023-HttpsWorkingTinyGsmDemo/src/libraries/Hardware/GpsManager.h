#ifndef STACUITY_MQTT_GPS_TRACKING_GPSMANAGER_H
#define STACUITY_MQTT_GPS_TRACKING_GPSMANAGER_H


#include "ModemManager.h"

class GpsManager
{
public:
    static void initializeGps()
    {

        // --------TESTING GPS--------

        Serial.println("\n---Starting GPS TEST---\n");
        // Set SIM7000G GPIO4 HIGH ,turn on GPS power
        // CMD:AT+SGPIO=0,4,1,1
        // Only in version 20200415 is there a function to control GPS power
        modem.sendAT("+SGPIO=0,4,1,1");
        if (modem.waitResponse(10000L) != 1)
        {
            DBG(" SGPIO=0,4,1,1 false ");
        }
        modem.enableGPS();
    }

    static void printGpsDataForever(){
        //    float lat,  lon;
//    while (1) {
//        if (modem.getGPS(&lat, &lon)) {
//            Serial.printf("lat:%f lon:%f\n", lat, lon);
//            break;
//        } else {
//            Serial.print("getGPS ");
//            Serial.println(millis());
//        }
//        delay(2000);
//    }
    }

    static void powerOffGps()
    {

//    modem.disableGPS();

        // Set SIM7000G GPIO4 LOW ,turn off GPS power
        // CMD:AT+SGPIO=0,4,1,0
        // Only in version 20200415 is there a function to control GPS power
        modem.sendAT("+SGPIO=0,4,1,0");
        if (modem.waitResponse(10000L) != 1) {
            DBG(" SGPIO=0,4,1,0 false ");
        }
    }
};


#endif //STACUITY_MQTT_GPS_TRACKING_GPSMANAGER_H
