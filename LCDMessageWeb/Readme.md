Load all modules
```powershell
npm install
```
In PowerShell set the debug var for output
```powershell
$env:DEBUG = "lcdmessageweb:*,-not_this"
```
```cmd
set DEBUG=lcdmessageweb:*,-not_this
```cmd

Then run the app
```powershell
npm start
```