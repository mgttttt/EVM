int currentLED = D0; 

void setup() {
  pinMode(D0, OUTPUT);    
  pinMode(D1, OUTPUT);  
  pinMode(D2, OUTPUT);  
  attachInterrupt(D3, buttonInterrupt, RISING); 
  
  digitalWrite(D0, LOW);
  digitalWrite(D1, LOW);
  digitalWrite(D2, LOW);
}

void loop() {
  
}

void buttonInterrupt() {
  currentLED = getNextLED(currentLED);
  updateLED(currentLED);
}


int getNextLED(int current) {
  if (current == D0) {
    return D1;
  } else if (current == D1) {
    return D2;
  } else {
    return D0;
  }
}

void updateLED(int current) {
  
  digitalWrite(D0, LOW);
  digitalWrite(D1, LOW);
  digitalWrite(D2, LOW);


  digitalWrite(current, HIGH);
}