var express = require('express');
var router = express.Router();
var messageHelper = require("../helpers/messageHelper");
var logicHelpers = require("../helpers/logicHelpers");
var serialport = require("serialport");
var SerialPort = serialport.SerialPort;
var lines = {};
var started = false;
var startedBuf = "";
var sp;

router.post('/msgHandle', function (req, res) {
    console.log(req.body);

    lines = messageHelper.FormatMessage(req.body.msg);    

    // send msg to cigarbox (serial port)
    // https://cmsdk.com/node-js/how-to-write-in-serial-port-using-nodejs.html
    sp = new SerialPort( {
        path: "COM3",
        baudRate: 9600
      });
    // Switches the port into "flowing mode"
    sp.on('data', function (data) {
        console.log('Serial Data:', data.toString("utf8"))
        startedBuf =startedBuf.concat(startedBuf, data.toString("utf8"));
        if (startedBuf.indexOf("STARTED") >= 0)
        {
            started = true;
        }
    });

    sp.on('open', function () 
    {    
        logicHelpers.waitFor(()=>{return started},true,50,0,'msgHandle->sp.open', ()=>{
            console.log("writing...");
            lineNbr = 0;
            var rtn = Buffer.from("13","hex");
            var ack = Buffer.from("06","hex");

            lines.map(msg=>{
                var ln = Buffer.from(lineNbr.toString());            
                var data = Buffer.from(lines[lineNbr++]);
                var buf = Buffer.concat([ack, ln, data, rtn]);
                sp.write(buf);
                console.log("done with line " + lineNbr.toString() + buf.toString('utf8'));
            });
            console.log("done sending messages");
            var bufprt = Buffer.concat([ack, Buffer.from("5,PRINTSCR"), rtn]);
            sp.write(bufprt);
            console.log("PRINT " + bufprt.toString('utf8'));
        });
    });
      
    res.render('index', { title: 'Cigar Box Message handled', msg: req.body.msg });
});
  
module.exports = router;
