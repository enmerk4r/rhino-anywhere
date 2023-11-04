import { createApp } from 'vue';
import './style.css';
import App from './App.vue';

createApp(App).mount('#app');

import { anywhere } from './lib/anywhere.js';
anywhere(
  document.getElementById('rhinoViewport'),
  document.getElementById('rhinoTextBox'),
  'wss://test.antmedia.io:5443/LiveApp/websocket'
);
