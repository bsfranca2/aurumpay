interface StoreState {
  name: string
}

export function useStoreState() {
  return useState<StoreState>('store:data', () => ({
    // id: '',
    // merchantId: '',
    name: 'Clothes Store',
  }))
}
