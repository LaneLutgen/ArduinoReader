int firstSensor = 0;
int secondSensor = 0;
int thirdSensor = 0;
int inByte = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(2, INPUT);
  establishContact();
}

void loop() {
  // put your main code here, to run repeatedly:
  if(Serial.available() > 0){
    inByte = Serial.read();
    firstSensor = analogRead(A0)/4;
    delay(10);
    secondSensor = analogRead(1)/4;
    thirdSensor = map(digitalRead(2), 0, 1, 0, 255);
    Serial.write(firstSensor);
    Serial.write(secondSensor);
    Serial.write(thirdSensor);
  }
}
void establishContact(){
  while(Serial.available() <= 0){
    Serial.print(firstSensor, DEC);
    Serial.print(", ");
    Serial.print(secondSensor, DEC);
    Serial.print(", ");
    Serial.print(thirdSensor, DEC);
    Serial.println();
    delay(300);  
  }
}

