<script setup lang="ts">
import Step from '../components/Step.vue'
import { useCheckoutStep } from '../composables/useCheckoutStep'
import { ShippingStep } from '../constants'

const { isActive, isFilled, isAccessible } = useCheckoutStep(ShippingStep)
</script>

<template>
  <Step
    step="2"
    :title="$t('customerAddressTitle')"
    :description="$t('customerAddressDescription')"
    :is-active
    :is-accessible
    :is-filled
  />
</template>
