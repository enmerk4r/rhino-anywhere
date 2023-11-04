<script setup>
import { ref } from 'vue';
let searchInput = ref('')
let searchHistory = ref([])

// Method to add the search term to the history
const addSearchTerm = () => {
    // Avoid adding empty strings or duplicates
    if (searchInput.value && !searchHistory.value.includes(searchInput.value)) {
    searchHistory.value.push(searchInput.value);
    searchInput.value = ''; // Clear the input after adding to history
    }
};
</script>

<template>
 <div class="search-container">
    <!-- Display the search history -->
    <div class="search-history">
      <ul>
        <li v-for="(entry, index) in searchHistory" :key="index">{{ entry }}</li>
      </ul>
    </div>

    <!-- Search bar -->
    <input
      type="text"
      placeholder="Type your Rhino command here..."
      v-model="searchInput"
      @keyup.enter="addSearchTerm"
    />
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
}

input[type="text"] {
  padding: 10px;
  font-size: 16px;
  width: 100%;
  max-width: 400px; /* Adjust the width as needed */
}

ul.no-bullets {
  list-style-type: none; /* Remove bullets */
  padding: 0; /* Remove padding */
  margin: 0; /* Remove margins */
}

ul {
  list-style-type: none;
  padding-left: 0;
  margin-left: 0;
}
</style>