var express = require('express');
var router = express.Router();
var serialport = require("serialport");
var SerialPort = serialport.SerialPort;

router.post('/msgHandle', function (req, res) {
    console.log(req.body);

    // send msg to cigarbox (serial port)
    // https://cmsdk.com/node-js/how-to-write-in-serial-port-using-nodejs.html
    var sp = new SerialPort( {
        path: "COM3",
        baudRate: 9600
      });
      sp.on('open', function () 
      {
          console.log("writing...");
          sp.write(req.body.msg);
          console.log("done");
      });
      
    res.render('index', { title: 'Cigar Box Message handled', msg: req.body.msg });
  });
  
module.exports = router;
