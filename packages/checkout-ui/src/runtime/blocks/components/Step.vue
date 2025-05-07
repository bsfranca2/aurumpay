<script setup lang="ts">
import { clsx } from 'clsx'

interface Props {
  step: string | number
  title: string
  description: string
  isActive?: boolean
  isFilled?: boolean
  isAccessible?: boolean
}

const { isActive = false, isFilled = false, isAccessible = false } = defineProps<Props>()

const emit = defineEmits<{
  (e: 'click'): void
}>()

function handleClick() {
  if (!isActive && isAccessible) {
    emit('click')
  }
}
</script>

<template>
  <div :class="clsx('step', isActive && 'step--active', isFilled && 'step--filled', !isAccessible && 'step--blocked')" @click="handleClick">
    <div class="step-header">
      <div class="step-header__track">
        <div class="number">
          {{ step }}
        </div>
      </div>
      <div class="step-header__text">
        <p class="step-header__title">
          {{ title }}
        </p>
        <p class="step-header__description">
          {{ description }}
        </p>
      </div>
    </div>
    <div class="step-content">
      <slot />
    </div>
    <div class="step-resume">
      <slot name="resume" />
    </div>
  </div>
</template>
