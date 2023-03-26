void printHttpClientGetResult(const char* httpGetUrl)
{
  HTTPClient httpClient;

  httpClient.begin(httpGetUrl);
  
  if (httpClient.GET() > 0) 
  {
    Serial.println("httpClient.getString(): ");
    Serial.println(httpClient.getString());
  }
  
  httpClient.end();
}