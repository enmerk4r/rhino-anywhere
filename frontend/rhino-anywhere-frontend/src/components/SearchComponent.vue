<script setup>
import { ref } from "vue"
let search = ref("")
let searchHistory = ref([])

let commands = ["aa", "bb", "cc"]
let filteredCommands = ref([])
let highlightedIndex = ref(0)

// Method to add the search term to the history
const addSearchTerm = () => {
  // Avoid adding empty strings or duplicates
  if (search.value && !searchHistory.value.includes(search.value)) {
    searchHistory.value.push(search.value)
    search.value = "" // Clear the input after adding to history
  }
}

const filterCommands = () => {
  if (!search) {
    filteredCommands = []
    return
  }
  filteredCommands = commands.filter((cmd) =>
    cmd.toLowerCase().includes(search)
  )
}

const highlightNext = () => {
  if (highlightedIndex < filteredCommands.length - 1) {
    highlightedIndex++
  }
}

const highlightPrevious = () => {
  if (highlightedIndex > 0) {
    highlightedIndex--
  }
}

const selectCommand = (command) => {
  search = command
  filteredCommands = []
}
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
    <input
      type="text"
      placeholder="Type your Rhino command here..."
      v-model="search"
      @keyup.enter="addSearchTerm"
    />

    <!-- <div class="autocomplete">
      <input
        type="text"
        v-model="search"
        @input="filterCommands"
        @keydown.down.prevent="highlightNext"
        @keydown.up.prevent="highlightPrevious"
        @keydown.enter.prevent="selectCommand"
      />
      <ul v-if="filteredCommands.length">
        <li
          v-for="(command, index) in filteredCommands"
          :key="command"
          :class="{ highlight: index === highlightedIndex }"
          @mousedown="selectCommand(command)"
        >
          {{ command }}
        </li>
      </ul>
    </div> -->

  </div>
</template>

<style scoped>
.search-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-top: 20px;
}

.search-history {
  margin-bottom: 10px;
  height: 150px;
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
  width: 600px;
  max-width: 400px; /* Adjust the width as needed */
  border-radius: 20px;
  border: 1px solid #ccc;
  outline: none;
  width: 500px;
}

ul.no-bullets {
  list-style-type: none; /* Remove bullets */
  padding: 0; /* Remove padding */
  margin: 0; /* Remove margins */
}

ul {
  list-style-type: none;
  padding-left: 10px;
  margin-left: 0;
  line-height: 1;
  color: grey;
}
</style>
