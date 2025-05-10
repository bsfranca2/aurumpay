<script setup lang="ts">
import type { RadioGroupItemProps } from 'reka-ui'
import type { HTMLAttributes } from 'vue'
import { clsx } from 'clsx'
import {
  RadioGroupIndicator,
  RadioGroupItem,
  useForwardProps,
} from 'reka-ui'
import { computed } from 'vue'
import RadioCardFormLabel from './RadioCardFormLabel.vue'

const props = defineProps<RadioGroupItemProps & { class?: HTMLAttributes['class'] }>()

const delegatedProps = computed(() => {
  const { class: _, ...delegated } = props

  return delegated
})

const forwardedProps = useForwardProps(delegatedProps)
</script>

<template>
  <RadioGroupItem
    data-slot="radio-group-item"
    v-bind="forwardedProps"
    :class="clsx('radio-card-item', props.class)"
  >
    <div class="radio-card-item__indicator">
      <RadioGroupIndicator
        data-slot="radio-group-indicator"
        class="radio-card-item__indicator-dot"
      />
    </div>
    <div class="radio-card-item__content-wrapper">
      <RadioCardFormLabel>
        <slot />
      </RadioCardFormLabel>
    </div>
  </RadioGroupItem>
</template>
