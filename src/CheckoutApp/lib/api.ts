import type { FetchOptions } from 'ofetch'
import type { ProblemDetail, Store } from '~/types/api'
import type { Either } from '~/utils/result'

type ApiFn = <Right, Left = ProblemDetail>(url: string, options: FetchOptions) => Promise<Either<Left, Right>>

export function createApiSdkPlugin(api: ApiFn) {
  return {
    // Store
    store: () => api<Store>('/store', { method: 'get' }),
    // Auth
    // signIn: (data: SignInRequest) => api<SignInResponse>('/auth/login', {
    //   method: 'post',
    //   body: data,
    // }),
    // signOut: () => api('/auth/logout', { method: 'post' }),

    // Users
    // me: () => api<MeResponse>('/users/me', { headers: useRequestHeaders(['cookie']) }),
  }
}

export function createApiErrorHandler(api: typeof $fetch) {
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
