import type { CustomerInfo } from '../../types'
import { useState } from '#imports'

interface CheckoutState {
  customerInfo: CustomerInfo
}

export function useCheckoutState() {
  return useState<CheckoutState>('checkout:data', () => ({
    customerInfo: {
      email: '',
      fullName: '',
      cpf: '',
      phoneNumber: '',
    },
  }))
}
