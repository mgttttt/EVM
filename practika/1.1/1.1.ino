int currentLED = D0;  
bool buttonWasPressed = false; 

void setup() {
  pinMode(D0, OUTPUT);    
  pinMode(D1, OUTPUT);  
  pinMode(D2, OUTPUT);   
  pinMode(D3, INPUT);      
  
  digitalWrite(D0, LOW);
  digitalWrite(D1, LOW);
  digitalWrite(D2, LOW);
}

void loop() {
  int buttonState = digitalRead(D3);  

  if (buttonState == HIGH && !buttonWasPressed) {
    currentLED = getNextLED(currentLED);
    updateLED(currentLED);
    buttonWasPressed = true;
  } else if (buttonState == LOW) {
    buttonWasPressed = false;
  }
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