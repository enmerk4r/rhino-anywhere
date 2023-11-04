<template>
  <div class="welcome-page">
    <div class="image-container">
      <img :src="currentImage" class="background-image" />
    </div>
    <div class="content">
      <button @click="startSomething">Start something...</button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onBeforeUnmount } from 'vue';

const images = [
  new URL('../assets/icons/Rhinoceros_1.png', import.meta.url).href,
  new URL('../assets/icons/Rhinoceros_2.png', import.meta.url).href
];
const currentImageIndex = ref(0);

const currentImage = computed(() => images[currentImageIndex.value]);

let timer = null;

const rotateImages = () => {
  timer = setInterval(() => {
    currentImageIndex.value = (currentImageIndex.value + 1) % images.length;
  }, 200); 
};

const emit = defineEmits(['start']);

const startSomething = () => {
   emit('start'); 
};

onMounted(() => {
  rotateImages();
});

onBeforeUnmount(() => {
  clearInterval(timer); 
});
</script>

<style scoped>
.welcome-page {
  text-align: center;
  padding-top: 50px; 
}

.image-container {
  display: inline-block;
  width: 500px;
  margin-bottom: 20px; 
}

.background-image {
  width: 100%;
  height: auto; 
}

.content {
  display: flex;
  justify-content: center;
}

button {
  font-size: 24px;
  padding: 20px 40px;
  background-color: #fff; /* Changed to white */
  color: #007bff; /* Text color changed to blue */
  border: 2px solid #007bff; /* Add blue border */
  cursor: pointer;
  border-radius: 25px; /* Makes the button rounded */
  transition: background-color 0.3s, color 0.3s; /* Smooth transition for hover effect */
}

button:hover {
  background-color: #007bff; /* Blue background on hover */
  color: #fff; /* White text on hover */
}

</style>
