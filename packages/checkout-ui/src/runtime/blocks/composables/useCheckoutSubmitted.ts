import type { CheckoutStep } from '#checkout-blocks/constants'
import { CustomerStep, PaymentStep, ShippingStep } from '#checkout-blocks/constants'
import { useState } from '#imports'

export function useCheckoutSubmitted() {
  const submitted = useState('checkout:submitted', () => ({
    [CustomerStep]: false,
    [ShippingStep]: false,
    [PaymentStep]: false,
  }))

  function submit(step: CheckoutStep) {
    submitted.value[step] = true
  }

  function isSubmitted(step: CheckoutStep) {
    return submitted.value[step]
  }

  return {
    submit,
    isSubmitted,
  }
}
