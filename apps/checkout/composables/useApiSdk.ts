import type { ApiSdk } from '~/lib/api'

export function useApiSdk() {
  const { $apiSdk } = useNuxtApp()
  return $apiSdk as ApiSdk
}
