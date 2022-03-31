# How to interact with the LCD Cigar Box
This ino script can be edited in the Arduino IDE and pushed to the cigar box LCD.

However, it has a simple protocol you can use on any app.
Send it data for each line to be displayed, and then print the screen.
It keeps a state of the previous lines sent to it, so you can simply update one line if you'd like before printing the screen again.

### Key to format:
- 0x06 = ACK (shown below as )
- Line Number = single byte, zero based line number 0-4 (4+ is for PRINTSCR)
- Message = a set of 20 or less characters for a line
- PRINTSCR = message that will print the stored lines
- comma = optional comma can be placed after line number.
- LF = return char, uses only 0x0D, shown below as \n

### Format of each line to be sent:  
*ACK|Line Number|[comma]|Message or PRINTSCR|LF*  
hex ex: 06 30 4F 48 20 42 6F 79 20 69 74 20 68 61 73 20 73 74 61 74 65 21 0D  
text: 0OH Boy it has state!\n  
This sets the first line. Follow this with,  
5PRINTSCR\n  
and it will print all lines in state.


### Notes: 
- When sending PRINTSCR, linenumber is optional
- \n is only the LF (0x0D)
- use [Serial Debug Assistant](ms-windows-store://pdp/?ProductId=9nblggh43hdm)  to test the box
    - an [included example ini](./Serial%20Debug%20Assistant%20example.ini) can be loaded for this project.