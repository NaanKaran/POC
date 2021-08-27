const WebSocket = require("ws");
//const url = "ws://localhost:3000";
const url = "wss://stage-pro-annahelpline-ticketing-api.azurewebsites.net";
const connection = new WebSocket(url);

connection.onopen = () => {
  connection.send("Message From Client");
};

connection.onerror = (error) => {
  console.log(`WebSocket error: ${error}`);
  console.dir(`WebSocket error: ${error}`);
  console.dir(error);
};

connection.onmessage = (e) => {
  console.log(e.data);
};
