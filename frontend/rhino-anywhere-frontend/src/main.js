import { createApp } from 'vue';
import './style.css';
import App from './App.vue';
import { RhinoAnywhere } from './lib/anywhere';

createApp(App).mount('#app');

window.anywhere = new RhinoAnywhere();