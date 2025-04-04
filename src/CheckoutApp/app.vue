<script>
export default defineComponent({
  data() {
    return {
      cardNumber: '',
      cardName: '',
      expiryDate: '',
      cvv: '',
      errors: {
        cardNumber: '',
        cardName: '',
        expiryDate: '',
        cvv: '',
      },
    }
  },
  methods: {
    handleSubmit() {
      if (this.validateForm()) {
        // Lógica de envio aqui
        alert('Pagamento processado com sucesso! (Simulação)')
      }
    },

    validateForm() {
      let isValid = true
      this.errors = { cardNumber: '', cardName: '', expiryDate: '', cvv: '' }

      // Validação do número do cartão
      if (this.cardNumber.replace(/ /g, '').length !== 16) {
        this.errors.cardNumber = 'Número do cartão inválido'
        isValid = false
      }

      // Validação do nome
      if (!this.cardName.trim()) {
        this.errors.cardName = 'Por favor, insira o nome'
        isValid = false
      }

      // Validação da data
      if (!/^\d{2}\/\d{4}$/.test(this.expiryDate)) {
        this.errors.expiryDate = 'Data inválida (MM/AAAA)'
        isValid = false
      }

      // Validação do CVV
      if (!/^\d{3,4}$/.test(this.cvv)) {
        this.errors.cvv = 'CVV inválido'
        isValid = false
      }

      return isValid
    },

    formatCardNumber(event) {
      let value = event.target.value.replace(/\D/g, '')
      if (value.length > 16)
        value = value.slice(0, 16)
      this.cardNumber = value.replace(/(\d{4})(?=\d)/g, '$1 ')
    },

    formatExpiryDate(event) {
      let value = event.target.value.replace(/\D/g, '')
      if (value.length > 6)
        value = value.slice(0, 6)

      if (value.length > 2) {
        value = `${value.slice(0, 2)}/${value.slice(2)}`
      }
      this.expiryDate = value
    },
  },
})
</script>

<template>
  <div class="min-h-screen bg-gray-100 py-8">
    <div class="max-w-md mx-auto bg-white rounded-lg shadow-lg p-6">
      <h2 class="text-2xl font-bold mb-6 text-gray-800">
        Pagamento com Cartão
      </h2>

      <form class="space-y-6" @submit.prevent="handleSubmit">
        <!-- Número do Cartão -->
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">Número do Cartão</label>
          <input
            v-model="cardNumber"
            type="text"
            placeholder="1234 5678 9012 3456"
            maxlength="19"
            class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            @input="formatCardNumber"
          >
          <p v-if="errors.cardNumber" class="text-red-500 text-sm mt-1">
            {{ errors.cardNumber }}
          </p>
        </div>

        <!-- Nome no Cartão -->
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-2">Nome no Cartão</label>
          <input
            v-model="cardName"
            type="text"
            placeholder="João da Silva"
            class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          >
          <p v-if="errors.cardName" class="text-red-500 text-sm mt-1">
            {{ errors.cardName }}
          </p>
        </div>

        <div class="grid grid-cols-2 gap-4">
          <!-- Validade -->
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">Validade</label>
            <input
              v-model="expiryDate"
              type="text"
              placeholder="MM/AAAA"
              maxlength="7"
              class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
              @input="formatExpiryDate"
            >
            <p v-if="errors.expiryDate" class="text-red-500 text-sm mt-1">
              {{ errors.expiryDate }}
            </p>
          </div>

          <!-- CVV -->
          <div>
            <label class="block text-sm font-medium text-gray-700 mb-2">CVV</label>
            <input
              v-model="cvv"
              type="text"
              placeholder="123"
              maxlength="4"
              class="w-full px-4 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
            <p v-if="errors.cvv" class="text-red-500 text-sm mt-1">
              {{ errors.cvv }}
            </p>
          </div>
        </div>

        <button
          type="submit"
          class="w-full bg-blue-600 text-white py-3 px-4 rounded-lg hover:bg-blue-700 transition-colors font-medium"
        >
          Finalizar Pagamento
        </button>
      </form>

      <p class="text-center text-sm text-gray-500 mt-4">
        Transações seguras com criptografia SSL
      </p>
    </div>
  </div>
</template>
