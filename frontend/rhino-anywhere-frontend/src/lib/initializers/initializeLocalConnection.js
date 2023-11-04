export const initializeLocalConnection = async (
  signalChannel,
  sendChannel,
  receiveChannel,
  videoElement,
  dataElement
) => {
  let localConnection = new RTCPeerConnection();

  const offer = await localConnection.createOffer();
  await localConnection.setLocalDescription(offer);

  localConnection.onicecandidate = (event) => {
    if (event.candidate) {
      signalChannel.send(JSON.stringify(event.candidate));
    }
  };

  localConnection.ontrack = (event) => {
    videoElement.srcObject = event.streams[0];
  };

  sendChannel = 

  signalChannel.onmessage = async (event) => {
    const obj = JSON.parse(event.data);
    if (obj?.candidate) {
      localConnection.addIceCandidate(obj);
    } else if (obj?.sdp) {
      await localConnection.setRemoteDescription(
        new RTCSessionDescription(obj)
      );
      localConnection
        .createAnswer()
        .then((answer) => localConnection.setLocalDescription(answer))
        .then(() => signalChannel.send(JSON.stringify(pc.localDescription)));
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