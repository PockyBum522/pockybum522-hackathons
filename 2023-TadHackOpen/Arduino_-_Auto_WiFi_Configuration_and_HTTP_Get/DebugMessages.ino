void printLoopingMessageAndCurrentMillis()
{
  #ifdef DEBUG_SHOW_LOOP
    Serial.print("Looping @ ");
    Serial.println(currentMillis);
  #endif
}