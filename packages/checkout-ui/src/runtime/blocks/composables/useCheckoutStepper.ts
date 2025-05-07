import { useState } from '#imports'
import { computed } from 'vue'
import { CustomerStep, PaymentStep, ShippingStep } from '../constants'

export function useCheckoutStepper() {
  const stepNames = [CustomerStep, ShippingStep, PaymentStep]
  const index = useState('checkout:step', () => 0)
  const current = computed(() => at(index.value))
  const isLast = computed(() => index.value === stepNames.length - 1)

  function at(index: number) {
    return stepNames[index]
  }

  function goToNext() {
    if (isLast.value)
      return

    index.value++
  }

  function isCurrent(step: string) {
    return stepNames.indexOf(step) === index.value
  }

  return {
    stepNames,
    index,
    current,
    goToNext,
    isCurrent,
  }
}
