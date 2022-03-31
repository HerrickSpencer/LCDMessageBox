exports.FormatMessage = function (msg) {
    var message = String(msg);
    var lines = message.split('\r\n');
    return lines;
}