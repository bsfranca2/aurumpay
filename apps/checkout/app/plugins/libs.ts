import { mask } from '@aurumpay/vue-the-mask'
import { configure } from 'vee-validate'

export default defineNuxtPlugin((nuxtApp) => {
  // Mask
  nuxtApp.vueApp.directive('mask', mask)

  // VeeValidate
  configure({
    validateOnBlur: true,
    validateOnChange: false,
    validateOnInput: false,
    validateOnModelUpdate: false,
  })
})
