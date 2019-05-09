#include "rn2xx3.h"
#include "TinyGPS++.h"

// Lora
const int TxL = 17;
const int RxL = 16;
const int RST = 23;
const int GLED = 15;

String hweui = "0004A30B00212AA4";
String appeui = "BE7A000000001172";
String appKey = "B34EE571D1FCF41BCAA910216475EC4F";

boolean connectResult = false;
HardwareSerial sLORA(1);
rn2xx3 LORA(sLORA);

// GPS
const int TxG = 18;
const int RxG = 19;
const int RLED = 2;
HardwareSerial sGPS(2);
TinyGPSPlus GPS;
String latitude, longitude = ""; 

// Button
const int button = 22;
int buttonState = 0;
boolean pushed = false;
boolean addSOS = false;
boolean valid = false;

// Timer
unsigned long startTime = 0; // Time for timer start
int interval; // current time.
int offTime = 2; // timer for how long to hold down button. Set to 2s


void setup() {
  delay(1000);

  pinMode(button, INPUT);
  pinMode(RST, OUTPUT);
  pinMode(GLED, OUTPUT);
  pinMode(RLED, OUTPUT);
  digitalWrite(GLED, LOW);
  digitalWrite(RLED, LOW);

  Serial.begin(115200);
  
  sLORA.begin(57600, SERIAL_8N1, RxL, TxL);

  Serial.println("Ready");

  Serial.println("Resetting rn2483");
  digitalWrite(RST, LOW);
  delay(500);
  digitalWrite(RST, HIGH);
  delay(500);

  Serial.println("Setting up LORA");
  Serial.println(LORA.sysver());
  Serial.println(hweui);
  Serial.println(appeui);
  Serial.println(appKey);

  Serial.println("Connecting to server");
  connectResult = LORA.initOTAA(appeui, appKey);
  while(!connectResult) {
    Serial.println("Unable to join.");
    delay(5000); //delay 5 seconds before retry
    connectResult = LORA.initOTAA(appeui, appKey);
  }
  Serial.println("Successfully joined");
  digitalWrite(GLED, HIGH);
  delay(1000);
  digitalWrite(GLED, LOW);
  
  Serial.println("Starting GPS and running code");
  sGPS.begin(9600, SERIAL_8N1, RxG, TxG);
}

void loop() {

  startTime = millis();
  interval = (millis() - startTime)/1000;
  buttonState = digitalRead(button);
  
  while (buttonState) {
    pushed = true;
    interval = (millis() - startTime)/1000;
    buttonState = digitalRead(button);
  }

  if (interval > offTime) {
      readGPS();
      if (valid) {
        digitalWrite(GLED, HIGH);
        addSOS = true;
        sendLora();
        digitalWrite(GLED, LOW);
      } else {
        flashRED();
      }
    } else if (pushed) {
      readGPS();
      if (valid) {
        digitalWrite(GLED, HIGH);
        addSOS = false;
        sendLora();
        digitalWrite(GLED, LOW);
      } else {
        flashRED();
      }
   }
   pushed = false;
}

void readGPS() {
  while (sGPS.available()) {
    if (GPS.encode(sGPS.read())) {
      Serial.print("Location: ");
      if (GPS.location.isValid()) {
        Serial.print(GPS.location.lat(), 6);
        Serial.print(", ");
        Serial.println(GPS.location.lng(), 6);
        valid = true;
      } else {
        Serial.println("INVALID");
        valid = false;
      }
    }
  }
}

void sendLora() {
  latitude = String(GPS.location.lat(), 6);
  longitude = String(GPS.location.lng(), 6);
  if (addSOS) {
    Serial.println("Sending: SOS "  + latitude + ", " + longitude);
    LORA.txUncnf("S," + latitude + "," + longitude);
    Serial.println("SENT");
  } else {
    Serial.println("Sending: " + latitude + ", " + longitude);
    LORA.txUncnf("N," + latitude + "," + longitude);
    Serial.println("SENT");
  }
}

void flashRED() {
  digitalWrite(RLED, HIGH);
  delay(300);
  digitalWrite(RLED, LOW);
  delay(100);
  digitalWrite(RLED, HIGH);
  delay(300);
  digitalWrite(RLED, LOW);
  delay(100);
  digitalWrite(RLED, HIGH);
  delay(300);
  digitalWrite(RLED, LOW);
}
