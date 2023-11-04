import { setupEvents } from "./inputHandlers"

/**
 * Anywhere Library Creator
 * @param {HTMLVideoElement} element
 * @param {string} url
 */
export function anywhere(element, url) {
  console.log("Setting up RhinoAnywhere");

  setupEvents(element, (data) => {
    sendData("input", data);
  })

  // TODO: Setup connection

  // TODO: TODO create public methods to allow us to send command and mouse movements

  function _setup() {}
}

export function sendCommand(string) {
  sendData("command", {
    "command":string
  })
}

/**
 * Send data to connection
 * @param {string} type "input" or "command"
 * @param {Object} data 
 */
function sendData(type, data){
  var toSend = {
    type: type,
    data: data
  }

  // Send over communication channel here
  console.log(toSend);
}