import { createApiErrorHandler, createApiSdkPlugin } from '~/lib/api'

export default defineNuxtPlugin({
  name: 'api-sdk-plugin',
  enforce: 'pre',
  dependsOn: ['api-plugin'],
  setup: (nuxtApp) => {
    const api = nuxtApp.$api as typeof $fetch
    const apiWithErrorHandler = createApiErrorHandler(api)
    return { provide: { apiSdk: createApiSdkPlugin(apiWithErrorHandler) } }
  },
})
