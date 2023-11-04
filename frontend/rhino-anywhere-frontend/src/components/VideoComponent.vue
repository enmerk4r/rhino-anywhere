<script setup>
import { anywhere } from "./../lib/anywhere.js";
import { onMounted } from 'vue';

function initialize() {
    anywhere(document.getElementById("rhinoViewport"), document.getElementById("rhinoTextBox"), "ws://127.0.0.1:8081/");
}

//onMounted(() => initialize());
</script>

<template>
    <div id="rhinoTextBox"></div>
    <video controls autoplay="autoplay" id="rhinoViewport" width="640" height="480"></video>
    <button @click="initialize">start</button>
</template>
