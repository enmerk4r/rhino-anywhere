<script setup>
import { ref } from 'vue';
import SearchComponent from './components/SearchComponent.vue'
import VideoComponent from './components/VideoComponent.vue';
import EditToolsComponent from './components/EditToolsComponent.vue';
import WelcomePageComponent from './components/WelcomePageComponent.vue';

let showWelcomePage = ref(true);

function start(link) {
  showWelcomePage.value = false;
  window.anywhere.connect(link);
}
</script>

<template>
  <WelcomePageComponent v-show="showWelcomePage" @start="start" />
  <div class="container" v-show="!showWelcomePage">
    <div class="search-area">
      <div class="logo-container">
        pretty logo
      </div>
      <SearchComponent />
    </div>

    <div class="content-area">
      <div class="edit-tools">
        <EditToolsComponent />
      </div>
      <div class="video-component">
        <VideoComponent />
      </div>
    </div>
  </div>
</template>

<style scoped>
.container {
  background-color: white;
  display: flex;
  flex-direction: column;
}

.search-area {
  align-self: flex-start;
  align-items: left;
  display: flex;
}

.content-area {
  display: flex;
  margin-top: 20px;
}

.edit-tools {
  width: 20%;
  width: 50px;
  border: 1px solid #ccc;
  border-radius: 30px;
}

.video-component {
  flex-grow: 1;
  margin-left: 20px;
  border: 1px solid #ccc;
  text-align: right;
  /* Aligns the content of the video component to the right */
}

@media (max-width: 600px) {
  .content-area {
    flex-direction: column;
  }

  .video-component {
    margin-left: 0;
    margin-top: 20px;
    /* Adjust the margin as needed */
    text-align: center;
    /* Center the content on small screens */
  }
}
</style>