<script setup lang="ts">
import type { CustomerAddress } from '../../types'
import { computed } from 'vue'
import ContentActionCard from '../../base/components/ContentActionCard.vue'

interface Props extends CustomerAddress {}

const {
  addressLine1,
  number,
  neighborhood,
  city,
  state,
  cep,
} = defineProps<Props>()

const title = computed(() => {
  return `${addressLine1}, ${number} - ${neighborhood}`
})
const description = computed(() => {
  return `${city}-${state} | CEP ${cep}`
})
</script>

<template>
  <ContentActionCard :title :description />
</template>
