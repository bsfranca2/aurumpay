interface CustomerInfo {
  email: string
  fullName: string
  cpf: string
  phoneNumber: string
}

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
