export default defineNuxtPlugin({
  name: 'domain-plugin',
  enforce: 'pre',
  async setup() {
    const headers = useRequestHeaders(['host'])
    const domain = document ? `https://${window.location.host}` : `https://${headers.host}`
    useDomain().value = domain
    useApiUrl().value = `${domain}/api`
  },
})
