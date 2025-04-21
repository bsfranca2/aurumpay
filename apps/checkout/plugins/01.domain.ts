import { createApiErrorHandler, createApiSdk } from '~/lib/api'

export default defineNuxtPlugin({
  name: 'domain-plugin',
  enforce: 'pre',
  async setup() {
    const domain = useDomain()
    const apiUrl = useApiUrl()

    const headers = useRequestHeaders(['host'])
    domain.value = document ? `https://${window.location.host}` : `https://${headers.host}`
    apiUrl.value = `${domain.value}/api`

    const api = $fetch.create({
      baseURL: apiUrl.value,
    })
    const apiWithErrorHandler = createApiErrorHandler(api)
    const apiSdk = createApiSdk(apiWithErrorHandler)
    return { provide: { api, apiSdk } }
  },
})
