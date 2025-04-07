export const useApi: typeof useFetch = (request, opts) => {
  const baseURL = useApiUrl()

  return useFetch(request, { baseURL, ...opts })
}
