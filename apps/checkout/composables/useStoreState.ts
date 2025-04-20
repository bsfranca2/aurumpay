import type { Store } from '~/types/api'

export function useStoreState() {
  return useState<Store>('store:data', () => ({
    id: '',
    merchantId: '',
    name: 'Clothes Store',
  }))
}
