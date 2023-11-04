import { initializeLocalConnection } from './initializers/initializeLocalConnection';
import { setupEvents } from './inputHandlers';

export class RhinoAnywhere {
  _videoElement = null;

  /**
   * Bind to a video element
   * @param {*} videoElement 
   */
  bind(videoElement){
    this._videoElement = videoElement;
  }

  /**
   * Connect to rhino
   * @param {*} url URl to connect to
   */
  connect(url){
    console.log('Setting up RhinoAnywhere');

    setupEvents(this._videoElement, (data) => {
      sendData('input', data);
    });

    // TODO: Setup connection

    // TODO: TODO create public methods to allow us to send command and mouse movements
    let signalChannel;
    let localConnection;

    signalChannel = new WebSocket(url, []);
    localConnection = initializeLocalConnection(
      signalChannel,
      this._videoElement,
      this._videoElement
    ); //Need to establish vars for data input and output
  }

  /**
   * Execute a command
   * @param {*} string 
   */
  sendCommand(string) {
    _sendData('command', {
      command: string
    });
  }

  /**
   * Send data to connection
   * @param {string} type "input" or "command"
   * @param {Object} data
   */
  _sendData(type, data) {
    var toSend = {
      type: type,
      data: data
    };

    // Send over communication channel here
    console.log(toSend);
  }
}
