import type { Resolver } from '@nuxt/kit'
import type { Nuxt, NuxtTemplate, NuxtTypeTemplate } from '@nuxt/schema'
import type { ModuleOptions } from './module'
import { addTemplate, addTypeTemplate } from '@nuxt/kit'

export function getTemplates(_options: ModuleOptions, _uiConfig: Record<string, any>) {
  const templates: NuxtTemplate[] = []

  templates.push({
    filename: 'ui.css',
    write: true,
    getContents: () => `@source "./ui";

@theme default inline {
  --color-checkout-header-bg: var(--checkout-header-background);
  --color-checkout-header-fg: var(--checkout-header-foreground);
  
  --color-checkout-primary-button: var(--checkout-primary-button);
  --color-checkout-primary-button-fg: var(--checkout-primary-button-foreground);
  --color-checkout-secondary-button: var(--checkout-secondary-button);
  --color-checkout-secondary-button-fg: var(--checkout-secondary-button-foreground);
  
  --color-checkout-accent: var(--checkout-accent);
  --color-checkout-bg: var(--checkout-background);
  --color-checkout-fg: var(--checkout-foreground);
  
  --color-checkout-footer-bg: var(--checkout-footer-background);
  --color-checkout-footer-fg: var(--checkout-footer-foreground);
  
  --color-ring: var(--ring);
}
`,
  })

  templates.push({
    filename: 'types/checkout-ui.d.ts',
    getContents: () => `import * as ui from '#build/ui'
import type { DeepPartial } from '@nuxt/ui'

type AppConfigUI = {
  header?: {
    backgroundColor?: string
    foregroundColor?: string
  },
  content?: {
    radius?: string
    primaryButtonColor?: string
    primaryButtonForegroundColor?: string
    secondaryButtonColor?: string
    secondaryButtonForegroundColor?: string
    accentColor?: string
    backgroundColor?: string
    foregroundColor?: string
  },
  footer?: {
    backgroundColor?: string
    foregroundColor?: string
  }
} & DeepPartial<typeof ui>

declare module '@nuxt/schema' {
  interface AppConfigInput {
    /**
     * Checkout UI theme configuration
     */
    checkoutUi?: AppConfigUI
  }
}

export {}
`,
  })

  return templates
}

export function addTemplates(options: ModuleOptions, nuxt: Nuxt, resolve: Resolver['resolve']) {
  const templates = getTemplates(options, nuxt.options.appConfig.checkoutUi)
  for (const template of templates) {
    if (template.filename!.endsWith('.d.ts')) {
      addTypeTemplate(template as NuxtTypeTemplate)
    }
    else {
      addTemplate(template)
    }
  }

  nuxt.hook('prepare:types', ({ references }) => {
    references.push({ path: resolve('./runtime/types/app.config.d.ts') })
  })
}
