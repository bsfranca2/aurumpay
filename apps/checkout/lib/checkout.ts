import { z } from 'zod'

const requiredError = 'Campo obrigatório'
const fullNameError = 'Digite o seu nome completo'
const emailError = 'E-mail inválido. Verifique se digitou corretamente'
const cpfError = 'Digite um CPF válido'
const phoneNumberError = 'Telefone inválido'

export const customerInfoSchema = z.object({
  fullName: z.string({ required_error: requiredError })
    .transform(s => s.trim())
    .refine(s => s.split(/\s+/).filter(p => p.length > 0).length >= 2, { message: fullNameError })
    .refine((s) => {
      const parts = s.split(/\s+/).filter(p => p.length > 0)
      if (parts.length === 2) {
        return parts[0].length >= 2 && parts[1].length >= 2
      }
      if (parts.length >= 3) {
        const [first, second, ...rest] = parts
        return first.length >= 2
          && second.length >= 1
          && rest.every(p => p.length >= 2)
      }
      return false
    }, { message: fullNameError }),

  email: z.string({ required_error: requiredError })
    .email({ message: emailError }),

  cpf: z.string({ required_error: requiredError })
    .transform(s => s.replace(/\D/g, ''))
    .refine(s => s.length === 11, cpfError)
    .refine((s) => {
      const digits = s.split('').map(Number)
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
    }, cpfError),

  phoneNumber: z.string({ required_error: requiredError })
    .transform(s => s.replace(/\D/g, ''))
    .refine(s => [10, 11].includes(s.length), { message: phoneNumberError })
    .refine((s) => {
      if (s.length === 11) {
        return s.startsWith('9', 2)
      }
      return true
    }, phoneNumberError),
})
