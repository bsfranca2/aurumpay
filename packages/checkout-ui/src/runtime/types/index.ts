export type FormHandlerResponse = { validationErrors: Record<string, string[]> } | null | undefined | void

export interface CustomerInfo {
  email: string
  fullName: string
  cpf: string
  phoneNumber: string
}
