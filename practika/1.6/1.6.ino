const int ledPin = D0;
const int potPin = A0; 
const int pwmFrequency = 60; 

void setup() {
  pinMode(ledPin, PWM);
  gpio_set_function(potPin, GPIO_FUNC_ADC);
  adc_init();
  adc_gpio_init(potPin);
  adc_select_input(0);
  adc_set_clkdiv(8); 
}

void loop() {
  uint16_t potValue = adc_read();
  uint16_t pwmValue = map(potValue, 0, 65535, 0, 1023); 
  pwm_set_clkdiv_fractional(pwmPinToTimer(ledPin), pwmValue);
  sleep_ms(16); 
}