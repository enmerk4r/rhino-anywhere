<script setup>
import SearchComponent from './components/SearchComponent.vue'
import VideoComponent from './components/VideoComponent.vue';
</script>

<template>
  <div>
    <SearchComponent class="search-component" />
    <img src="./assets/img/rhino.png" alt="some nice logo"  style="width: 50px; height: 50px;"/>
  <h3>Rhino anywhere</h3>
  <VideoComponent/>
</div>  
</template>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}

.search-component {
  position: absolute; /* or 'fixed' if you want it to stay at the top even when scrolling */
  top: 0;
  left: 0;
  right: 0;
  z-index: 1000; /* Ensure it's above other content; adjust as necessary */
}
</style>
