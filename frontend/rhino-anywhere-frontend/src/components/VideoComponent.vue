<script setup>
import { onMounted } from 'vue';

function initialize() {
    var viewport = document.getElementById("rhinoViewport")
    window.anywhere.bind(viewport);

  window.onresize = _ =>  window.anywhere.sendResize(viewport.clientWidth, viewport.clientHeight);
}

onMounted(() => initialize());
</script>

<template>
    <video autoplay="autoplay" id="rhinoViewport"></video>
</template>
