<script setup>
import { onMounted } from 'vue';

function initialize() {
    window.anywhere.bind(document.getElementById("rhinoViewport"));
}

onMounted(() => initialize());
</script>

<template>
    <video controls autoplay="autoplay" id="rhinoViewport" width="640" height="480"></video>
</template>
