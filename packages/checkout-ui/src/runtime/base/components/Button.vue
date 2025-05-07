<script lang="ts">
import type { PrimitiveProps } from 'reka-ui'
import type { HTMLAttributes } from 'vue'
</script>

<script setup lang="ts">
import { clsx } from 'clsx'
import { Primitive } from 'reka-ui'

interface Props extends PrimitiveProps {
  variant?: 'primary' | 'secondary'
  block?: boolean
  class?: HTMLAttributes['class']
}

const props = withDefaults(defineProps<Props>(), {
  variant: 'primary',
  block: false,
  as: 'button',
})
</script>

<template>
  <Primitive
    data-slot="button"
    :as="as"
    :as-child="asChild"
    :class="clsx('button', 'button--primary', 'button--md', block && 'button--block', props.class)"
  >
    <slot />
  </Primitive>
</template>
