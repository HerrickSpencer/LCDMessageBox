This is a site to see if we can push text data to a serial port, which will display on an LCD screen remotely through Wixel transmiter.

To start the server run StartServer.bat from root.

index.js controls the node server and serves requests to the website. Lines below will serve up the home page in the views folder.
app.get('/', (request, response) => {
  response.render('home', {
    name: 'John'
  });
})

Installing/uninstalling service to run website on startup from https://stackoverflow.com/questions/20445599/auto-start-node-js-server-on-boot
There's this super easy module that installs a node script as a windows service, it's called node-windows (npm, github, documentation). I've used before and worked like a charm.
var Service = require('node-windows').Service;

// Create a new service object
var svc = new Service({
  name:'Hello World',
  description: 'The nodejs.org example web server.',
  script: 'C:\\path\\to\\helloworld.js'
});

// Listen for the "install" event, which indicates the
// process is available as a service.
svc.on('install',function(){
  svc.start();
});

svc.install();

p.s.
I found the thing so useful that I built an even easier to use wrapper around it (npm, github).
Installing it:
npm install -g qckwinsvc
Installing your service:
> qckwinsvc
prompt: Service name: [name for your service]
prompt: Service description: [description for it]
prompt: Node script path: [path of your node script]
Service installed
Uninstalling your service:
> qckwinsvc --uninstall
prompt: Service name: [name of your service]
prompt: Node script path: [path of your node script]
Service stopped
Service uninstalled