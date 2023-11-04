<template>
  <div class="welcome-page">
    <img :src="currentImage" class="background-image" />
    <div class="content">
      <button @click="startSomething">Start something...</button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue';

const images = [
  '../assets/icons/Rhinoceros_1.png', // Replace with your image paths
  '../assets/icons/Rhinoceros_1.png'
];
const currentImageIndex = ref(0);

const currentImage = computed(() => images[currentImageIndex.value]);

const rotateImages = () => {
  setInterval(() => {
    currentImageIndex.value = (currentImageIndex.value + 1) % images.length;
  }, 500); // Rotate images every half a second (500 milliseconds)
};

const startSomething = () => {
  // Implement your logic to start something here
};

onMounted(() => {
  rotateImages();
});

onBeforeUnmount(() => {
  clearInterval(timer); // Clear the timer when the component is unmounted
});
</script>

<style scoped>
.welcome-page {
  position: relative;
  text-align: center;
}

.background-image {
  width: 100%;
  height: 100vh;
  object-fit: cover;
  position: absolute;
  top: 0;
  left: 0;
  z-index: -1;
}

.content {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}

button {
  font-size: 24px;
  padding: 20px 40px;
  background-color: #007bff;
  color: #fff;
  border: none;
  cursor: pointer;
}

button:hover {
  background-color: #0056b3;
}
</style>
