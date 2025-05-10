<script setup lang="ts">
interface Props {
  title: string
  description?: string
}

defineProps<Props>()
</script>

<template>
  <div class="content-action-card">
    <div class="content-action-card__content-area">
      <p class="content-action-card__title">
        {{ title }}
      </p>
      <p v-if="description" class="content-action-card__description">
        {{ description }}
      </p>
    </div>
    <div class="content-action-card__actions-area">
      <slot />
    </div>
  </div>
</template>
