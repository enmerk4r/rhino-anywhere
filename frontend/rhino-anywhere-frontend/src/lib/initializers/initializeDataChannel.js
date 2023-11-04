export const initializeDataChannel = (localConnection, dataElement) => {
  let dataChannel = localConnection.createDataChannel();
  dataChannel.addEventListener('open', (event) => {
    beginTransmission(dataChannel);
  });
};
