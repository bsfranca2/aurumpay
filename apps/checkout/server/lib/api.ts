import type { $Fetch, FetchOptions } from 'ofetch'
import type { Either } from 'result'
import type {
  CheckoutSummary,
  CreateCheckout,
  IdentifyCustomer,
  ProblemDetails,
  Store,
} from '~/server/types/api'
import { left, right } from 'result'

type ApiFn = <Right, Left = ProblemDetails>(url: string, options: FetchOptions) => Promise<Either<Left, Right>>

export function createApiSdk(api: ApiFn) {
  return {
    api,

    // Store
    store: () => api<Store>('/store', { method: 'get' }),

    // Checkout
    checkout: {
      init: (data: CreateCheckout) => api('/checkout/init/product', { method: 'post', body: data }),
      summary: () => api<CheckoutSummary>('/checkout/summary', { method: 'get' }),
      identifyCustomer: (data: IdentifyCustomer) => api('/checkout/customer', { method: 'put', body: data }),
    },
  }
}

export function createApiErrorHandler(api: $Fetch) {
  async function apiWithErrorHandler<Left, Right>(url: string, options: FetchOptions): Promise<Either<Left, Right>> {
    try {
      const response = await api<Right>(url, options as any)
      return right(response)
    }
    catch (err: any) {
      return left(err.data || err.message || err)
    }
  }
  return apiWithErrorHandler
}
