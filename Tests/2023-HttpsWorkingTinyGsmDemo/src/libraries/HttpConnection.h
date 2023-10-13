#ifndef STACUITY_MQTT_GPS_TRACKING_HTTPCONNECTION_H
#define STACUITY_MQTT_GPS_TRACKING_HTTPCONNECTION_H

// Insecure server details
//const char server[]   = "vsh.pp.ua";
//const char resource[] = "/TinyGSM/logo.txt";
//const int  port       = 80;
//
//HttpClient    http(client, server, port);


// Https server details
const char server[]   = "api.stacuity.com";
const char resource[] = "/api/v1/endpoints/b257522a-4d83-4f3e-8954-71d296bbf7c1/keyvalues";
const int  port       = 443;

#define LOGGING  // <- Logging is for the HTTP library

HttpClient    https(client, server, port);

class HttpConnection
{
public:
    static void makeSslGetRequest()
    {

        #define LOGGING  // <- Logging is for the HTTP library

        Serial.print(F("Performing HTTPS GET request... "));

        https.connectionKeepAlive();  // Currently, this is needed for HTTPS

        https.beginRequest();
        int err = https.get(resource);
        https.sendHeader("Content-Type:application/json");
        https.sendHeader("Authorization: Bearer c7c31bb975594ee0a389d4cdc727c18a");
        https.endRequest();

        Serial.print(F("err from https.get was: "));
        Serial.println(err);

        int status = https.responseStatusCode();
        Serial.print(F("Response status code: "));
        Serial.println(status);

        if (err != 0)
        {
            Serial.println(F("failed to connect"));
            delay(10000);
            return;
        }

        status = https.responseStatusCode();
        Serial.print(F("Response status code: "));
        Serial.println(status);

        if (!status)
        {
            delay(10000);
            return;
        }

        Serial.println(F("Response Headers:"));
        while (https.headerAvailable())
        {
            String headerName  = https.readHeaderName();
            String headerValue = https.readHeaderValue();
            Serial.println("    " + headerName + " : " + headerValue);
        }

        int length = https.contentLength();
        if (length >= 0) {
            Serial.print(F("Content length is: "));
            Serial.println(length);
        }
        if (https.isResponseChunked()) {
            Serial.println(F("The response is chunked"));
        }

        String body = https.responseBody();
        Serial.println(F("Response:"));
        Serial.println(body);

        Serial.print(F("Body length is: "));
        Serial.println(body.length());

        // Shutdown

        https.stop();
        Serial.println(F("Server disconnected"));
    }

//    static void makeGetRequest()
//    {
//        Serial.print(F("Performing HTTP GET request... "));
//
//        int err = https.get(resource);
//
//        if (err != 0)
//        {
//            Serial.println(F("failed to connect"));
//            delay(10000);
//            return;
//        }
//
//        int status = https.responseStatusCode();
//
//        Serial.print(F("Response status code: "));
//        Serial.println(status);
//
//        if (!status)
//        {
//            delay(10000);
//            return;
//        }
//
//        Serial.println(F("Response Headers:"));
//
//        while (https.headerAvailable())
//        {
//            String headerName  = https.readHeaderName();
//            String headerValue = https.readHeaderValue();
//            Serial.println("    " + headerName + " : " + headerValue);
//        }
//
//        int length = https.contentLength();
//        if (length >= 0) {
//            Serial.print(F("Content length is: "));
//            Serial.println(length);
//        }
//        if (https.isResponseChunked())
//        {
//            Serial.println(F("The response is chunked"));
//        }
//
//        String body = https.responseBody();
//
//        Serial.println(F("Response:"));
//        Serial.println(body);
//
//        Serial.print(F("Body length is: "));
//        Serial.println(body.length());
//
//        // Shutdown
//        https.stop();
//        Serial.println(F("Server disconnected"));
//    }
};


#endif //STACUITY_MQTT_GPS_TRACKING_HTTPCONNECTION_H
