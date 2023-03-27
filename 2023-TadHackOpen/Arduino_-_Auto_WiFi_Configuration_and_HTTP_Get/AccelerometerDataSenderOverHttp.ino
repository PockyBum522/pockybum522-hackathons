void sendAccelerometerDataViaHttpGet(String serverBaseUrlWithPort)
{
  if (currentMillis - previousMillis >= 500)
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

    #ifdef DEBUG_MODE_ON
      // Send the X, Y, and Z values over Serial
      Serial.print(F("Raw values from accelerometer; X: "));
      Serial.print(xG);
      Serial.print("g,\tY: ");
      Serial.print(yG);
      Serial.print("g,\tZ: ");
      Serial.print(zG);
      Serial.println("g");

      Serial.println(F("About to build URL from these..."));
    #endif

    // Build the url with converted data
    // char builtUrl[50] = getBuiltUrl(abs(xG), abs(yG), abs(zG));
    char* builtUrl = getBuiltUrl(abs(xG), abs(yG), abs(zG));

    #ifdef DEBUG_MODE_ON
      Serial.println();
      Serial.print(F("Built URL: "));
      Serial.println(builtUrl);
      Serial.println();
    #endif        

    // And then make the call
    if (WiFi.status() == WL_CONNECTED)
    {
      printHttpClientGetResult(builtUrl);
    }    
    else
    {
      Serial.print("Wifi status: ");
      Serial.println(WiFi.status());
    }
  }
}

char* getBuiltUrl(float xValue, float yValue, float zValue) 
{
  const int bufferSize = 100; // Adjust the buffer size if needed
  static char resultBuffer[bufferSize];

  String stringX = String(xValue, 2); // Convert the first float to a string with 2 decimal places
  String stringY = String(yValue, 2); // Convert the second float to a string with 2 decimal places
  String stringZ = String(zValue, 2); // Convert the third float to a string with 2 decimal places

  String concatenatedString = 
    serverBaseUrlWithPort +
    String("/detect") +
    String("/") + stringX + 
    String("/") + stringY + 
    String("/") + stringZ;
    
  concatenatedString.toCharArray(resultBuffer, bufferSize); // Copy the concatenated string to the result buffer

  return resultBuffer;
}