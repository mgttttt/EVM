#include <SPI.h>
#include <WiFi.h>
#include <string>
std::string ssid = "ASOIU";     
std::string pass = "kaf.asoiu.48";   
int status = WL_IDLE_STATUS;  

int pingResult;

void setup() {
  Serial.begin(9600);
  while (!Serial) {
    continue; 
  }

  if (WiFi.status() == WL_NO_SHIELD) {
    Serial.println("WiFi Shield not present");
    // don't continue:
    while (true);
  }

  while ( status != WL_CONNECTED) {
    Serial.print("Attempting to connect to WPA SSID: ");
    Serial.println(ssid.c_str());
    status = WiFi.begin(ssid.c_str(), pass.c_str());

    delay(5000);
  }

  Serial.println("You're connected to the network");
  Serial.println(WiFi.localIP());
}

void loop() {
  if (Serial.available())
  {
    std::string hostname;
    hostname = Serial.readString().c_str();
    Serial.print("Pinging ");
    Serial.print(hostname.substr(0, hostname.size() - 1).c_str());
    Serial.print(": ");

    pingResult = WiFi.ping(hostname.substr(0, hostname.size() - 1).c_str());

    if (pingResult >= 0) {
      Serial.print("SUCCESS! RTT = ");
      Serial.print(pingResult);
      Serial.println(" ms");
    } else {
      Serial.print("FAILED! Error code: ");
      Serial.println(pingResult);
    }
    delay(5000);
  }
}