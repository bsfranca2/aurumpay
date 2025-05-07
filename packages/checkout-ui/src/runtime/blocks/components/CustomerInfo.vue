<script setup lang="ts">
import Step from '../components/Step.vue'
import { useCheckoutState } from '../composables/useCheckoutState'
import { useCheckoutStep } from '../composables/useCheckoutStep'
import { CustomerStep } from '../constants'
import CustomerInfoForm from './CustomerInfoForm.vue'

const checkout = useCheckoutState()
const { isActive, isFilled, isAccessible } = useCheckoutStep(CustomerStep)
</script>

<template>
  <Step
    step="1"
    :title="$t('customerInfoTitle')"
    :description="$t('customerInfoDescription')"
    :is-active
    :is-accessible
    :is-filled
  >
    <CustomerInfoForm />

    <template #resume>
      <p class="font-bold">
        {{ checkout.customerInfo.fullName }}
      </p>
      <p>{{ checkout.customerInfo.email }}<br>{{ $t('cpf') }} {{ checkout.customerInfo.cpf }}</p>
    </template>
  </Step>
</template>
