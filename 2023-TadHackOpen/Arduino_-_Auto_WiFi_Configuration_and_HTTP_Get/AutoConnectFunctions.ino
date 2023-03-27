void sendRedirect(String uri) 
{
  WebServer& webServer = autoConnect.host();

  webServer.sendHeader("Location", uri, true);
  webServer.send(302, "test/plain");
  
  webServer.client().stop();
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