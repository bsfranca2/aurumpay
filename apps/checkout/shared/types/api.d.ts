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

export interface CheckoutContext {
  bearerToken: string
  cartId: string
}
