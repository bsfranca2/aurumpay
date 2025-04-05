<script setup>
import {
  ArrowLeftIcon,
  ArrowRightIcon,
  BanknotesIcon,
  BuildingLibraryIcon,
  BuildingOfficeIcon,
  CalendarIcon,
  CreditCardIcon,
  DocumentIcon,
  EnvelopeIcon,
  LockClosedIcon,
  MapPinIcon,
  TruckIcon,
  UserIcon,
} from '@heroicons/vue/24/outline'
import { computed, ref } from 'vue'

const currentStep = ref(1)
const steps = ['Informações', 'Endereço', 'Pagamento']

const cartItems = ref([
  { id: 1, name: 'Tênis Esportivo', category: 'Calçados', price: 299.90 },
  { id: 2, name: 'Camiseta Básica', category: 'Roupas', price: 89.90 },
])

const shippingCost = ref(15.90)

// Configuração do Mercado Pago
const loadingMP = ref(true)
const processingPayment = ref(false)
const errorMessage = ref('')
let mp = null

const subtotal = computed(() => {
  return cartItems.value.reduce((sum, item) => sum + item.price, 0)
})

const total = computed(() => {
  return subtotal.value + shippingCost.value
})

function nextStep() {
  if (currentStep.value < steps.length) {
    currentStep.value++
  }
}

function prevStep() {
  if (currentStep.value > 1) {
    currentStep.value--
  }
}

async function submitCheckout(token) {
  try {
    // Simulação de requisição ao backend
    // Substituir por chamada real à sua API
    alert(`Pagamento processado com sucesso! Token: ${token}`)
  }
  catch (error) {
    errorMessage.value = `Erro ao finalizar compra: ${error.message}`
  }
}

async function initializeMercadoPago() {
  try {
    // Carrega o SDK do Mercado Pago
    await loadScript('https://sdk.mercadopago.com/js/v2')

    // Inicializa com sua public key
    mp = new MercadoPago('TEST-7790a154-ba07-44f7-91b4-70c44ae68dae', {
      locale: 'pt-BR',
    })

    // Cria o card form
    const cardForm = mp.cardForm({
      amount: total.value.toString(),
      autoMount: true,
      form: {
        id: 'form-checkout',
        cardNumber: {
          id: 'form-checkout__cardNumber',
          placeholder: 'Número do cartão',
        },
        expirationDate: {
          id: 'form-checkout__cardExpirationDate',
          placeholder: 'MM/AAAA',
        },
        securityCode: {
          id: 'form-checkout__securityCode',
          placeholder: 'Código de segurança',
        },
        cardholderName: {
          id: 'form-checkout__cardholderName',
          placeholder: 'Titular do cartão',
        },
        identificationNumber: {
          id: 'form-checkout__identificationNumber',
          placeholder: 'CPF',
        },
        issuer: {
          id: 'form-checkout__issuer',
          placeholder: 'Banco emissor',
        },
        installments: {
          id: 'form-checkout__installments',
          placeholder: 'Parcelas',
        },
      },
      callbacks: {
        onFormMounted: () => {
          loadingMP.value = false
        },
        onError: (error) => {
          errorMessage.value = error.message
          processingPayment.value = false
        },
        onBinChange: async (bin) => {
          const issuer = await mp.getIssuers({ bin })
          cardForm.updateField('issuer', { options: issuer })
        },
        onIssuerChange: async (issuerId) => {
          const installments = await mp.getInstallments({
            amount: total.value.toString(),
            bin: cardForm.getBin(),
            issuerId,
          })
          cardForm.updateField('installments', { options: installments[0].payer_costs })
        },
      },
    })
  }
  catch (error) {
    console.error(error)
    errorMessage.value = 'Erro ao carregar o sistema de pagamento'
    loadingMP.value = false
  }
}

async function handleSubmit() {
  try {
    processingPayment.value = true
    errorMessage.value = ''

    const result = await mp.createCardToken({
      cardholderName: document.getElementById('form-checkout__cardholderName').value,
      identificationNumber: document.getElementById('form-checkout__identificationNumber').value,
    })

    // Enviar o token para seu backend
    console.log('Token gerado:', result.id)
    await submitCheckout(result.id)
  }
  catch (error) {
    errorMessage.value = error.message || 'Erro ao processar pagamento'
  }
  finally {
    processingPayment.value = false
  }
}

function loadScript(src) {
  return new Promise((resolve, reject) => {
    const script = document.createElement('script')
    script.src = src
    script.onload = () => resolve()
    script.onerror = () => reject()
    document.head.appendChild(script)
  })
}

onMounted(() => {
  initializeMercadoPago()
})
</script>

