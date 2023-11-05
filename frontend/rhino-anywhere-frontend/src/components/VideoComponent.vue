<script setup>
import { onMounted, onUnmounted, ref } from 'vue';
const rhinoViewport = ref(null);

function initialize() {
    var viewport = document.getElementById("rhinoViewport")
    window.anywhere.bind(viewport);

  window.onresize = _ =>  window.anywhere.sendResize(viewport.clientWidth, viewport.clientHeight);
}

onMounted(() => initialize());
</script>

<template>
    <video ref="rhinoViewport" autoplay="autoplay" playsinline>
    </video>
</template>

<style>

video {
  width: 100%;
  height: 100%;
  object-fit: cover;
}</style>
