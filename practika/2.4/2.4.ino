#include <TroykaIMU.h>
#include <TroykaMeteoSensor.h>

Barometer barometer;
TroykaMeteoSensor meteoSensor;

void setup() {
  Serial.begin(115200);
  while(!Serial) {
  }
  Serial.println("Serial init OK");
  meteoSensor.begin();
  Serial.println("Meteo Sensor init OK");
  delay(1000);
  Serial.begin(9600);
    // Выводим сообщение о начале инициализации
  Serial.println("Barometer begin");
    // Инициализируем барометр
  barometer.begin();
    // Выводим сообщение об удачной инициализации
  Serial.println("Initialization completed");
}

void loop() {
  int stateSensor = meteoSensor.read();
  float pressureMillimetersHg = barometer.readPressureMillimetersHg();
  float temperature = barometer.readTemperatureC();
  switch (stateSensor) {
    case SHT_OK:
      Serial.println("Data sensor is OK");
      Serial.print("Temperature = ");
      Serial.print(meteoSensor.getTemperatureC());
      Serial.println(" C \t");
      Serial.print("Humidity = ");
      Serial.print(meteoSensor.getHumidity());
      Serial.println(" %\r\n");
      Serial.print(pressureMillimetersHg);
      Serial.print(" mmHg\t");
      Serial.print("Temperature: ");
      Serial.print(temperature);
      Serial.println(" C");
      break;
    case SHT_ERROR_DATA:
      Serial.println("Data error or sensor not connected");
      break; 
    case SHT_ERROR_CHECKSUM:
      Serial.println("Checksum error");
      break;
  }
  delay(1000);
}