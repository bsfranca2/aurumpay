import type { CustomerAddress, CustomerInfo } from '../../types'
import { useState } from '#imports'

interface CheckoutState {
  customerInfo: CustomerInfo
  addresses: CustomerAddress[]
}

export function useCheckoutState() {
  return useState<CheckoutState>('checkout:data', () => ({
    customerInfo: {
      email: '',
      fullName: '',
      cpf: '',
      phoneNumber: '',
    },
    addresses: [],
  }))
}
