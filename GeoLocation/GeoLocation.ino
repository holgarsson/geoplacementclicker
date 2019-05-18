#include "rn2xx3.h"
#include "TinyGPS++.h"

// Setting up everything needed for LoRa.
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

// Setting up everything needed for GPS.
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
  
  sLORA.begin(57600, SERIAL_8N1, RxL, TxL); // Starting HW serial for LoRa.

  Serial.println("Ready");

  Serial.println("Resetting rn2483"); // Resets the LoRa module.
  digitalWrite(RST, LOW);
  delay(500);
  digitalWrite(RST, HIGH);
  delay(500);

  Serial.println("Setting up LORA");
  Serial.println(LORA.sysver());  // Printing some info to check if connection is OK.
  Serial.println(hweui); // printing server setup information
  Serial.println(appeui);
  Serial.println(appKey);

  Serial.println("Connecting to server");
  connectResult = LORA.initOTAA(appeui, appKey); //Using OTAA to connect to the server.
  while(!connectResult) {
    Serial.println("Unable to join.");
    delay(5000); //delay 5 seconds before retry
    connectResult = LORA.initOTAA(appeui, appKey); // Loop runs until lora module reports that it has connected to the server.
  }
  Serial.println("Successfully joined");
  digitalWrite(GLED, HIGH);
  delay(1000);
  digitalWrite(GLED, LOW);
  
  Serial.println("Starting GPS and running code");
  sGPS.begin(9600, SERIAL_8N1, RxG, TxG); // Setting up HW serial for GPS.
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

  if (interval > offTime) { // compares how long the button was pressed to the specified time.
      readGPS(); // runs the function that verifies and prints the GPS coordinates.
      if (valid) {
        digitalWrite(GLED, HIGH);
        addSOS = true; // specifies which of the two packages should be sent.
        sendLora(); // runs the function that transmits the package
        digitalWrite(GLED, LOW);
      } else {
        flashRED(); // flashes the red led if coordinates was not verified
      }
    } else if (pushed) {
      readGPS(); // runs the function that verifies and prints the GPS coordinates.
      if (valid) {
        digitalWrite(GLED, HIGH);
        addSOS = false; // specifies which of the two packages should be sent.
        sendLora(); // runs the function that transmits the package
        digitalWrite(GLED, LOW);
      } else {
        flashRED(); // flashes the red led if coordinates was not verified
      }
   }
   pushed = false;
}

void readGPS() {
  while (sGPS.available()) { // checks if anything is available in the buffer
    if (GPS.encode(sGPS.read())) { // encodes the data from the GPS module
      Serial.print("Location: ");
      if (GPS.location.isValid()) { // checks if valid
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
  latitude = String(GPS.location.lat(), 6); // saves the current location with 6 decimals
  longitude = String(GPS.location.lng(), 6);
  if (addSOS) { // looks for what package to sent, based on how long the button was pressed
    Serial.println("Sending: SOS "  + latitude + ", " + longitude);
    LORA.txUncnf("S," + latitude + "," + longitude); // command to transmit using the lora module
    Serial.println("SENT");
  } else {
    Serial.println("Sending: " + latitude + ", " + longitude);
    LORA.txUncnf("N," + latitude + "," + longitude); // command to transmit using the lora module
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
