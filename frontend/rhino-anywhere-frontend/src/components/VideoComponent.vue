<script setup>
import { onMounted } from 'vue';

function initialize() {
    var viewport = document.getElementById("rhinoViewport")
    window.anywhere.bind(viewport);
    viewport.onresize += (e) => {
        window.anywhere.sendResize(viewport.clientWidth, viewport.clientHeight);
    }
}

onMounted(() => initialize());
</script>

<template>
    <video controls autoplay="autoplay" id="rhinoViewport" width="640" height="480"></video>
</template>
