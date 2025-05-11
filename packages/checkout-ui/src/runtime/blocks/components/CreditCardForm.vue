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
import { useForm } from 'vee-validate'

const { isSubmitting } = useForm()
</script>

<template>
  <form>
    <FormField v-slot="{ componentField }" name="cardNumber">
      <FormItem>
        <FormLabel>
          Número do cartão
        </FormLabel>
        <FormControl>
          <Input
            v-mask="['#### #### #### ####']"
            type="text"
            placeholder="1234 1234 1234 1234"
            v-bind="componentField"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <div class="cvv">
      <FormField v-slot="{ componentField }" name="expiryDate">
        <FormItem>
          <FormLabel>Validade (mês/ano)</FormLabel>
          <FormControl>
            <Input
              v-mask="['##/####']"
              type="text"
              placeholder="MM/AA"
              v-bind="componentField"
            />
          </FormControl>
          <FormMessage />
        </FormItem>
      </FormField>
      <FormField v-slot="{ componentField }" name="cvc">
        <FormItem>
          <FormLabel class="text-sm font-medium text-gray-700">
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
                    Os 3 ou 4 dígitos no verso do seu cartão (ou 4 na frente
                    para Amex).
                  </p>
                </TooltipContent>
              </Tooltip>
            </TooltipProvider>
          </FormLabel>
          <FormControl>
            <Input v-mask="['####']" type="text" v-bind="componentField" maxlegth="4" />
          </FormControl>
          <FormMessage />
        </FormItem>
      </FormField>
    </div>

    <FormField v-slot="{ componentField }" name="cardHolderName">
      <FormItem>
        <FormLabel>
          Nome e sobrenome do titular
        </FormLabel>
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

    <FormField v-slot="{ componentField }" name="cpf">
      <FormItem>
        <FormLabel>
          CPF do titular
        </FormLabel>
        <FormControl>
          <Input
            v-mask="['###.###.###-##']"
            type="text"
            placeholder="000.000.000-00"
            v-bind="componentField"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <Button type="submit" :loading="isSubmitting" block>
      Finalizar compra
    </Button>
  </form>
</template>
