void setup() {
  // initialize the serial communication at 9600 baud:
  Serial.begin(9600);
}

void loop() {
  // send the value of analog input 0:
  int voltage_ticks = analogRead(A0);
  
  // stabalize the ADC
  delay(2);

  //get actual voltage value
  float analog_voltage = ticks_to_volts(voltage_ticks);

  //print out voltage value to console
  Serial.print(analog_voltage);
  Serial.println(" V");
}

float ticks_to_volts(int ticks) {
    return ticks * (5.0 / 1023);
}


