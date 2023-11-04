<script setup>
import { onMounted } from 'vue';

function initialize() {
    window.anywhere.bind(document.getElementById("rhinoViewport"));
    window.anywhere.connect("ws://127.0.0.1:8081/");
}

//onMounted(() => initialize());
</script>

<template>
    <div id="rhinoTextBox"></div>
    <video controls autoplay="autoplay" id="rhinoViewport" width="640" height="480"></video>
    <button @click="initialize">start</button>
</template>
