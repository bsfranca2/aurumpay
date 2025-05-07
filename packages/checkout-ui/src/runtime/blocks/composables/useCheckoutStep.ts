import type { CheckoutStep } from '../constants'
import { computed } from 'vue'
import { useCheckoutSubmitted } from '../composables/useCheckoutSubmitted'
import { useCheckoutStepper } from './useCheckoutStepper'

export function useCheckoutStep(step: CheckoutStep) {
  const stepper = useCheckoutStepper()
  const { isSubmitted } = useCheckoutSubmitted()

  const isActive = computed(() => stepper.isCurrent(step))
  const isFilled = computed(() => isSubmitted(step))
  const isAccessible = computed(() => isActive.value || isFilled.value)

  return {
    isActive,
    isFilled,
    isAccessible,
  }
}
