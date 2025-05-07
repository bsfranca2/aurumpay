import type { CustomerInfo } from '../../types'
import { CustomerStep } from '../constants'
import { useCheckoutState } from './useCheckoutState'
import { useCheckoutStepper } from './useCheckoutStepper'
import { useCheckoutSubmitted } from './useCheckoutSubmitted'

export function useCheckout() {
  const checkout = useCheckoutState()
  const form = useCheckoutSubmitted()
  const stepper = useCheckoutStepper()

  function submitCustomerInfo(customerInfo: CustomerInfo) {
    checkout.value.customerInfo = customerInfo
    form.submit(CustomerStep)
    stepper.goToNext()
  }

  // function submitShippingAddress() {
  //   form.submit(ShippingStep)
  //   stepper.goToNext()
  // }

  return {
    ...checkout.value,
    submitCustomerInfo,
    // submitShippingAddress,
  }
}
