void setup() {
  analogWriteFreq(25000);
}
void loop() {
int angle = map(analogRead(A0), 0, 4095, 0, 255);
analogWrite(D0, angle);
analogWrite(D1, angle);
}