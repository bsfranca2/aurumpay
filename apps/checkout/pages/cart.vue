<script setup lang="ts">
const store = useStoreState()
</script>

<template>
  <header class="bg-white w-full shadow py-4">
    <div class="max-w-7xl mx-auto px-4">
      <h1 class="text-xl font-semibold text-gray-700">
        {{ store.name }}
      </h1>
    </div>
  </header>

  <div class="checkout-container">
    <div>
      <p class="text-2xl">Seu carrinho esta vázio.</p>
    </div>
  </div>
</template>
