export function useHost() {
  const headers = useRequestHeaders(['x-forwarded-host', 'host'])

  const host = headers['x-forwarded-host'] || headers.host || ''

  const protocol = useRequestHeaders(['x-forwarded-proto'])['x-forwarded-proto'] || 'https'

  const url = `${protocol}://${host}`

  return {
    host,
    url,
  }
}
