import { callWithNuxt } from '#app'

async function fetchAndSet() {
  const nuxt = useNuxtApp()
  const data = await nuxt.$apiSdk.store()
  if (data.isLeft()) {
    throw new Error('Failed to fetch store data')
  }
  const storeState = await callWithNuxt(nuxt, useStoreState)
  storeState.value = data.value
  return data
}

export function useStore() {
  const storeState = useStoreState()

  return {
    store: storeState,
    fetchAndSet,
  }
}
