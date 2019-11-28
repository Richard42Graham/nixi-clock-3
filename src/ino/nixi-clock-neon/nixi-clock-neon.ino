//#include <Wire.h>
#include <TinyWireS.h>

int LEDh = 9;
int LEDl = 10;

long previousMillis = 0;
long interval = 10;         // cound be duty cyal

int dutyCycal = 100;
float LED1Raw = 1.0;      // brighness LED 1
float LED2Raw = 1.0;     // brighness LED 2
float Total = 1;        // total brighness

byte own_address = 0xB4;  // adress of device.
int registers[4];

byte v;
byte x;
byte y;
byte z;

int LED1OnPeriod = 0;
int LED2OnPeriod = 0;
int LED1OffPeriod = 0;
int LED2OffPeriod = 0;
bool goingUP = false;

void setup() {
  // put your setup code here, to run once:

  pinMode(LEDh, OUTPUT);
  pinMode(LEDl, OUTPUT);

  digitalWrite(LEDh, LOW);
  digitalWrite(LEDl, LOW);

  TinyWireS.begin(own_address);             // join i2c bus with address #8
 TinyWireS.onReceive( receiveEvent ); // register event
 // Serial.begin(115200);  // start serial for output
}

/* Arduino-Based Interleaved PWM Generator

  Demo Hardware: Arduino Uno R3

  Prepared & Tested by: T.K.Hareendran

*/



void loop()
{


  digitalWrite(9, LOW);
  digitalWrite(10, LOW);
  swap();
  delayMicroseconds(LED1OffPeriod);
  //  swap();
  if (LED1OnPeriod != 0) {
    digitalWrite(9, HIGH);
    digitalWrite(10, LOW);
    delayMicroseconds(LED1OnPeriod); // Approximately 10% duty cycle @ 1KHz
  }
  //  swap();

  digitalWrite(9, LOW);
  digitalWrite(10, LOW);

  delayMicroseconds(LED2OffPeriod);
  //  swap();
  if (LED2OnPeriod != 0) {
    digitalWrite(9, LOW);
    digitalWrite(10, HIGH);
    delayMicroseconds(LED2OnPeriod); // Approximately 10% duty cycle @ 1KHz
  }

}


void swap()     // swap witch LED is on
{
  unsigned long currentMillis = millis();

  if (currentMillis - previousMillis > interval) {
    // save the last time you blinked the LED
    previousMillis = currentMillis;

//
//    if (Total <= 0.0 )
//    {
//      Total = 0.0;
//      goingUP = true;
//    }
//    if (Total >= 1.0) {
//      Total = 1.0;
//      goingUP = false;
//    }
//    if (goingUP)
//    {
//      Total = Total + 0.01;
//    } else {
//      Total = Total - 0.01;
//    }
    LED1OnPeriod = dutyCycal * (LED1Raw * Total);
    LED1OffPeriod = dutyCycal - LED1OnPeriod;
    LED2OnPeriod = dutyCycal * (LED2Raw * Total);
    LED2OffPeriod = dutyCycal - LED2OnPeriod;
  }
}



// function that executes whenever data is received from master
// this function is registered as an event, see setup()
void receiveEvent(int howMany) {
  while (4 < TinyWireS.available()) { // loop through all but the last
   int c = TinyWireS.read(); // receive byte as a character
 //  Serial.print("c");
//    Serial.println(c);         // print the character;
  }
  
//  Serial.print("X");
  x = TinyWireS.read();    // receive byte as an integer
 // Serial.println(x);

//  Serial.print("Y");
  y = TinyWireS.read();    // receive byte as an integer
 // Serial.println(y);
  
//  Serial.print("z");
  z = TinyWireS.read();    // receive byte as an integer
//  Serial.println(z);         // print the integer
 if(y < 4){
   registers[x] = y;
 }
 
// dutyCycal = v;
 LED1Raw =  ((float) registers[1])/((float)256);      // brighness LED 1
// Serial.println(registers[1]);
//  Serial.println(LED1Raw);

 LED2Raw =  ((float) registers[2])/((float)256);    // brighness LED 2
 Total =   ((float) registers[3])/((float)256);

      LED1OnPeriod = dutyCycal * (LED1Raw * Total);
    LED1OffPeriod = dutyCycal - LED1OnPeriod;
    LED2OnPeriod = dutyCycal * (LED2Raw * Total);
    LED2OffPeriod = dutyCycal - LED2OnPeriod;
}
