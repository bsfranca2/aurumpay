<script setup lang="ts">
import Button from '#checkout-base/components/Button.vue'
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '#checkout-base/components/form'
import Input from '#checkout-base/components/Input.vue'
import Tooltip from '#checkout-base/components/Tooltip.vue'
import TooltipContent from '#checkout-base/components/TooltipContent.vue'
import TooltipProvider from '#checkout-base/components/TooltipProvider.vue'
import TooltipTrigger from '#checkout-base/components/TooltipTrigger.vue'
import { useCheckout, useI18n, useRuntimeConfig, useScript } from '#imports'
import { toTypedSchema } from '@vee-validate/valibot'
import { Lock } from 'lucide-vue-next'
import { useForm } from 'vee-validate'
import { computed, inject, watch } from 'vue'
import { createPaymentSchema } from '../../schemas'
import { useMercadoPago } from '../composables/useMercadoPago'
import { CREDIT_CARD_FORM_KEY } from '../constants'
import { mapToFormErrors, noop } from '../utils'

const formHandler = inject(CREDIT_CARD_FORM_KEY, noop)

const { t } = useI18n()
const mercadoPago = useMercadoPago({
  publicKey: useRuntimeConfig().public.mercadoPagoPublicKey,
  locale: 'pt-BR',
  trackingDisabled: true,
})
const { total } = useCheckout()

const isFormMounted = computed(() => mercadoPago.isInitialized && mercadoPago.isReady)
const installments = computed(() => {
  if (!mercadoPago.installmentOptions.value || !mercadoPago.installmentOptions.value.length) {
    return []
  }
  return mercadoPago.installmentOptions.value[0].payer_costs.map(payerCost => ({
    label: payerCost.recommended_message,
    value: payerCost.installments.toString(),
  }))
})

const isCardNumberValid = computed(() => {
  const field = mercadoPago.validationState.cardNumber
  return !field.touched || field.errors.length === 0
})

const isExpirationDateValid = computed(() => {
  const field = mercadoPago.validationState.expirationDate
  return !field.touched || field.errors.length === 0
})

const isSecurityCodeValid = computed(() => {
  const field = mercadoPago.validationState.securityCode
  return !field.touched || field.errors.length === 0
})

const { onLoaded } = useScript('https://sdk.mercadopago.com/js/v2')

const paymentSchema = createPaymentSchema(t)

const { values, isSubmitting, setFieldError, handleSubmit } = useForm({
  validationSchema: toTypedSchema(paymentSchema),
  initialValues: {
    cardholderName: '',
    identificationNumber: '',
    identificationType: 'CPF',
    installments: '',
    mpCardNumber: '',
    mpExpirationDate: '',
    mpSecurityCode: '',
  },
})

const onSubmit = handleSubmit(async (values) => {
  const tokenResult = await mercadoPago.createCardToken(values)
  const tokenId = tokenResult?.token?.id

  if (!tokenResult || !tokenResult.success || !tokenId) {
    return
  }

  const response = await formHandler({
    token: tokenId,
    cardholderName: values.cardholderName,
    identificationNumber: values.identificationNumber,
    identificationType: values.identificationType,
    installments: values.installments,
  })
  mapToFormErrors(response, setFieldError)
})

async function initializeMercadoPago() {
  await mercadoPago.initialize()
  mercadoPago.setAmount(total.value)
}

watch(
  () => mercadoPago.validationState.cardNumber,
  (state) => {
    if (state.touched && state.hasBeenValidated) {
      if (state.errors.length > 0) {
        setFieldError('mpCardNumber', state.errors[0])
      }
      else {
        setFieldError('mpCardNumber', [])
      }
    }
  },
  { deep: true },
)

watch(
  () => mercadoPago.validationState.expirationDate,
  (state) => {
    if (state.touched && state.hasBeenValidated) {
      if (state.errors.length > 0) {
        setFieldError('mpExpirationDate', state.errors[0])
      }
      else {
        setFieldError('mpExpirationDate', [])
      }
    }
  },
  { deep: true },
)

