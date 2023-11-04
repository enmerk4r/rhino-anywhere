<script setup>
import { ref } from 'vue';
import Add from './svgs/Icon.vue'

import Draw_01 from './svgs/Draw_01.vue'


// import Test_Icons_01 from '../assets/icons/Test_Icons_01.svg'

const iconStates = ref([false, false, false, false, false]);

const toggleIcon = (index) => {
  iconStates.value[index] = !iconStates.value[index];
};

function doSomething(){

}

function drawLine(){
  console.log("draw line");
}

</script>

<template>
  <div class="icon-stack">

    
    <button class="transparent-button" @click="drawLine">
      <Draw_01 class="svg-size"></Draw_01>

    </button>



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

.transparent-button {
  background-color: transparent;
  border: none;
  padding: 0;
  cursor: pointer;
}

.transparent-button svg {
  display: block; /* This can help with SVG sizing issues */
}

/* You might also need to set the fill or stroke of the SVG paths if they're not set within the SVG itself */
.transparent-button svg path {
  stroke: #000; /* Example: setting a black stroke for visibility */
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

.svg-size {
  width: 30px;
  height: 30px;
}
</style>