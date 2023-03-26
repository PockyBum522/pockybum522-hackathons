void setupAutoConnect()
{
  #ifdef DEBUG_MODE_ON
    Serial.println(F("Setting up AutoConnect..."));
  #endif

  autoConnectConfig.apid = "New Phi Device";
  autoConnectConfig.psk  = "12345678";


  autoConnect.config(autoConnectConfig);
  autoConnect.onDetect(onDetect);
  
  if (autoConnect.begin()) 
  {
    WebServer& webServer = autoConnect.host();

    webServer.on("/", handleRoot);
    webServer.on("/io", handleIO);
    
    #ifdef DEBUG_MODE_ON
      Serial.println(F("AutoConnect handled redirect succesfully"));
    #endif
  }
  else 
  {
    Serial.println(F("AutoConnect attempted redirect BUT HAD EXCEPTIONS"));
  }
}