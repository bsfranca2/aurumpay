import type { $Fetch, FetchOptions } from 'ofetch'
import type { CreateCheckout, ProblemDetail, Store } from '~/server/types/api'
import type { Either } from '~/utils/result'
import { left, right } from '~/utils/result'

type ApiFn = <Right, Left = ProblemDetail>(url: string, options: FetchOptions) => Promise<Either<Left, Right>>

export function createApiSdk(api: ApiFn) {
  return {
    api,

    // Store
    store: () => api<Store>('/store', { method: 'get' }),

    // Checkout
    checkout: {
      init: (data: CreateCheckout) => api('/checkout/init/product', { method: 'post', body: data }),
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
