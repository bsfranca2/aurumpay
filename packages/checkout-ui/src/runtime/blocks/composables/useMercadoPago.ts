import type {
  BinChangeEvent,
  ErrorEvent,
  IdentificationType,
  InstallmentOption,
  Issuer,
  MercadoPagoField,
  MercadoPagoInstance,
  MercadoPagoOptions,
  PaymentMethod,
  ValidityChangeEvent,
} from '../../types/mercadopago'
import { useI18n } from '#imports'
import { computed, reactive, readonly, ref, shallowRef } from 'vue'

export interface MercadoPagoConfig extends MercadoPagoOptions {
  publicKey: string
}

export interface MercadoPagoFieldValidation {
  isValid: boolean
  errors: string[]
  touched: boolean
  hasBeenValidated: boolean
}

export interface MercadoPagoValidationState {
  cardNumber: MercadoPagoFieldValidation
  expirationDate: MercadoPagoFieldValidation
  securityCode: MercadoPagoFieldValidation
}

export interface MercadoPagoTokenData {
  cardholderName: string
  identificationType: string
  identificationNumber: string
}

export function useMercadoPago(config: MercadoPagoConfig) {
  const { t } = useI18n()

  const cardNumberField = ref<MercadoPagoField | null>(null)
  const securityCodeField = ref<MercadoPagoField | null>(null)
  const expirationDateField = ref<MercadoPagoField | null>(null)

  const readyState = reactive({
    cardNumber: false,
    securityCode: false,
    expirationDate: false,
  })

  const focusedState = reactive({
    cardNumber: false,
    securityCode: false,
    expirationDate: false,
  })

  const validationState = reactive<MercadoPagoValidationState>({
    cardNumber: {
      isValid: false,
      errors: [],
      touched: false,
      hasBeenValidated: false,
    },
    expirationDate: {
      isValid: false,
      errors: [],
      touched: false,
      hasBeenValidated: false,
    },
    securityCode: {
      isValid: false,
      errors: [],
      touched: false,
      hasBeenValidated: false,
    },
  })

  const identificationTypes = shallowRef<IdentificationType[]>([])
  const paymentMethods = shallowRef<PaymentMethod[]>([])
  const issuers = shallowRef<Issuer[]>([])
  const installmentOptions = shallowRef<InstallmentOption[]>([])
  const currentPaymentMethod = shallowRef<PaymentMethod | null>(null)

  const isLoadingPaymentMethods = ref(false)
  const isLoadingIssuers = ref(false)
  const isLoadingInstallments = ref(false)

  const mercadoPago = shallowRef<MercadoPagoInstance | null>(null)
  const isInitialized = ref(false)
  const initializationError = ref<string | null>(null)
  const currentBin = ref<string | null>(null)
  const amount = ref(0)
  const selectedIssuer = ref('')

  const errorTranslations: Record<string, string> = {
    'cardNumber should be a number.': t('validation.card.number.invalid_format'),
    'cardNumber should be of length between \'9\' and \'18\'.': t('validation.card.number.invalid_length'),
    'card number rejected on Luhn Validation.': t('validation.card.number.luhn_validation_failed'),
    'securityCode should be a number.': t('validation.card.security_code.invalid_format'),
    'securityCode should be of length \'3\'.': t('validation.card.security_code.invalid_length_3'),
    'securityCode should be of length \'4\'.': t('validation.card.security_code.invalid_length_4'),
    'expirationMonth should be a number.': t('validation.card.expiration.month.invalid_format'),
    'expirationMonth should be a value from 1 to 12.': t('validation.card.expiration.month.invalid_range'),
    'expirationYear should be a number.': t('validation.card.expiration.year.invalid_format'),
    'expirationYear should be of length \'2\' or \'4\'.': t('validation.card.expiration.year.invalid_length'),
    'expirationYear should be greater or equal than': t('validation.card.expiration.card_expired'),
    'expirationMonth value should be greater than': t('validation.card.expiration.card_expired'),
  }

  const isReady = computed(() => readyState.cardNumber && readyState.securityCode && readyState.expirationDate)

  const allFieldsValid = computed(() => {
    return Object.values(validationState).every(field => field.isValid && field.hasBeenValidated)
  })

  const allFieldsTouched = computed(() => {
    return Object.values(validationState).every(field => field.touched)
  })

  const hasErrors = computed(() => {
    return Object.values(validationState).some(field =>
      field.touched && field.errors.length > 0,
    )
  })

  const availableInstallments = computed(() => {
    if (!installmentOptions.value.length)
      return []

    const payerCosts = installmentOptions.value[0]?.payer_costs || []

    return payerCosts.map(cost => ({
      value: cost.installments.toString(),
      label: cost.recommended_message,
      installments: cost.installments,
      installmentAmount: cost.installment_amount,
      totalAmount: cost.total_amount,
      rate: cost.installment_rate,
    }))
  })

  const hasPaymentMethod = computed(() => currentPaymentMethod.value !== null)

  const canCreateToken = computed(() =>
    isReady.value
    && allFieldsValid.value,
  )

  const translateErrorMessages = (errorMessages: Array<{ message: string, cause: string }>): string[] => {
    return errorMessages.map((error) => {
      if (errorTranslations[error.message]) {
        return errorTranslations[error.message]
      }

      for (const [key, translation] of Object.entries(errorTranslations)) {
        if (error.message.includes(key.split(' ')[0])) {
          return translation
        }
      }

      return error.message
    })
  }

  const setFieldError = (fieldName: keyof MercadoPagoValidationState, error: string) => {
    const field = validationState[fieldName]
    field.errors = [error]
    field.isValid = false
    field.touched = true
    field.hasBeenValidated = true
  }

  const fetchIdentificationTypes = async () => {
    try {
      const types = await mercadoPago.value?.getIdentificationTypes()
      identificationTypes.value = types || []
      return types
    }
    catch (error) {
      console.error('Error fetching identification types:', error)
      identificationTypes.value = []
      throw error
    }
  }

  const fetchPaymentMethods = async (bin: string) => {
    try {
      isLoadingPaymentMethods.value = true
      const methods = await mercadoPago.value?.getPaymentMethods({ bin })
      paymentMethods.value = methods?.results || []
      return methods
    }
    catch (error) {
      console.error('Error fetching payment methods:', error)
      paymentMethods.value = []
      throw error
    }
    finally {
      isLoadingPaymentMethods.value = false
    }
  }

  const fetchIssuers = async (paymentMethodId: string, bin?: string) => {
    try {
      isLoadingIssuers.value = true

      const params: any = {
        paymentMethodId,
      }

      if (bin && bin.length >= 6) {
        params.bin = bin
      }

      const issuersData = await mercadoPago.value?.getIssuers(params)
      issuers.value = issuersData || []
      return issuersData
    }
    catch (error) {
      console.error('Error fetching issuers:', error)
      issuers.value = []
      throw error
    }
    finally {
      isLoadingIssuers.value = false
    }
  }

  const fetchInstallments = async (params: {
    amount: number
    paymentTypeId: string
    bin: string
  }) => {
    try {
      isLoadingInstallments.value = true

      const installmentParams: any = {
        amount: params.amount.toString(),
        paymentTypeId: params.paymentTypeId,
      }

      if (params.bin && params.bin.length >= 6) {
        installmentParams.bin = params.bin
      }

      const installmentsData = await mercadoPago.value?.getInstallments(installmentParams)
      installmentOptions.value = installmentsData || []
      return installmentsData
    }
    catch (error) {
      console.error('Error fetching installments:', error)
      installmentOptions.value = []
      throw error
    }
    finally {
      isLoadingInstallments.value = false
    }
  }

  const updatePaymentMethodFromBin = async (bin: string | null) => {
    if (!bin) {
      currentPaymentMethod.value = null
      issuers.value = []
      installmentOptions.value = []
      return
    }

    await fetchPaymentMethods(bin)

    const method = paymentMethods.value.find(method =>
      method.payment_type_id === 'credit_card',
    )

    if (method && method.id !== currentPaymentMethod.value?.id) {
      currentPaymentMethod.value = method

      // TODO: For debit cards we might need to handle differently
      if (method && bin) {
        const params = {
          amount: amount.value,
          paymentTypeId: method.payment_type_id,
          bin,
        }
        fetchInstallments(params).catch(console.warn)
      }
    }
  }

  const createCardToken = async (tokenData: MercadoPagoTokenData) => {
    try {
      if (!allFieldsValid.value) {
        const invalidFields = Object.entries(validationState)
          .filter(([_, field]) => !field.isValid)
          .map(([name]) => name)

        return {
          success: false,
          error: `Campos inválidos: ${invalidFields.join(', ')}`,
        }
      }

      const cleanedTokenData = {
        ...tokenData,
        identificationNumber: tokenData.identificationNumber.replace(/\D/g, ''),
      }

      const token = await mercadoPago.value?.fields.createCardToken(cleanedTokenData)

      if (!token || !token.id) {
        return {
          success: false,
          error: 'Falha ao criar token do cartão',
        }
      }

      return {
        success: true,
        token,
      }
    }
    catch (error: any) {
      console.error('Error creating card token:', error)

      if (error.message?.includes('card_number')) {
        setFieldError('cardNumber', 'Número do cartão inválido')
      }
      else if (error.message?.includes('security_code')) {
        setFieldError('securityCode', 'Código de segurança inválido')
      }
      else if (error.message?.includes('expiration')) {
        setFieldError('expirationDate', 'Data de expiração inválida')
      }

      return {
        success: false,
        error: 'Erro ao processar dados do cartão',
      }
    }
  }

  const setupFieldListeners = (
    field: MercadoPagoField,
    fieldName: keyof MercadoPagoValidationState,
  ) => {
    const fieldState = validationState[fieldName]

    field.on('focus', () => {
      focusedState[fieldName] = true
    })

    field.on('blur', () => {
      focusedState[fieldName] = false
      fieldState.touched = true
    })

    field.on('validityChange', (event: ValidityChangeEvent) => {
      fieldState.isValid = event.errorMessages.length === 0
      fieldState.errors = translateErrorMessages(event.errorMessages)
      fieldState.hasBeenValidated = true

      if (fieldState.touched && event.errorMessages.length === 0) {
        fieldState.errors = []
      }
    })

    field.on('ready', () => {
      readyState[fieldName] = true
    })

    field.on('error', (event: ErrorEvent) => {
      console.error(`MercadoPago field error (${fieldName}):`, event.error)
      setFieldError(fieldName, `Erro no campo: ${event.error}`)
    })

    if (fieldName === 'cardNumber') {
      field.on('binChange', (event: BinChangeEvent) => {
        currentBin.value = event.bin
        updatePaymentMethodFromBin(event.bin)
      })
    }
  }

  const registerField = (
    fieldType: 'cardNumber' | 'securityCode' | 'expirationDate',
    field: MercadoPagoField,
  ) => {
    switch (fieldType) {
      case 'cardNumber':
        cardNumberField.value = field
        setupFieldListeners(field, 'cardNumber')
        break
      case 'securityCode':
        securityCodeField.value = field
        setupFieldListeners(field, 'securityCode')
        break
      case 'expirationDate':
        expirationDateField.value = field
        setupFieldListeners(field, 'expirationDate')
        break
    }
  }

  const initialize = async () => {
    try {
      if (!window.MercadoPago) {
        throw new Error('MercadoPago SDK not loaded')
      }

      const mp = new window.MercadoPago(config.publicKey, {
        locale: config.locale || 'pt-BR',
        trackingDisabled: config.trackingDisabled ?? true,
        advancedFraudPrevention: config.advancedFraudPrevention ?? true,
      })

      mercadoPago.value = mp

      const cardNumberElement = mp.fields.create('cardNumber', {
        placeholder: '1234 1234 1234 1234',
      })
      const expirationDateElement = mp.fields.create('expirationDate', {
        placeholder: 'MM/AA',
        mode: 'short',
      })
      const securityCodeElement = mp.fields.create('securityCode', {
        placeholder: '',
      })

      registerField('cardNumber', cardNumberElement)
      registerField('expirationDate', expirationDateElement)
      registerField('securityCode', securityCodeElement)

      cardNumberElement.mount('form-checkout__cardNumber')
      expirationDateElement.mount('form-checkout__expirationDate')
      securityCodeElement.mount('form-checkout__securityCode')

      await fetchIdentificationTypes()

      isInitialized.value = true
      initializationError.value = null
    }
    catch (error: any) {
      console.error('Error initializing MercadoPago:', error)
      initializationError.value = error.message
      throw error
    }
  }

  const setAmount = (newAmount: number) => {
    amount.value = newAmount
  }

  const setIssuer = (issuerId: string) => {
    selectedIssuer.value = issuerId
  }

  return {
    focusedState: readonly(focusedState),

    validationState: readonly(validationState),
    allFieldsValid,
    allFieldsTouched,
    hasErrors,

    identificationTypes: readonly(identificationTypes),
    paymentMethods: readonly(paymentMethods),
    issuers: readonly(issuers),
    installmentOptions: readonly(installmentOptions),
    availableInstallments,
    currentPaymentMethod: readonly(currentPaymentMethod),
    hasPaymentMethod,

    isLoadingPaymentMethods: readonly(isLoadingPaymentMethods),
    isLoadingIssuers: readonly(isLoadingIssuers),
    isLoadingInstallments: readonly(isLoadingInstallments),

    isInitialized: readonly(isInitialized),
    isReady: readonly(isReady),
    canCreateToken,
    initializationError: readonly(initializationError),
    currentBin: readonly(currentBin),
    amount: readonly(amount),
    selectedIssuer: readonly(selectedIssuer),

    initialize,
    registerField,
    createCardToken,
    fetchPaymentMethods,
    fetchIssuers,
    fetchInstallments,
    setAmount,
    setIssuer,
  }
}
