int currentLED = D0;  // Текущий активный светодиод

void setup() {
  pinMode(D0, OUTPUT);     // Устанавливаем пин с красным светодиодом как выход
  pinMode(D1, OUTPUT);  // Устанавливаем пин с желтым светодиодом как выход
  pinMode(D2, OUTPUT);   // Устанавливаем пин с зеленым светодиодом как выход
  attachInterrupt(D3, buttonInterrupt, RISING); // Привязываем прерывание к пину D3
  // Изначально все светодиоды выключены
  digitalWrite(D0, LOW);
  digitalWrite(D1, LOW);
  digitalWrite(D2, LOW);
}

void loop() {
  // В этом примере, loop() не выполняет никаких действий
  // Вся логика управления светодиодами реализуется через аппаратное прерывание
}

void buttonInterrupt() {
  currentLED = getNextLED(currentLED);
  updateLED(currentLED);
}

// Функция для получения следующего светодиода в очереди
int getNextLED(int current) {
  if (current == D0) {
    return D1;
  } else if (current == D1) {
    return D2;
  } else {
    return D0;
  }
}

// Функция для обновления состояния светодиодов
void updateLED(int current) {
  // Выключаем все светодиоды
  digitalWrite(D0, LOW);
  digitalWrite(D1, LOW);
  digitalWrite(D2, LOW);

  // Включаем текущий светодиод
  digitalWrite(current, HIGH);
}