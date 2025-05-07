import type { UseHeadInput } from '@unhead/vue/types'
import { defineNuxtPlugin, useAppConfig, useHead, useNuxtApp } from '#imports'
import { computed } from 'vue'

export default defineNuxtPlugin(() => {
  const appConfig = useAppConfig()
  const nuxtApp = useNuxtApp()

  const root = computed(() => {
    const { header, content, footer } = appConfig.checkoutUi

    return `@layer base {
  :root {
    --checkout-header-background: ${header.backgroundColor};
    --checkout-header-foreground: ${header.foregroundColor};
    
    --checkout-radius: ${content.radius};
    --checkout-primary-button: ${content.primaryButtonColor};
    --checkout-primary-button-foreground: ${content.primaryButtonForegroundColor};
    --checkout-secondary-button: ${content.secondaryButtonColor};
    --checkout-secondary-button-foreground: ${content.secondaryButtonForegroundColor};
    --checkout-accent: ${content.accentColor};
    --checkout-background: ${content.backgroundColor};
    --checkout-foreground: ${content.foregroundColor};
    
    --checkout-footer-background: ${footer.backgroundColor};
    --checkout-footer-foreground: ${footer.foregroundColor};
  }
}`
  })

  // Head
  const headData: UseHeadInput = {
    style: [{
      innerHTML: () => root.value,
      tagPriority: -2,
      id: 'checkout-ui-theme',
    }],
  }

  // SPA mode
  if (import.meta.client && nuxtApp.isHydrating && !nuxtApp.payload.serverRendered) {
    const style = document.createElement('style')

    style.innerHTML = root.value
    style.setAttribute('data-checkout-ui-theme', '')
    document.head.appendChild(style)

    headData.script = [{
      innerHTML: 'document.head.removeChild(document.querySelector(\'[data-checkout-ui-theme]\'))',
    }]
  }

  useHead(headData)
})
