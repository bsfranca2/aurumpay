import { callWithNuxt } from '#app'

async function fetchAndSet() {
  const nuxt = useNuxtApp()
  const data = await nuxt.$api('/api/store')
  if (!data) {
    throw new Error('Failed to fetch store data')
  }
  const storeState = await callWithNuxt(nuxt, useStoreState)
  storeState.value = data
  return data
}

export function useStore() {
  const storeState = useStoreState()

  return {
    store: storeState,
    fetchAndSet,
  }
}