watch(
  () => mercadoPago.validationState.securityCode,
  (state) => {
    if (state.touched && state.hasBeenValidated) {
      if (state.errors.length > 0) {
        setFieldError('mpSecurityCode', state.errors[0])
      }
      else {
        setFieldError('mpSecurityCode', [])
      }
    }
  },
  { deep: true },
)

onLoaded(() => {
  initializeMercadoPago()
})
</script>

<template>
  <div v-show="!isFormMounted">
    Carregando métodos de pagamentos...
  </div>
  <form v-show="isFormMounted" class="form" @submit="onSubmit">
    <FormField name="mpCardNumber">
      <FormItem class="field">
        <FormLabel>Número do cartão</FormLabel>
        <FormControl>
          <div
            id="form-checkout__cardNumber"
            class="field-container"
            :class="{
              'field-container--focused': mercadoPago.focusedState.cardNumber,
              'field-container--error': !isCardNumberValid,
            }"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField name="mpExpirationDate">
      <FormItem class="field">
        <FormLabel>Validade (mês/ano)</FormLabel>
        <FormControl>
          <div
            id="form-checkout__expirationDate"
            class="field-container"
            :class="{
              'field-container--focused': mercadoPago.focusedState.expirationDate,
              'field-container--error': !isExpirationDateValid,
            }"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField name="mpSecurityCode">
      <FormItem class="field">
        <FormLabel>
          Cód. de segurança
          <TooltipProvider :delay-duration="100">
            <Tooltip>
              <TooltipTrigger
                type="button"
                aria-label="Informações sobre o código de segurança"
              >
                (?)
              </TooltipTrigger>
              <TooltipContent>
                <p>
                  Os 3 ou 4 dígitos no verso do seu cartão (ou 4 na frente para Amex).
                </p>
              </TooltipContent>
            </Tooltip>
          </TooltipProvider>
        </FormLabel>
        <FormControl>
          <div
            id="form-checkout__securityCode"
            class="field-container"
            :class="{
              'field-container--focused': mercadoPago.focusedState.securityCode,
              'field-container--error': !isSecurityCodeValid,
            }"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="cardholderName">
      <FormItem>
        <FormLabel>Nome e sobrenome do titular</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="ex.: Ana Luiza Moreira"
            v-bind="componentField"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <div class="field-row">
      <div class="hidden">
        <FormField v-slot="{ componentField }" name="identificationType">
          <FormItem>
            <FormLabel>Tipo de documento</FormLabel>
            <FormControl>
              <select v-bind="componentField" class="input">
                <option value="CPF">
                  CPF
                </option>
                <option value="CNPJ">
                  CNPJ
                </option>
              </select>
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
      </div>

      <FormField v-slot="{ componentField }" name="identificationNumber">
        <FormItem>
          <FormLabel>{{ values.identificationType || 'CPF' }} do titular</FormLabel>
          <FormControl>
            <Input
              v-mask="values.identificationType === 'CNPJ' ? '##.###.###/####-##' : '###.###.###-##'"
              type="text"
              :placeholder="values.identificationType === 'CNPJ' ? '00.000.000/0000-00' : '000.000.000-00'"
              v-bind="componentField"
            />
          </FormControl>
          <FormMessage />
        </FormItem>
      </FormField>
    </div>

    <FormField v-slot="{ componentField }" name="installments">
      <FormItem>
        <FormLabel>Nº de Parcelas</FormLabel>
        <FormControl>
          <select v-bind="componentField" class="input">
            <option value="" disabled selected>
              Selecione...
            </option>
            <option v-for="installment in installments" :key="installment.value" :value="installment.value">
              {{ installment.label }}
            </option>
          </select>
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <Button
      id="form-checkout__submit"
      type="submit"
      :loading="isSubmitting"
      block
    >
      Pagar
    </Button>

    <p class="poweredbymercadopago">
      Pagamento seguro via Mercado Pago
      <Lock />
    </p>
  </form>
</template>
