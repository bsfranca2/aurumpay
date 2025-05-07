<script setup lang="ts">
import Step from '../components/Step.vue'
import { useCheckoutStep } from '../composables/useCheckoutStep'
import { PaymentStep } from '../constants'

const { isActive, isFilled, isAccessible } = useCheckoutStep(PaymentStep)
</script>

<template>
  <Step
    step="3"
    :title="$t('paymentTitle')"
    :description="$t('paymentDescription')"
    :is-active
    :is-accessible
    :is-filled
  />
</template>
