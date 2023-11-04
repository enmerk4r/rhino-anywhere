<script setup>
import SearchComponent from './components/SearchComponent.vue'
import VideoComponent from './components/VideoComponent.vue';
import EditToolsComponent from './components/EditToolsComponent.vue';
</script>

<template>
  <div>
    <SearchComponent class="search-component" />
    <h3>Rhino anywhere</h3>
<div className="container">
  <div className="edit-tools">
    <EditToolsComponent/>
  </div>
  <div className="video-component">
    <VideoComponent/>
  </div>
</div>
  
</div>  
</template>

<style scoped>
.search-component {
  position: absolute; /* or 'fixed' if you want it to stay at the top even when scrolling */
  top: 0;
  left: 0;
  right: 0;
  z-index: 1000; /* Ensure it's above other content; adjust as necessary */
}
</style>
