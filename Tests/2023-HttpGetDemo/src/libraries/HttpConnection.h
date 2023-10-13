#ifndef STACUITY_MQTT_GPS_TRACKING_HTTPCONNECTION_H
#define STACUITY_MQTT_GPS_TRACKING_HTTPCONNECTION_H


// Server details
const char server[]   = "vsh.pp.ua";
const char resource[] = "/TinyGSM/logo.txt";
const int  port       = 80;

HttpClient    http(client, server, port);

class HttpConnection
{
public:
    static void makeGetRequest()
    {

        Serial.print(F("Performing HTTP GET request... "));

        int err = http.get(resource);

        if (err != 0)
        {
            Serial.println(F("failed to connect"));
            delay(10000);
            return;
        }

        int status = http.responseStatusCode();
        Serial.print(F("Response status code: "));
        Serial.println(status);
        if (!status) {
            delay(10000);
            return;
        }

        Serial.println(F("Response Headers:"));
        while (http.headerAvailable()) {
            String headerName  = http.readHeaderName();
            String headerValue = http.readHeaderValue();
            Serial.println("    " + headerName + " : " + headerValue);
        }

        int length = http.contentLength();
        if (length >= 0) {
            Serial.print(F("Content length is: "));
            Serial.println(length);
        }
        if (http.isResponseChunked()) {
            Serial.println(F("The response is chunked"));
        }

        String body = http.responseBody();
        Serial.println(F("Response:"));
        Serial.println(body);

        Serial.print(F("Body length is: "));
        Serial.println(body.length());

        // Shutdown

        http.stop();
        Serial.println(F("Server disconnected"));
    }
};


#endif //STACUITY_MQTT_GPS_TRACKING_HTTPCONNECTION_H
