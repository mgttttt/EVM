#include <Servo.h>

Servo servo; 

int potPin = A0; 
int angle = 0; 

void setup() {
  servo.attach(D0, 600, 2600);
}

void loop() {
int potValue = analogRead(potPin);
angle = map(potValue, 0, 1023, 0, 180);
servo.write(angle);
delay(25); 
}