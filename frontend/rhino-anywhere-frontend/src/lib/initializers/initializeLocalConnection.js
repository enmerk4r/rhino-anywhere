export const initializeLocalConnection = async (
  signalChannel,
  videoElement
) => {
  let localConnection = new RTCPeerConnection();
  console.log('Created local connection');

  // const offer = await localConnection.createOffer();
  // console.log('Offer created');
  // await localConnection.setLocalDescription(offer);
  // console.log('Local description set');

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

  signalChannel.onmessage = async (event) => {
    console.log('Received Message');
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
        .then(() =>
          signalChannel.send(JSON.stringify(localConnection.localDescription))
        );
    }
  };

  return localConnection;
};
