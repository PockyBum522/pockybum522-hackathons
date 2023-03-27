void printLoopingMessageAndCurrentMillis()
{
  #ifdef DEBUG_SHOW_LOOP
    Serial.print("Looping @ ");
    Serial.println(currentMillis);

    
    Serial.print("WiFi.status(): ");
    Serial.println(WiFi.status());
  #endif
}