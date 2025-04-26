export default defineNuxtPlugin({
  name: 'domain-plugin',
  enforce: 'pre',
  async setup() {
    const api = $fetch.create({
      headers: useRequestHeaders(),
    })
    return { provide: { api } }
  },
})
