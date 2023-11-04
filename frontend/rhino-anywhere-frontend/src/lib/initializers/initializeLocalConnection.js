export const initializeLocalConnection = async (
  signalChannel,
  videoElement
) => {
  let localConnection = new RTCPeerConnection(configuration);

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

  return localConnection;
};
