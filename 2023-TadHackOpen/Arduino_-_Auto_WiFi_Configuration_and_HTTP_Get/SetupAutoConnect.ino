void setupAutoConnect()
{
  #ifdef DEBUG_MODE_ON
    Serial.println(F("Setting up AutoConnect..."));
  #endif

  autoConnectConfig.apid = "New Phi Device";
  autoConnectConfig.psk  = "12345678";

  // On boot, attempts to connect to known networks. Fallback is to start the portal/ad hoc wifi
  autoConnectConfig.autoReconnect = true;
  
  // If disconnected from wifi, ESP will not reset
  autoConnectConfig.autoReset = false;
  
  // Enable OTA
  autoConnectConfig.ota = AC_OTA_BUILTIN;

  // When connecting to known networks, strongest RSSI is the priority
  autoConnectConfig.principle = AC_PRINCIPLE_RSSI;
  
  // Auto reconnect every 30s
  autoConnectConfig.reconnectInterval = 1;

  // Set title on home screen
  autoConnectConfig.title = "New Phi Device";

  //autoConnect.whileConnecting(autoConnectWhileConnecting);
  autoConnect.config(autoConnectConfig);
  
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
