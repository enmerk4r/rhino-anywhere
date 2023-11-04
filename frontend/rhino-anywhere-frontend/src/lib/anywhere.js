import { initializeLocalConnection } from './initializers/initializeLocalConnection';
import { setupEvents } from './inputHandlers';

export class RhinoAnywhere {
  _videoElement = null;
  onMessageReceived = (data) => { console.log("Not subscribed, but you sent " + data)}

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
      this._sendData('input', data);
    });

    // TODO: Setup connection

    // TODO: TODO create public methods to allow us to send command and mouse movements
    let signalChannel;
    let localConnection;
    
    let sendChannel;
    let receiveChannel;

    signalChannel = new WebSocket(url, []);
    localConnection = initializeLocalConnection(
      signalChannel,
      this._videoElement,
      this.onMessageReceived
    ); //Need to establish vars for data input and output
  }

  /**
   * Execute a command
   * @param {*} string 
   */
  sendCommand(string) {
    this._sendData('command', {
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
