#include <Wire.h>

// ADXL335 analog output pins connected to the ESP32
const int xPin = A0; // x-axis of the accelerometer
const int yPin = A3; // y-axis
const int zPin = A4; // z-axis

// ADXL335 voltage reference and sensitivity
const float vRef = 3.3; // VREF (voltage reference) value for the ADXL335, usually 3.3V
const float sensitivity = 0.3; // Sensitivity of the ADXL335 in V/g (volts per g)

// Timing variables
unsigned long previousMillis = 0;
const unsigned long interval = 500; // Time interval in milliseconds (1000 ms = 1 second)
 
void setup()
{
  Serial.begin(115200); // Begin serial communication at 115200 baud rate
  Serial.println(F("ESP32 with ADXL335 Accelerometer Init..."));
}
 
void loop()
{
  unsigned long currentMillis = millis(); // Get the current elapsed time

  if (currentMillis - previousMillis >= 250)
  { 
    previousMillis = currentMillis; // Update the previousMillis variable

    // Read the raw ADC values from the accelerometer
    int xRaw = analogRead(xPin);
    delay(10);
    int yRaw = analogRead(yPin);
    delay(10);
    int zRaw = analogRead(zPin);
    delay(10);

    // Convert the raw ADC values to g-force units
    float xG = (xRaw * vRef / 4095.0 - vRef / 2.0) / sensitivity;
    float yG = (yRaw * vRef / 4095.0 - vRef / 2.0) / sensitivity;
    float zG = (zRaw * vRef / 4095.0 - vRef / 2.0) / sensitivity;

    // Calibrate all to be 0 when flat
    xG += .6;
    yG += .56;
    zG -= .75;

    // Send the X, Y, and Z values over Serial
    Serial.print("X:\t");
    Serial.print(xG);
    Serial.print("g,\tY: ");
    Serial.print(yG);
    Serial.print("g,\tZ: ");
    Serial.print(zG);
    Serial.println("g");
  }
}