//Lets require/import the HTTP module
var http = require('http');
var dispatcher = require('httpdispatcher');
var azure = require('azure');
var notificationHubService;

//Lets define a port we want to listen to
var PORT = 8080;

//We need a function which handles requests and send response
function handleRequest(request, response) {
    try {
        //log the request on console
        console.log(request.url);
        //Dispatch
        dispatcher.dispatch(request, response);
    } catch (err) {
        console.log(err);
    }
}

//For all your static (js/css/images/etc.) set the directory name (relative path).
dispatcher.setStatic('resources');

dispatcher.onGet("/pushTemplate", function (req, res) {
    notificationHubService.send(null,
    {
        title: "Template notification",
        message: 'This is my template notification'
    },
    function (error) {
        if (!error) {
            console.log("Template notification sent with success");
        }
        else {
            console.log("Template notification sent with error");
        }
    });
    
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('Template notification sent');
});

dispatcher.onGet("/pushNative", function (req, res) {
    var xml = '<toast><visual><binding template="ToastGeneric"><text>Hello insiders!</text><text>This is a notification from Notification Hub</text></binding></visual></toast>';
    notificationHubService.wns.send(null, xml, 'wns/toast', function (error) {
        if (!error) {
            console.log("Native notification sent with success");
        }
        else {
            console.log("Native notification sent with error");
        }
    });
    
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    res.end('Template notification sent');
});

//Create a server
var server = http.createServer(handleRequest);

//Lets start our server
server.listen(PORT, function () {
    //Callback triggered when server is successfully listening. Hurray!
    console.log("Server listening on: http://localhost:%s", PORT);
    notificationHubService = azure.createNotificationHubService('uwpsample', 'Endpoint=sb://windows10samples.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=DlbQZoLHpq49BNJbP9YmkRVPoN4jqCfnJZwt+vAHU24=');
});