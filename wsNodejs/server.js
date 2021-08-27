// server.js
const ess = require("event-source-stream");
const WebSocket = require("ws");

const wss = new WebSocket.Server({ port: 8090 });

console.log(`Received message`);

console.dir(wss);

wss.on("connection", (ws) => {
  ws.on("message", (message) => {
    console.log(`Received message => ${message}`);
  });
  ess(
    "https://konnect.knowlarity.com:8100/update-stream/d59bc895-ebc1-483c-8dbb-87174ce78f91/konnect"
  ).on("data", function (data) {
    console.log("received event:", data);
    ws.send(data);
  });
});
