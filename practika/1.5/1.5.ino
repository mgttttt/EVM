
const int timerInterval = 1000; 
struct repeating_timer timer;

int ind = 0;

bool timerISR(struct repeating_timer *timer) {
  digitalWrite(D0, LOW);
  digitalWrite(D1, LOW);
  digitalWrite(D2, LOW);
  ind = (ind + 1) % 3;
  digitalWrite(ind, HIGH);
  return true;
}

void setup() {
  pinMode(D0, OUTPUT);
  pinMode(D1, OUTPUT);
  pinMode(D2, OUTPUT);

  add_repeating_timer_ms(1000, timerISR, NULL, &timer);
}

void loop() {
  
}
