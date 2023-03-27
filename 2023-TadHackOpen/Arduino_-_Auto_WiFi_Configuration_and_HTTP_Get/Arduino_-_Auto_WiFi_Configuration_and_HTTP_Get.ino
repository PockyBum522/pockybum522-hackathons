// Libraries:
//    https://github.com/Hieromon/AutoConnect

/*  
    0: WL_IDLE_STATUS
    1:  WL_NO_SSID_AVAIL
    2:  WL_SCAN_COMPLETED
    3:  WL_CONNECTED
    4:  WL_CONNECT_FAILED
    5:  WL_CONNECTION_LOST
    6:  WL_DISCONNECTED 
*/

#include <AutoConnect.h>
#include <HTTPClient.h>
#include <WebServer.h>
#include <WiFi.h>
#include <Wire.h>

#define LED_BUILTIN 2

// Comment this out if you don't want debug serial messages. Also might disable AutoConnect setup. Look in setup() to see if it does.
#define DEBUG_MODE_ON
// #define DEBUG_SHOW_LOOP

AutoConnect autoConnect;
AutoConnectConfig autoConnectConfig;
HTTPClient httpClient;

// ADXL335 analog output pins connected to the ESP32
const int xPin = A0; // x-axis of the accelerometer
const int yPin = A3; // y-axis
const int zPin = A4; // z-axis

// ADXL335 voltage reference and sensitivity
const float vRef = 3.3; // VREF (voltage reference) value for the ADXL335, usually 3.3V
const float sensitivity = 0.3; // Sensitivity of the ADXL335 in V/g (volts per g)

// Timing variables
unsigned long previousMillis = 0;
unsigned long currentMillis = 0;

const String serverBaseWithPort = "http://192.168.1.78:5000";

void setup() 
{
  Serial.begin(115200);

  pinMode(LED_BUILTIN, OUTPUT);

  #ifdef DEBUG_MODE_ON
    delay(3000); // Give user a chance to open serial monitor
  #endif  

  setupAutoConnect();

  Serial.println(F("Initialization done..."));
}

void loop() 
{
  currentMillis = millis(); // Get the current elapsed time

  autoConnect.handleClient();
  
  printLoopingMessageAndCurrentMillis();

  // Don't run beyond here unless we're connected
  if (!WiFi.status() == WL_CONNECTED) return;

  sendAccelerometerDataViaHttpGet(serverBaseWithPort);  
}