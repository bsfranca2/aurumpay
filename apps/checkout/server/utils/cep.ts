import fetchCep from 'cep-promise'

export const cachedCep = defineCachedFunction(async (cep: string) => {
  try {
    return await fetchCep(cep, { providers: ['brasilapi'] })
  }
  catch {
    return null
  }
}, {
  maxAge: 60 * 60 * 24 * 30,
  name: 'cep',
  getKey: (cep: string) => cep,
})
