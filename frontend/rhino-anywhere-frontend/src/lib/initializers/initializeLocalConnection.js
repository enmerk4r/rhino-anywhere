export const initializeLocalConnection = async (
  signalChannel,
  sendChannel,
  receiveChannel,
  videoElement,
  dataElement
) => {
  console.log('Starting');
  let localConnection = new RTCPeerConnection();
  console.log('Created local connection');

  localConnection.onicecandidate = (event) => {
    console.log('Detected ice candidate event');
    if (event.candidate) {
      signalChannel.send(JSON.stringify(event.candidate));
      console.log('Found candidate');
    }
  };

  localConnection.ontrack = (event) => {
    videoElement.srcObject = event.streams[0];
    console.log('found track');
  };

  sendChannel = 

  signalChannel.onmessage = async (event) => {
    console.log('Received Message');
    const obj = JSON.parse(event.data);
    console.log('Get message');
    if (obj?.candidate) {
      localConnection.addIceCandidate(obj);
    } else if (obj?.sdp) {
      await localConnection.setRemoteDescription(
        new RTCSessionDescription(obj)
      );
      localConnection
        .createAnswer()
        .then((answer) => localConnection.setLocalDescription(answer))
        .then(() =>
          signalChannel.send(JSON.stringify(localConnection.localDescription))
        );
    }
  };

  localConnection.ondatachannel = (e) => receiveChannelCallback(e, receiveChannel);

  return localConnection;
};

const sendChannelCallback = (e, sendChannel) => {
  
}

const receiveChannelCallback = (e, receiveChannel) => {
  receiveChannel = e.channel;
  receiveChannel.onemessage = receiveMessageCallback;
}

const receiveMessageCallback = (event) => {
    
}