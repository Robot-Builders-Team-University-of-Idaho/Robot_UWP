#include <Servo.h>

int startpos = 0;

int state = 0;
int timer = 500;

Servo servo1;
Servo servo2;
Servo servo3;
Servo servo4;
Servo servo5;
Servo servo6;

int pin1 = 32;
int pin2 = 36;
int pin3 = 40;
int pin4 = 44;

void setup() {
  Serial.begin(115200);
  Serial.setTimeout(5);
  servo1.attach(13);
  servo2.attach(12);
  servo3.attach(11);
  servo4.attach(10);
  servo5.attach(9);
  servo6.attach(8);
  
  servo1.write(startpos);
  servo2.write(startpos);
  servo3.write(startpos);
  servo4.write(startpos);
  servo5.write(startpos);
  servo6.write(startpos);

  pinMode(pin1, OUTPUT);
  pinMode(pin2, OUTPUT);
  pinMode(pin3, OUTPUT);
  pinMode(pin4, OUTPUT);
  delay(100);
  
  digitalWrite(pin1, LOW);
  digitalWrite(pin2, LOW);
  digitalWrite(pin3, LOW);
  digitalWrite(pin4, LOW);
}

void loop() {
  
  while(Serial.available() == 0){ 
  }
  String input = Serial.readString();
  Serial.flush();

  
  
  int pos1 = (input.substring(0, 3)).toInt();
  int pos2 = (input.substring(3, 6)).toInt();
  int pos3 = (input.substring(6, 9)).toInt();
  int pos4 = (input.substring(9, 12)).toInt();
  int pos5 = (input.substring(12, 15)).toInt();
  int pos6 = (input.substring(15, 18)).toInt();
  state = (input.substring(18, 19)).toInt();

  switch(state){
    case 0: //nothing 
      digitalWrite(pin1, LOW);
      digitalWrite(pin2, LOW);
      digitalWrite(pin3, LOW);
      digitalWrite(pin4, LOW);
      timer = 50;
      break;
    case 1: //blowing
      if (timer > 0){
        digitalWrite(pin1, HIGH);
        digitalWrite(pin2, HIGH);
        digitalWrite(pin3, HIGH);
        digitalWrite(pin4, HIGH);
        timer -= 1;
      } else {
        digitalWrite(pin1, LOW);
        digitalWrite(pin2, LOW);
        digitalWrite(pin3, LOW);
        digitalWrite(pin4, LOW);
      }
      break;
    case 2: //sucking
      digitalWrite(pin1, LOW);
      digitalWrite(pin2, LOW);
      digitalWrite(pin3, LOW);
      digitalWrite(pin4, HIGH);
      timer = 50;
      break;
    default:
      timer = 50;
      break;
  }
  
  servo1.write(pos1);
  servo2.write(pos2);
  servo3.write(pos3);
  servo4.write(pos4);
  servo5.write(pos5);
  servo6.write(pos6);
  input = "";
}
