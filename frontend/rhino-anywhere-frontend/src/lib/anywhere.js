import { initializeLocalConnection } from './initializers/initializeLocalConnection';
import { setupEvents } from './inputHandlers';

/**
 * Anywhere Library Creator
 * @param {HTMLVideoElement} videoElement
 * @param {string} url
 */
export function anywhere(videoElement, dataElement, url) {
  console.log('Setting up RhinoAnywhere');

  setupEvents(element, (data) => {
    sendData('input', data);
  });

  // TODO: Setup connection

  // TODO: TODO create public methods to allow us to send command and mouse movements
  let signalChannel;
  let localConnection;

  function _setup() {
    signalChannel = new WebSocket(url, []);
    localConnection = initializeLocalConnection(
      signalChannel,
      videoElement,
      dataElement
    ); //Need to establish vars for data input and output
  }

  function sendCommand() {}
}

export function sendCommand(string) {
  sendData('command', {
    command: string
  });
}

/**
 * Send data to connection
 * @param {string} type "input" or "command"
 * @param {Object} data
 */
function sendData(type, data) {
  var toSend = {
    type: type,
    data: data
  };

  // Send over communication channel here
  console.log(toSend);
}
