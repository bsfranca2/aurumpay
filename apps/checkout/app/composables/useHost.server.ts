export function useHost() {
  const headers = useRequestHeaders(['x-forwarded-host', 'host'])

  // TODO: Check order in prod
  const host = headers.host || headers['x-forwarded-host'] || ''

  const protocol = useRequestHeaders(['x-forwarded-proto'])['x-forwarded-proto'] || 'https'

  const url = `${protocol}://${host}`

  return {
    host,
    url,
  }
}
