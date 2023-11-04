export default initializeLocalConnection = (server) => {
  window.localConnection = localConnection = new RTCPeerConnection(server);
};
