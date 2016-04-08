void setup() {
  // initialize the serial communication at 9600 baud:
  Serial.begin(9600);
}

void loop() {
  // send the value of analog input 0:
  int pressure_ticks = analogRead(A0);
  delay(2);  // stabalize the ADC
  int salinity_ticks = analogRead(A1);
  delay(2);

  //get actual voltage value
  float pressure_voltage = ticks_to_volts(pressure_ticks);
  float salinity_voltage = ticks_to_volts(salinity_ticks);

  //print out voltage value to console
  Serial.print(pressure_voltage);
  Serial.println("p");
  Serial.print(salinity_voltage);
  Serial.println("s");
}

float ticks_to_volts(int ticks) {
    return ticks * (5.0 / 1023);
}


