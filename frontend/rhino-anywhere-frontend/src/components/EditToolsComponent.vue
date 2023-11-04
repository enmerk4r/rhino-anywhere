<script setup>
import { ref } from 'vue';
import Add from './svgs/Icon.vue'

// import Draw_01 from './svgs/Draw_01.vue

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
    <button @click="drawLine">

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