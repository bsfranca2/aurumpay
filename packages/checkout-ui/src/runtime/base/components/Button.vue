<script setup lang="ts">
import type { PrimitiveProps } from 'reka-ui'
import type { HTMLAttributes } from 'vue'
import { clsx } from 'clsx'
import { Loader2 } from 'lucide-vue-next'
import { Primitive } from 'reka-ui'
import { computed, withDefaults } from 'vue'

interface Props extends PrimitiveProps {
  variant?: 'primary' | 'secondary'
  block?: boolean
  loading?: boolean
  disabled?: boolean
  class?: HTMLAttributes['class']
}

const props = withDefaults(defineProps<Props>(), {
  variant: 'primary',
  block: false,
  loading: false,
  disabled: false,
  as: 'button',
})

const isDisabled = computed(() => props.loading || props.disabled)
</script>

<template>
  <Primitive
    data-slot="button"
    :as="as"
    :as-child="asChild"
    :class="clsx('button', 'button--primary', 'button--md', block && 'button--block', props.class)"
    :disabled="isDisabled"
  >
    <Loader2 v-if="loading" class="button__spinner" />
    <slot />
  </Primitive>
</template>
