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

#rhinoViewport {
  width: 100%;
  height: 100%;
}

.container {
  display: flex;
  flex-direction: column;
}

.search-area {
  /* align-self: flex-start; */
  align-items: left;
   /* display: none; */
  position: absolute;
  top: 10px;
  left: 100px;
  /* background-color: red; */
  z-index: 1;

}

.content-area {
  display: flex;
  margin-top: 0px;
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
  margin-top: 10px;
  margin-bottom: 10px;
  text-align: center;
  max-height: calc(100vh - 280px);

}

@media (max-width: 600px) {
  .content-area {
    flex-direction: column;
  }

}

</style>