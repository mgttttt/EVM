int sensorValue = 0;

void setup() {
  analogWriteFreq(60);
}

void loop() {
  sensorValue = analogRead(A0);
  analogWrite(D0, map(sensorValue, 0, 4095, 0, 255););
  delay(10);
}
