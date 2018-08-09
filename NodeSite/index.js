// index.js
const path = require('path')
const fs = require('fs')
const express = require('express')
const bodyParser = require('body-parser')
const exphbs = require('express-handlebars')
const port = 3000;
const app = express()
app.use(bodyParser.urlencoded({
  extended: true
}));

/////// WEBCAM ITEMS
const NodeWebcam = require( "node-webcam" );
//Default options 
var opts = {
  //Picture related 
  width: 1280,
  height: 720,
  quality: 100,

  //Delay to take shot 
  delay: 0,

  //Save shots in memory 
  saveShots: true,

  // [jpeg, png] support varies 
  // Webcam.OutputTypes 
  output: "jpeg",

  //Which camera to use 
  //Use Webcam.list() for results 
  //false for default device 
  device: false,

  // [location, buffer, base64] 
  // Webcam.CallbackReturnTypes 
  callbackReturn: "location",

  //Logging 
  verbose: false
};

let imageUpdating = false;
function UpdateWebCamImage()
{
  if (imageUpdating) {return;}
  imageUpdating = true;
  //Creates webcam instance 
  var Webcam = NodeWebcam.create();

  //Get list of cameras 
  Webcam.list( function( list ) {
    //Use another device 
    Webcam = NodeWebcam.create( { device: list[ 1 ] } ); 
    Webcam.capture( "test_picture", function( err, data ) {
      if (err) {
          console.log("Failed to get image!");          
      }
      imageUpdating = false;
    });
  });
}
////// END WEBCAM ITEMS

////// Serial Port Items
function WriteToLog(msg)
{
  console.log(msg);
  fs.appendFile("e:/temp/testNodeLog.txt", msg, function(err) {
      if(err) {
          return console.log(err);
      }
  }); 
}

var SerialPort = require('serialport');
var serPortName = '\\\\.\\COM3';
serPortName = 'COM3';
var serialPort = new SerialPort(serPortName, {baudRate: 9600}, 
  function (err) {
    if (err) {
      return console.log('Error: ', err.message);
    }
  });

var ReturnData = "";
var WaitingOn = "~Waiting~";
serialPort.on ('data', function( data ) {
  console.log("data: " + data.toString());
  ReturnData+=data;
  var foundIndex = ReturnData.lastIndexOf(WaitingOn);
  if (foundIndex >= 0)
  {
    WaitingOn = "~FOUND~";
    ReturnData=ReturnData.substr(foundIndex);
  }
});
serialPort.on('error', function(err) {
  console.log('Error: ', err.message);
})

function SendData(lines, nbr = 0, attemptNbr = 0)
{
  if (attemptNbr > 5)
  {
    console.log("Failed to send in time");
    return;
  }
  if (attemptNbr++ > 0) // waiting for confirm
  {
    if (WaitingOn != "~FOUND~")
    {
      // did not get sent yet.. .wait
      setTimeout(SendData, 2000, lines, nbr, attemptNbr);
      return;
    }
    else
    {
      WaitingOn="~Waiting~";
      // success on that line!
      nbr++; attemptNbr=0;        
    }
  }

  if (nbr >= lines.length)
  {
    console.log("Sending PRTSCR\n");
    serialPort.write("PRINTSCR\n");
    return; //done.
  }

  WaitingOn = nbr + "," + lines[nbr]
  var data = "" + WaitingOn + "\n"
  console.log("Writing:\n" + data);
  serialPort.write(data);  
  // start waiting
  setTimeout(SendData, 2000, lines, nbr, attemptNbr);
}
////// END Serial Port Items

app.engine('.hbs', exphbs({
  defaultLayout: 'main',
  extname: '.hbs',
  layoutsDir: path.join(__dirname, 'views/layouts')
}))
app.set('view engine', '.hbs')
app.set('views', path.join(__dirname, 'views'))

app.post('/post',function(request,response){
    // console.log(request.body.mytext); //you will get your data in this as object.
    var lines = [request.body.mytext0, request.body.mytext1, request.body.mytext2, request.body.mytext3];
    SendData(lines);
    var answer = "Printing:"
    response.render('home', {
      Answer: answer,
      Line0: lines[0],
      Line1: lines[1],
      Line2: lines[2],
      Line3: lines[3]
    });
    var msg = "\nMessage:\n";
    msg += lines.join("\n");
    WriteToLog(msg);
 })

app.get('/test_picture.bmp', function(req,res){
  //console.log("Image requested");
  var imagePath =  __dirname + '/test_picture.bmp';
  res.sendFile(imagePath);
});

app.get('/messages.txt', function(req,res){
  console.log("msgs requested");
  var imagePath =  __dirname + '/Data/messages.txt';
  res.sendFile(imagePath);
});

app.get('/', (request, response) => {
  response.render('home', {
    name: 'John'
  });
})

app.use((err, request, response, next) => {
  // log the error, for now just console.log
  console.log(err)
  response.status(500).send('Something broke!')
})

app.listen(port, (err) => {
  if (err) {
    return console.log('something bad happened', err)
  }

  console.log(`server is listening on ${port}`)
  setInterval(UpdateWebCamImage, 2000);
})