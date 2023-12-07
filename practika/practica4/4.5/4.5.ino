#include <Wire.h>
#include <iostream>
struct Request
{
  int pwm;
};
void setup() {
  analogWriteFreq(25000);
  Wire.begin(32);
  Wire.onReceive(update);
}

void loop() {}

void update(int size)
{
  uint8_t *PWM = new uint8_t[size];

  Wire.readBytes(PWM, size);
  
  uint32_t res = reinterpret_cast<Request*>(PWM)->pwm;

  delete[] PWM;

  analogWrite(D0, res);

  Serial.println(res);
}