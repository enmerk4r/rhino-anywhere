<script setup>
import { ref } from 'vue';
import Add from './svgs/Icon.vue'


const iconStates = ref([false, false, false, false, false]);

const toggleIcon = (index) => {
  iconStates.value[index] = !iconStates.value[index];
};

function doSomething(){

}
</script>

<template>
  <div class="icon-stack">
     <div 
              @click="doSomething">
            <Add class = "icon" />
          </div>


      <Add class = "icon" 
        :class="{ 'icon-hover': !clicked, 'icon-clicked': clicked }"/>
    
              <Add class = "icon" 
        :class="{ 'icon-hover': !clicked, 'icon-clicked': clicked }"/>

              <Add class = "icon" 
        :class="{ 'icon-hover': !clicked, 'icon-clicked': clicked }"/>
  </div>
</template>

<style>
.icon-stack {
  display: flex;
  flex-direction: column;
  align-items: center;
  width:50px;
  padding-top:10px;
}


.icon-container {
  margin: 2px; /* Adjust the space between icons as needed */
  cursor: pointer;
}

.icon-container:hover .icon-hover {
  fill: black; /* Color on hover */
}

.icon-clicked {
  fill: pink; /* Color on click */
}

.icon {
    width:30px;
    height:30px;
}
</style>