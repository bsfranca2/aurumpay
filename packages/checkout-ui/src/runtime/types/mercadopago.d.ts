declare global {
  interface Window {
    MercadoPago: {
      new (publicKey: string, options?: MercadoPagoOptions): MercadoPagoInstance
    }
  }
}

export interface MercadoPagoInstance {
  fields: {
    create: (type: FieldType, options?: FieldOptions) => MercadoPagoField
    createCardToken: (data: TokenCreationData) => Promise<CardTokenResponse | void>
    // updateCardToken: (token: string) => Promise<CardTokenResponse | void>
    focus: () => void
    blur: () => void
  }

  getIdentificationTypes: () => Promise<IdentificationType[]>
  getPaymentMethods: (options: { bin: string }) => Promise<ApiPagedResponse<PaymentMethod>>
  getIssuers: (options: { paymentMethodId: string, bin: string }) => Promise<Issuer[]>
  getInstallments: (options: {
    amount: string
    paymentTypeId: string
    bin: string
  }) => Promise<InstallmentOption[]>

  // cardForm: (config: CardFormConfig) => void
  // checkout: (options: any) => any
  // bricks: (options: any) => any

  // getSDKInstanceId: () => string
}

export interface MercadoPagoOptions {
  locale?: 'es-AR' | 'es-CL' | 'es-CO' | 'es-MX' | 'es-VE' | 'es-UY' | 'es-PE' | 'pt-BR' | 'en-US'
  advancedFraudPrevention?: boolean
  trackingDisabled?: boolean
}

export interface FieldOptions {
  placeholder?: string
  style?: FieldStyle
  customFonts?: CustomFont[]
  mode?: 'short' | 'full'
  enableLuhnValidation?: boolean
  srLabel?: string
  ariaRequired?: boolean
}

export type FieldType = 'cardNumber' | 'securityCode' | 'expirationMonth' | 'expirationYear' | 'expirationDate'

export interface TokenCreationData {
  cardholderName: string
  identificationType: string
  identificationNumber: string
}

export interface CardTokenResponse {
  id: string
  public_key: string
  card_id?: unknown
  luhn_validation: boolean
  status: string
  date_used?: unknown
  card_number_length: number
  date_created: string
  first_six_digits: string
  last_four_digits: string
  security_code_length: number
  expiration_month: number
  expiration_year: number
  date_last_updated: string
  date_due: string
  live_mode: boolean
  cardholder: Cardholder
}

export interface Cardholder {
  name: string
  identification: Identification
}

export interface MercadoPagoField {
  mount: (containerId: string) => MercadoPagoField
  unmount: () => void
  on: <T extends keyof MercadoPagoEventMap>(
    event: T,
    callback: (event: MercadoPagoEventMap[T]) => void
  ) => void
  update: (properties: FieldUpdateProperties) => MercadoPagoField
}

export interface FieldUpdateProperties {
  placeholder?: string
  settings?: SecurityCodeSettings | CardNumberSettings
  invalid?: boolean
}

export interface SecurityCodeSettings {
  mode: 'mandatory' | 'optional'
  length: 3 | 4
}

export interface CardNumberSettings {
  length: number // Between 8 and 19
  validation: 'standard' | 'none'
}

interface MercadoPagoEventMap {
  blur: DefaultEvent
  focus: DefaultEvent
  ready: DefaultEvent
  change: DefaultEvent
  paste: DefaultEvent
  validityChange: ValidityChangeEvent
  error: ErrorEvent
  binChange: BinChangeEvent
}

interface DefaultEvent {
  field: string
}

interface ValidityChangeEvent {
  field: string
  errorMessages: ErrorMessage[]
}

interface ErrorMessage {
  message: string
  cause: string
}

interface ErrorEvent {
  field: string
  error: string
}

interface BinChangeEvent {
  bin: string | null
  field: string
}

type CardNumberCause = 'invalid_type' | 'invalid_length' | 'invalid_value'
type SecurityCodeCause = 'invalid_type' | 'invalid_length'
type ExpirationMonthCause = 'invalid_type' | 'invalid_value'
type ExpirationYearCause = 'invalid_type' | 'invalid_length' | 'invalid_value'
type ExpirationDateCause = ExpirationMonthCause | ExpirationYearCause

type ErrorCause = CardNumberCause | SecurityCodeCause | ExpirationMonthCause | ExpirationYearCause | ExpirationDateCause

export interface FieldStyle {
  'color'?: string
  'font-family'?: string
  'fontFamily'?: string
  'font-size'?: string
  'fontSize'?: string
  'font-style'?: string
  'fontStyle'?: string
  'font-variant'?: string
  'fontVariant'?: string
  'font-weight'?: string
  'fontWeight'?: string
  'height'?: string
  'margin'?: string
  'margin-bottom'?: string
  'marginBottom'?: string
  'margin-left'?: string
  'marginLeft'?: string
  'margin-right'?: string
  'marginRight'?: string
  'margin-top'?: string
  'marginTop'?: string
  'padding'?: string
  'padding-bottom'?: string
  'paddingBottom'?: string
  'padding-left'?: string
  'paddingLeft'?: string
  'padding-right'?: string
  'paddingRight'?: string
  'padding-top'?: string
  'paddingTop'?: string
  'placeholder-color'?: string
  'placeholderColor'?: string
  'text-align'?: string
  'textAlign'?: string
  'width'?: string
}

export interface CustomFont {
  src: string
}

export interface ApiPagedResponse<T> {
  paging: unknown
  results: T[]
}

export interface IdentificationType {
  id: string
  name: string
  type: string
  min_length: number
  max_length: number
}

export interface Issuer {
  id: string
  name: string
  secure_thumbnail: string
  thumbnail: string
  processing_mode: string
  merchant_account_id?: string
  status?: string
}

export interface InstallmentOption {
  payment_method_id: string
  payment_type_id: string
  issuer: {
    id: string
    name: string
    secure_thumbnail: string
    thumbnail: string
  }
  processing_mode: string
  merchant_account_id: string | null
  payer_costs: PayerCost[]
  agreements: Agreement[]
}

export interface PayerCost {
  installments: number
  installment_rate: number
  discount_rate: number
  reimbursement_rate?: number
  labels: string[]
  installment_rate_collector?: string[]
  min_allowed_amount: number
  max_allowed_amount: number
  recommended_message?: string
  installment_amount?: number
  total_amount?: number
  payment_method_option_id?: string
}

export interface PaymentMethod {
  id: string
  name: string
  payment_type_id: string
  status: string
  secure_thumbnail: string
  thumbnail: string
  deferred_capture: string
  settings: PaymentMethodSettings[]
  additional_info_needed: string[]
  min_allowed_amount: number
  max_allowed_amount: number
  accreditation_time: number
  financial_institutions: PaymentMethodFinancialInstitutions[]
  processing_modes: string[]
  payer_costs?: PayerCost[]
}

export interface PaymentMethodSettings {
  bin: PaymentMethodSettingsBin
  card_number: PaymentMethodSettingsCardNumber
  security_code: PaymentMethodSettingsSecurityCode
}

export interface PaymentMethodFinancialInstitutions {
  id: number
  description: string
}

export interface PaymentMethodSettingsBin {
  pattern: string
  exclusion_pattern: string
  installments_pattern: string
}

export interface PaymentMethodSettingsCardNumber {
  length: number
  validation: string
}

export interface PaymentMethodSettingsSecurityCode {
  mode: string
  length: number
  card_location: string
}
