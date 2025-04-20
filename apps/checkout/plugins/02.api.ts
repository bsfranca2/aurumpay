// import process from 'node:process'
// import { Agent } from 'undici'

// const isDev = process.env.NODE_ENV === 'development'

// https://stackoverflow.com/a/78381490
// const httpsAgent = isDev
//   ? new Agent({ connect: { rejectUnauthorized: false } })
//   : undefined

export default defineNuxtPlugin({
  name: 'api-plugin',
  enforce: 'pre',
  dependsOn: ['domain-plugin'],
  async setup() {
    return {
      provide: {
        api: $fetch.create({
          baseURL: useApiUrl().value,
          // dispatcher: httpsAgent,
        }),
      },
    }
  },
})
