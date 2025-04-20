<script lang="ts" setup>
const x = ref('something')

onMounted(() => {
  x.value = 'ola mundo'
})
</script>

<template>
  <header>
    <h1>Clothe Store</h1>
  </header>

  <nav class="steps-container" aria-label="Etapas do checkout">
    <div class="steps">
      <div class="step active">
        <div class="step-circle">
          1
        </div>
        <span style="margin-left: 6px">Informações pessoais</span>
      </div>
      <div class="divider" />
      <div class="step">
        <div class="step-circle">
          2
        </div>
        <span style="margin-left: 6px">Entrega</span>
      </div>
      <div class="divider" />
      <div class="step">
        <div class="step-circle">
          3
        </div>
        <span style="margin-left: 6px">Pagamento</span>
      </div>
    </div>
  </nav>

  <main class="container">
    <div class="resumo-container w-full">
      <div class="resumo-info">
        <span class="resumo-titulo">RESUMO (2)</span>
        <span class="resumo-descricao">Informações da sua compra</span>
      </div>
      <div class="resumo-preco">
        R$ 139,90
      </div>
    </div>

    <form aria-label="Formulário de informações pessoais">
      <div>
        <label for="email">E-mail</label>
        <input id="email" type="email" placeholder="Ex.: seu.e-mail@gmail.com" required>
        <p class="error">
          Este campo é obrigatório
        </p>
      </div>
      <div>
        <label for="nome">Nome completo</label>
        <input id="nome" type="text" placeholder="Informe seu primeiro nome" required>
        <p class="error">
          Este campo é obrigatório
        </p>
      </div>
      <div>
        <label for="cpf">CPF</label>
        <input id="cpf" type="text" placeholder="000.000.000-00">
      </div>
      <div>
        <label for="celular">Celular/WhatsApp</label>
        <input id="celular" type="tel" placeholder="(00) 00000-0000">
      </div>
      <button type="submit">
        CONTINUAR →
      </button>
    </form>
  </main>
</template>

<style>
body {
  font-family: sans-serif;
  background-color: #f3f4f6;
  margin: 0;
  padding: 0;
}

header,
section,
form {
  padding: 24px;
}

header {
  border-bottom: 1px solid #e5e7eb;
}

.summary {
  background: #f9fafb;
  border-bottom: 1px solid #e5e7eb;
  display: flex;
  justify-content: space-between;
  font-size: 14px;
  font-weight: 500;
}

.steps-container {
  padding: 24px;
}

.steps {
  display: flex;
  align-items: center;
  margin-top: 0;
}

.step {
  display: flex;
  align-items: center;
  color: #9ca3af;
  font-size: 14px;
}

.step.active {
  color: #db2777;
}

.step-circle {
  width: 24px;
  height: 24px;
  border-radius: 9999px;
  border: 2px solid currentColor;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 12px;
  font-weight: bold;
}

.divider {
  flex: 1;
  height: 1px;
  background: #d1d5db;
  margin: 0 8px;
}

form {
  display: flex;
  flex-direction: column;
  gap: 16px;
}

label {
  font-size: 14px;
  margin-bottom: 4px;
  display: block;
}

input {
  padding: 8px;
  font-size: 14px;
  border-radius: 4px;
  border: 1px solid #d1d5db;
  width: 100%;
}

input:required:invalid {
  border-color: #f87171;
}

.error {
  font-size: 12px;
  color: #ef4444;
  margin-top: 4px;
}

button {
  background: #db2777;
  color: white;
  font-weight: bold;
  padding: 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background 0.2s ease;
}

button:hover {
  background: #be185d;
}

.resumo-container {
  background-color: #edf1f5;
  padding: 16px;
  border-radius: 6px;
  max-width: 360px;
  font-family: sans-serif;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.resumo-info {
  display: flex;
  flex-direction: column;
}

.resumo-titulo {
  font-weight: bold;
  font-size: 14px;
  color: #1a1a1a;
}

.resumo-descricao {
  font-size: 13px;
  color: #666;
}

.resumo-preco {
  font-weight: bold;
  font-size: 14px;
  color: #1a1a1a;
}
</style>
