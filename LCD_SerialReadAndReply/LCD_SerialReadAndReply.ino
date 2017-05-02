// include the library code:
#include <LiquidCrystal.h>

//give arduino pin # for each LCD pin
enum {
  RS = 7, //LCD#4
  EN = 8, //LCD#6
  DB4 = 9,//LCD#11
  DB5 = 10,//LCD#12
  DB6 = 11,//LCD#13
  DB7 = 12 //LCD#14
};

enum {
  COLS = 20,
  ROWS = 4
};
String incomingMsg = "";
int lnNmb;
bool lineComplete = false;
String lines[4];
// initialize the library with the numbers of the interface pins
LiquidCrystal lcd(RS, EN, DB4, DB5, DB6, DB7);

void setup() {
  Serial.begin(115200);
  // set up the LCD's number of columns and rows: 
  lcd.begin(COLS, ROWS);
  // Print a message to the LCD.
  lcd.print("Hello World!");
  Serial.println("STARTED");
}

void serialEvent(){
while (Serial.available()) {
  char last = Serial.read();
  
  switch (last)
  {
    case '\x06':
      ResetLine();
      lnNmb = Serial.parseInt();
      last = Serial.read();
      if (last != ',')
      {
        incomingMsg+=last;
      }
      Serial.println("Got Ack");
      break;
    case '\n':
      if (lnNmb < ROWS)
      {
        lines[lnNmb] = incomingMsg;
      }
      else
      {
        if (incomingMsg=="PRINTSCR")
        {
          PrintScreen();
        }
      }
      lineComplete = true;
      Serial.println("Got CR");
      break;
    default:
      incomingMsg+=last;
  }
}    

}

void PrintScreen()
{
  Serial.println("Printing Screen");
  lcd.clear();
  lcd.setCursor(0, 0);
  for (int i=0;i<ROWS;i++)
  {
    lcd.setCursor(0, i);
    lcd.print(lines[i]);
  }
//  int lastPos = COLS * ROWS;
//  int cursorPos[] = { 0,0 };
//  for (int i=0;i<lastPos;i++) {
//    lcd.write(lines[cursorPos[0]][cursorPos[1]]);
//    cursorPos[0]++;
//    if (cursorPos[0] >= COLS)
//    {
//      cursorPos[0] = 0;
//      cursorPos[1]++;
//      if (cursorPos[1] >= ROWS)
//      {
//        cursorPos[1] = 0;
//      }
//      lcd.setCursor(cursorPos[0],cursorPos[1]);
//    }
//  }
}

void ResetLine()
{
  lnNmb = 67;
  incomingMsg = "";
  lineComplete = false;
}

void loop() {
  if (lineComplete)
  {
    Serial.print(lnNmb);
    Serial.print(",");
    Serial.println( incomingMsg);
    ResetLine();
  }
}


