        function KeyPressed() {
            var keynum
            var keychar
            var numcheck

            if (window.event) // IE
            {
                keynum = event.keyCode
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                keynum = event.which
            }
            keychar = String.fromCharCode(keynum)
            numcheck = /\n/
            if (numcheck.test(keychar)) {
                btnSend.click();
                return false;
            }
            else {
                MainContent_txtLCDMessage.innerText = '';
                FormatMessage(MainContent_txtLCDMessage.innerText + keychar)
                return !numcheck.test(keychar)
            }
        }
        function FormatMessage(message) {
            var messageLabels = (MainContent_lblMessageLine0, MainContent_lblMessageLine1, MainContent_lblMessageLine2, MainContent_lblMessageLine3)
            MainContent_lblMessageLine0.innerText = "";
            MainContent_lblMessageLine1.innerText = "";
            MainContent_lblMessageLine2.innerText = "";
            MainContent_lblMessageLine3.innerText = "";

            for (var row = 0; row < 4; row++) {
                for (var i = 0; i < 20; i++) {
                    if (message.length < row * i) {
                        return;
                    }
                    switch (row) {
                        case 0:
                            MainContent_lblMessageLine0.innerText += message[row * i];
                            break;
                        case 1:
                            MainContent_lblMessageLine0.innerText += message[row * i];
                            break;
                        case 2:
                            MainContent_lblMessageLine0.innerText += message[row * i];
                            break;
                        case 3:
                            MainContent_lblMessageLine0.innerText += message[row * i];
                            break;
                    }
                }
            }
        }