<template>
  <div class="min-h-screen bg-gray-100 py-8">
    <div class="max-w-7xl mx-auto px-4">
      <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
        <!-- Passos do Checkout -->
        <div class="md:col-span-2 bg-white rounded-lg shadow-md p-6">
          <!-- Progresso -->
          <div class="flex items-center justify-between mb-8">
            <div v-for="(step, index) in steps" :key="index" class="flex items-center">
              <div
                class="w-8 h-8 rounded-full flex items-center justify-center text-sm font-semibold" :class="[
                  currentStep > index + 1 ? 'bg-emerald-500 text-white'
                  : currentStep === index + 1 ? 'bg-emerald-100 text-emerald-600 border-2 border-emerald-500' : 'bg-gray-100 text-gray-500',
                ]"
              >
                {{ index + 1 }}
              </div>
              <div
                v-if="index < steps.length - 1"
                class="flex-1 h-1 mx-2" :class="[
                  currentStep > index + 1 ? 'bg-emerald-500' : 'bg-gray-200',
                ]"
              />
            </div>
          </div>

          <!-- Passo 1 - Informações Pessoais -->
          <div v-show="currentStep === 1">
            <h2 class="text-2xl font-bold mb-6">
              Informações Pessoais
            </h2>
            <form class="space-y-5" @submit.prevent="nextStep">
              <div class="relative">
                <label class="block text-sm font-medium text-gray-700 mb-2">Nome Completo</label>
                <div class="relative">
                  <input
                    type="text"
                    required
                    class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                  >
                  <UserIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                </div>
              </div>

              <div class="relative">
                <label class="block text-sm font-medium text-gray-700 mb-2">E-mail</label>
                <div class="relative">
                  <input
                    type="email"
                    required
                    class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                  >
                  <EnvelopeIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                </div>
              </div>
            </form>
          </div>

          <!-- Passo 2 - Endereço -->
          <div v-show="currentStep === 2">
            <h2 class="text-2xl font-bold mb-6">
              Endereço de Entrega
            </h2>
            <form class="space-y-5" @submit.prevent="nextStep">
              <div class="grid grid-cols-2 gap-5">
                <div class="relative">
                  <label class="block text-sm font-medium text-gray-700 mb-2">CEP</label>
                  <div class="relative">
                    <input
                      type="text"
                      required
                      class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                    >
                    <TruckIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                  </div>
                </div>
                <div class="relative">
                  <label class="block text-sm font-medium text-gray-700 mb-2">Cidade</label>
                  <div class="relative">
                    <input
                      type="text"
                      required
                      class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                    >
                    <BuildingOfficeIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                  </div>
                </div>
              </div>

              <div class="relative">
                <label class="block text-sm font-medium text-gray-700 mb-2">Endereço Completo</label>
                <div class="relative">
                  <input
                    type="text"
                    required
                    class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                  >
                  <MapPinIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                </div>
              </div>
            </form>
          </div>

          <!-- Passo 3 - Pagamento (Atualizado para Mercado Pago) -->
          <div v-show="currentStep === 3">
            <h2 class="text-2xl font-bold mb-6">
              Pagamento
            </h2>
            <div v-if="loadingMP" class="text-center py-8">
              <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-emerald-500 mx-auto" />
              <p class="mt-4 text-gray-600">
                Carregando gateway de pagamento...
              </p>
            </div>

            <div v-else>
              <form id="form-checkout" class="space-y-5" @submit.prevent="handleSubmit">
                <div class="relative">
                  <label class="block text-sm font-medium text-gray-700 mb-2">Número do Cartão</label>
                  <div class="relative">
                    <input
                      id="form-checkout__cardNumber"
                      type="text"
                      class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                      placeholder="4509 9535 6623 3704"
                    >
                    <CreditCardIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                  </div>
                </div>

                <div class="relative">
                  <label class="block text-sm font-medium text-gray-700 mb-2">Banco Emissor</label>
                  <div class="relative">
                    <select
                      id="form-checkout__issuer"
                      class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all appearance-none bg-white"
                    >
                      <option value="">
                        Selecione seu banco
                      </option>
                    </select>
                    <BuildingLibraryIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                  </div>
                </div>

                <div class="relative">
                  <label class="block text-sm font-medium text-gray-700 mb-2">Parcelas</label>
                  <div class="relative">
                    <select
                      id="form-checkout__installments"
                      class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all appearance-none bg-white"
                    >
                      <option value="">
                        Selecione o número de parcelas
                      </option>
                    </select>
                    <BanknotesIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                  </div>
                </div>

                <div class="grid grid-cols-2 gap-5">
                  <div class="relative">
                    <label class="block text-sm font-medium text-gray-700 mb-2">Validade</label>
                    <div class="relative">
                      <input
                        id="form-checkout__cardExpirationDate"
                        type="text"
                        placeholder="MM/AAAA"
                        class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                      >
                      <CalendarIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                    </div>
                  </div>
                  <div class="relative">
                    <label class="block text-sm font-medium text-gray-700 mb-2">CVV</label>
                    <div class="relative">
                      <input
                        id="form-checkout__securityCode"
                        type="text"
                        class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                        placeholder="123"
                      >
                      <LockClosedIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                    </div>
                  </div>
                </div>

                <div class="relative">
                  <label class="block text-sm font-medium text-gray-700 mb-2">Nome do Titular</label>
                  <div class="relative">
                    <input
                      id="form-checkout__cardholderName"
                      type="text"
                      class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                      placeholder="JOÃO S SILVA"
                    >
                    <UserIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                  </div>
                </div>

                <div class="relative">
                  <label class="block text-sm font-medium text-gray-700 mb-2">Documento</label>
                  <div class="relative">
                    <input
                      id="form-checkout__identificationNumber"
                      type="text"
                      class="w-full pl-10 pr-4 py-3 rounded-lg border border-gray-300 focus:border-emerald-500 focus:ring-2 focus:ring-emerald-200 transition-all"
                      placeholder="CPF"
                    >
                    <DocumentIcon class="w-5 h-5 absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" />
                  </div>
                </div>

                <div v-if="errorMessage" class="p-4 bg-red-50 text-red-700 rounded-lg">
                  {{ errorMessage }}
                </div>

                <button
                  type="submit"
                  class="w-full px-6 py-2.5 bg-emerald-500 text-white rounded-lg hover:bg-emerald-600 transition-colors"
                  :disabled="processingPayment"
                >
                  <span v-if="!processingPayment">Pagar R$ {{ total.toFixed(2) }}</span>
                  <span v-else>Processando pagamento...</span>
                </button>
              </form>

              <p class="mt-4 text-sm text-gray-500">
                Pagamento seguro via Mercado Pago
                <LockClosedIcon class="w-4 h-4 inline-block ml-1 text-emerald-500" />
              </p>
            </div>
          </div>

          <!-- Navegação -->
          <div class="mt-8 flex justify-between border-t pt-6">
            <button
              v-if="currentStep > 1"
              class="px-5 py-2.5 text-gray-600 hover:text-emerald-600 font-medium flex items-center gap-2"
              @click="prevStep"
            >
              <ArrowLeftIcon class="w-4 h-4" />
              Voltar
            </button>
            <div v-else />

            <button
              v-if="currentStep < steps.length"
              class="px-6 py-2.5 bg-emerald-500 text-white rounded-lg hover:bg-emerald-600 transition-colors"
              @click="nextStep"
            >
              Próximo
              <ArrowRightIcon class="w-4 h-4 inline-block ml-2" />
            </button>
            <button
              v-else
              class="px-6 py-2.5 bg-emerald-500 text-white rounded-lg hover:bg-emerald-600 transition-colors"
              @click="submitCheckout"
            >
              Finalizar Compra
            </button>
          </div>
        </div>

        <!-- Resumo da Compra -->
        <div class="bg-white rounded-lg shadow-md p-6 h-fit sticky top-8">
          <h3 class="text-xl font-bold mb-4">
            Resumo da Compra
          </h3>
          <div class="space-y-4 mb-6">
            <div v-for="item in cartItems" :key="item.id" class="flex justify-between items-center">
              <div class="flex items-center gap-3">
                <div class="w-12 h-12 bg-gray-100 rounded-lg flex items-center justify-center">
                  <span class="text-sm text-gray-500">1x</span>
                </div>
                <div>
                  <p class="font-medium">
                    {{ item.name }}
                  </p>
                  <p class="text-sm text-gray-500">
                    {{ item.category }}
                  </p>
                </div>
              </div>
              <span class="font-medium">R$ {{ item.price.toFixed(2) }}</span>
            </div>
          </div>

          <div class="space-y-3 border-t pt-4">
            <div class="flex justify-between text-gray-600">
              <span>Subtotal:</span>
              <span>R$ {{ subtotal.toFixed(2) }}</span>
            </div>
            <div class="flex justify-between text-gray-600">
              <span>Frete:</span>
              <span>R$ {{ shippingCost.toFixed(2) }}</span>
            </div>
            <div class="flex justify-between font-bold text-lg pt-2">
              <span>Total:</span>
              <span>R$ {{ total.toFixed(2) }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
