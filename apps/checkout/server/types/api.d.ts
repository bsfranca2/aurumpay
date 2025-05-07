export interface ProblemDetails {
  type: string
  title: string
  status: number
  detail: string
  instance: string
  validationErrors?: Record<string, string[]>
}

export interface Store {
  id: string
  merchantId: string
  name: string
}

export interface CreateCheckout {
  cartItems: Record<string, number>
}

export interface IdentifyCustomer {
  fullName: string
  cpf: string
  email: string
  phoneNumber: string
}

export interface CheckoutContext {
  bearerToken: string
  cartId: string
}

export interface CartItem {
  productId: number
  quantity: number
}

export interface CheckoutSummary {
  cartItems: CartItem[]
  customer?: IdentifyCustomer
}
