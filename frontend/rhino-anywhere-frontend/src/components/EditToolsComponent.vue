<script setup>
import { ref } from 'vue';
import Add from './svgs/Icon.vue'

import Draw_01 from './svgs/Draw_01.vue'
import Draw_02 from './svgs/Draw_02.vue'
import Draw_03 from './svgs/Draw_03.vue'

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
      <Draw_01 class="svg-size" :class="{ 'svg-hover': hover, 'svg-clicked': clicked }"></Draw_01>
    </button>

    <button class="transparent-button" @click="drawLine">
      <Draw_02 class="svg-size"
      ></Draw_02>
    </button>

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
  padding: 3px;
  margin-top:5px;
  cursor: pointer;
  border-radius: 18px;
  border-style: solid;
  border-color:transparent;
}

.transparent-button:hover {
  padding: 3px;
  cursor: pointer;
  border-radius: 18px;
  border-color:yellow;
  border-style: solid;
}

.transparent-button svg {
  display: block; 
}

.icon-container {
  margin: 2px;
  cursor: pointer;
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