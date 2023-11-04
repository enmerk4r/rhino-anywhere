<script setup>
import { ref, computed, onMounted, onUnmounted } from "vue";

import { RhinoCommands } from '../assets/data/rhinoCommands.js'
let search = ref("")
let searchHistory = ref([])


let commands = ref(RhinoCommands);

const filteredSuggestions = computed(() => {
  if (!search.value) {
    return [];
  }
  return commands.value.filter((cmd) =>
    cmd.toLowerCase().startsWith(search.value.toLowerCase())
  );
});

const addSearchTerm = () => {
  if (search.value && !searchHistory.value.includes(search.value)) {
    searchHistory.value.push(search.value)

    window.anywhere.sendCommand(search.value);
    search.value = "" // Clear the input after sending the command
  }
}

window.anywhere.onMessageReceived = (data) => {
  searchHistory.value.push(data);
}

const handleTab = (event) => {
  if (event.key === "Tab" && filteredSuggestions.value.length > 0) {
    event.preventDefault();
    search.value = filteredSuggestions.value[0];
    console.log(filteredSuggestions.value[0])
    //addSearchTerm(); 
  }
};

const handleEnter = (event) => {
  if (event.key === "Enter" && filteredSuggestions.value.length > 0) {
    event.preventDefault(); // Prevent the default enter behavior
    search.value = filteredSuggestions.value[0]; // Select the first suggestion
    console.log(filteredSuggestions.value[0])
    addSearchTerm(); // Send the selected suggestion as a search term
  }
};

onMounted(() => {
  window.addEventListener("keydown", handleTab);
});

onUnmounted(() => {
  window.removeEventListener("keydown", handleTab);
});
</script>

<template>
  <div class="search-container">
    <!-- Display the search history -->
    <div class="search-history">
      <ul>
        <li v-for="(entry, index) in searchHistory" :key="index">
          {{ entry }}
        </li>
      </ul>
    </div>

    <!-- Search bar -->
    <input type="text" placeholder="Type your Rhino command here..." v-model="search" @keyup.enter="addSearchTerm" />


    <ul v-if="filteredSuggestions.length" class="suggestions">
      <li v-for="(suggestion, index) in filteredSuggestions" :key="index"
        @mousedown.prevent="search.value = suggestion; addSearchTerm()">
        {{ suggestion }}
      </li>
    </ul>

  </div>
</template>

<style scoped>
.search-container {
  display: flex;
  flex-direction: column;
  align-items: left;
  margin-top: 0px;
  position: relative;
  margin-bottom: 50px;
}

.search-history {
  margin-bottom: 10px;
  height: 120px;
  width: 600px;
  overflow-y: auto;
  display: flex;
  flex-direction: column-reverse;
  border: 1px solid #ccc;
  text-align: left;
  border-radius: 20px;
}

input[type="text"] {
  padding: 10px 15px;
  font-size: 16px;
  width: 570px;
  border-radius: 20px;
  border: 1px solid #ccc;
  outline: none;
}

ul.no-bullets {
  list-style-type: none;
  padding: 0;
  margin: 0;
}

ul {
  list-style-type: none;
  padding-left: 10px;
  margin-left: 0;
  line-height: 1;
  color: grey;
}

.suggestions {
  margin-top: 2px;
  border-radius: 20px;
  list-style-type: none;
  padding: 0;
  width: 600px;
  position: absolute;
  left: 0;
  top: 100%;
  background-color: #f8f8f8;
  z-index: 10;
  max-height: 100px;
  overflow-y: auto;
  font-size: 0.8rem;
}

.suggestions li {
  padding: 5px 10px;
  cursor: pointer;
  white-space: nowrap;
}

.suggestions li:hover {
  background-color: #ececec;
}
</style>
