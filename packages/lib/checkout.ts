export function validateFullName(value: unknown) {
  if (typeof value !== 'string')
    return false

  const parts = value.split(/\s+/).filter(p => p.length > 0)
  return parts.length >= 2 && parts[0].length >= 2
}

export function validateCPF(cpf: unknown) {
  if (typeof cpf !== 'string')
    return false

  const digits = cpf.split('').map(Number)

  if (digits.every(d => d === digits[0]))
    return false

  let sum = digits
    .slice(0, 9)
    .reduce((acc, digit, index) => acc + digit * (10 - index), 0)
  const firstVerifier = (sum % 11) < 2 ? 0 : 11 - (sum % 11)
  if (firstVerifier !== digits[9])
    return false

  sum = digits
    .slice(0, 10)
    .reduce((acc, digit, index) => acc + digit * (11 - index), 0)
  const secondVerifier = (sum % 11) < 2 ? 0 : 11 - (sum % 11)
  return secondVerifier === digits[10]
}
