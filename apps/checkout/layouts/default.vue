<script setup lang="ts">
const store = useStoreState()
</script>

<template>
  <UHeader>
    {{ store.name }}
  </UHeader>

  <main class="content">
    <slot />
  </main>

  <UFooter>
    {{ store.name }}
  </UFooter>
</template>
