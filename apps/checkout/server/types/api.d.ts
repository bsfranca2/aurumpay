export interface ProblemDetail {
  type: string
  title: string
  status: number
  detail: string
  instance: string
  validationErrors?: Array<{ path: string, message: string }>
}

export interface Store {
  id: string
  merchantId: string
  name: string
}

export interface CreateCheckout {
  cartItems: Record<string, number>
}

export interface CheckoutContext {
  bearerToken: string
  cartId: string
}
