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

#include <AutoConnectCore.h>
#include <HTTPClient.h>
#include <WebServer.h>
#include <WiFi.h>

#define LED_BUILTIN 2

const char* SSID = "";
const char* PASSWORD = "";

const char* SERVER_URL = "http://192.168.1.78:8000";

AutoConnect autoConnect;

void setup() 
{
  Serial.begin(115200);

  pinMode(LED_BUILTIN, OUTPUT);

  autoConnect.onDetect(onDetect);
  if (autoConnect.begin()) 
  {
    WebServer& webServer = autoConnect.host();
    webServer.on("/", handleRoot);
    webServer.on("/io", handleIO);
    Serial.println("Good!");
  }
  else 
  {
    Serial.println("Bad!");
  }

  WiFi.mode(WIFI_STA);
  WiFi.begin(SSID, PASSWORD);

  Serial.print("Connecting");

  while (WiFi.status() != WL_CONNECTED) 
  {
    delay(1000);
    Serial.print(".");
  }

  Serial.print("Connected to ");
  Serial.print(SSID);
  Serial.print(" with ");
  Serial.print(WiFi.localIP().toString());
  Serial.println("!");
}

void loop() 
{
  autoConnect.handleClient();

  if (WiFi.status() == WL_IDLE_STATUS) 
  {
    Serial.println(F("WiFi.status() == WL_IDLE_STATUS, so about to restart ESP."));

    delay(100);

    ESP.restart();
  }
  
  HTTPClient httpClient;
  httpClient.begin(SERVER_URL);
  
  if (httpClient.GET() > 0) 
  {
    Serial.println("httpClient.getString(): ");
    Serial.println(httpClient.getString());
  }
  
  httpClient.end();
  
  delay(5000);
}

void sendRedirect(String uri) 
{
  WebServer& webServer = autoConnect.host();

  webServer.sendHeader("Location", uri, true);
  webServer.send(302, "test/plain");
  
  webServer.client().stop();
}

bool onDetect(IPAddress& ipAddress) 
{
  Serial.println(ipAddress.toString());

  return true;
}

void handleRoot() 
{
  String html = PSTR(
    "<html>"
    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">"
    "<style type=\"text/css\">"
    "body {"
    "-webkit-appearance: none;"
    "-moz-appearance: none;"
    "font-family: \"Arial\", sans-serif"
    "text-align: center;"
    "}"
    ".menu > a: link {"
    "position: absolute;"
    "display: inline-block;"
    "right: 12px;"
    "padding: 0 6px;"
    "text-decoration: none;"
    "}"
    ".button {"
    "display: inline-block;"
    "border-radius: 7px;"
    "background: #73ad21"
    "margin: 0 10px 0 10px;"
    "padding: 10px 20px 10px 20px;"
    "text-decoration: none;"
    "color: #000000;"
    "}"
    "</style>"
    "<head>"
    "</head>"
    "<body>"
    "<div class=\"menu\">" AUTOCONNECT_LINK(BAR_32) "</div>"
    "LED_BUILTIN<br>"
    "IO("
  );

  html += String(LED_BUILTIN);
  html += String(F(") : <span style=\"font-weight: bold; color: "));
  html += digitalRead(LED_BUILTIN) ? String("Tomato\"HIGH") : String("SlateBlue\">LOW");
  html += String(F("</span>"));
  html += String(F("<p><a class=\"button\" href=\"/io?value=low\">LOW</a><a class=\"button\" href=\"/io?value=high\">HIGH</a></p>"));
  html += String(F("</body></html>"));

  autoConnect.host().send(200, "text/html", html);
}

void handleIO() 
{
  WebServer& webServer = autoConnect.host();
  
  if (webServer.arg("value") == "low") 
  {
    digitalWrite(LED_BUILTIN, LOW);
  } 
  else if (webServer.arg("value") == "high") 
  {
    digitalWrite(LED_BUILTIN, HIGH);
  }

  sendRedirect("/");
}