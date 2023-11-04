import initializeLocalConnection from './initializers/initializeLocalConnection';

/**
 * Anywhere Library Creator
 * @param {HTMLVideoElement} element
 * @param {string} url
 */
export function anywhere(element, url) {
  console.log('Setting up RhinoAnywhere');
  // TODO: Setup event listeners

  // TODO: Setup connection

  // TODO: TODO create public methods to allow us to send command and mouse movements
  let signalChannel;

  function _setup() {
    signalChannel = new WebSocket(url, []);
    initializeLocalConnection(signalChannel, element); //Need to establish vars for data input and output
  }

  function sendCommand() {}
}
