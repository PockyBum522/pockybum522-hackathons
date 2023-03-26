void printHttpClientGetResult(char* httpGetUrl)
{
  #ifdef DEBUG_MODE_ON
    Serial.println("HTTP Get firing to: " + String(httpGetUrl));
  #endif

  httpClient.begin(httpGetUrl);
  
  if (httpClient.GET() > 0) 
  {
    Serial.println(F("httpClient.getString(): "));
    Serial.println(httpClient.getString());
  }
  
  httpClient.end();
}