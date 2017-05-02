Need to install device drivers for wixel : "\LCDMessageBoxController\wixel-windows\setup.exe"
Need to set correct Com Port (look in device manager or wixel configuration utility for which is wixel)
	set private string TRANSEIVERPORT = "COM3"; IN ..\LCDMessageBoxController\Service1.cs


Need to install service from Visual Studio Command window in administrator mode.
Installutil "...\LCDMessageBoxController\bin\Debug\LCDMessageBoxController.exe"

Then start LCDMessageBoxContoller service from services.msc

Changes... 
stop service
uninstall service (/u)
build in vs
install service
start service


Logs to Application log with app name LCDMessageBoxContoller