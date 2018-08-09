// index.js
const path = require('path')
const fs = require('fs')
const express = require('express')
const bodyParser = require('body-parser')
const exphbs = require('express-handlebars')
const port = 4000;
const app = express()
app.use(bodyParser.urlencoded({
  extended: true
}));

app.engine('.hbs', exphbs({
  defaultLayout: 'main',
  extname: '.hbs',
  layoutsDir: path.join(__dirname, 'views/layouts')
}))
app.set('view engine', '.hbs')
app.set('views', path.join(__dirname, 'views'))

app.get('/test_picture.bmp', function(req,res){
    //console.log("Image requested");
    var imagePath =  __dirname + '/test_picture.bmp';
    res.sendFile(imagePath);
  });
  
 app.get('*.txt', function(req,res){
  console.log("msgs requested");
  var imagePath =  __dirname + '/Data/messages.txt';
  res.sendFile(imagePath);
});

app.get('/', (request, response) => {
  response.render('test', {
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
})