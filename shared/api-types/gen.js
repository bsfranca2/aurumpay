import { writeFile } from 'node:fs/promises'
import { defineCommand, runMain } from 'citty'
import { consola } from 'consola'
import { ESLint } from 'eslint'
import { ofetch } from 'ofetch'
import {
  InputData,
  JSONSchemaInput,
  quicktype,
} from 'quicktype-core'

const main = defineCommand({
  meta: {
    name: 'gen',
    description: 'Generate API types',
  },
  args: {
    openapi: {
      type: 'positional',
      description: 'OpenAPI file',
      required: true,
    },
    output: {
      type: 'positional',
      description: 'Output file',
      required: true,
    },
  },
  async run({ args }) {
    const openapi = await ofetch(args.openapi)

    const originalSchemas = { ...openapi.components.schemas }
    const schemaKeys = Object.keys(originalSchemas)

    consola.info(`Found ${schemaKeys.length} schemas to process`)

    const nameMapping = {}

    for (const key of schemaKeys) {
      const newName = key.endsWith('Dto') ? key.slice(0, -3) : key
      nameMapping[key] = newName
    }

    function adjustReferences(obj) {
      if (!obj || typeof obj !== 'object')
        return obj

      if (Array.isArray(obj)) {
        return obj.map(item => adjustReferences(item))
      }

      const result = {}
      for (const [key, value] of Object.entries(obj)) {
        if (key === '$ref' && typeof value === 'string' && value.startsWith('#/components/schemas/')) {
          const refName = value.replace('#/components/schemas/', '')

          if (nameMapping[refName]) {
            result[key] = `#/definitions/${nameMapping[refName]}`
          }
          else {
            result[key] = `#/definitions/${refName}`
          }
        }
        else if (typeof value === 'object') {
          result[key] = adjustReferences(value)
        }
        else {
          result[key] = value
        }
      }

      return result
    }

    const adjustedSchemas = {}
    for (const key of schemaKeys) {
      const newName = nameMapping[key]
      const adjustedSchema = adjustReferences(originalSchemas[key])
      adjustedSchemas[newName] = adjustedSchema
    }

    const rootSchema = {
      type: 'object',
      properties: {},
    }

    for (const newName of Object.values(nameMapping)) {
      rootSchema.properties[newName] = { $ref: `#/definitions/${newName}` }
    }

    rootSchema.definitions = adjustedSchemas

    const inputData = new InputData()
    const schemaInput = new JSONSchemaInput(undefined)

    await schemaInput.addSource({
      name: 'API',
      schema: JSON.stringify(rootSchema),
    })

    inputData.addInput(schemaInput)

    consola.start('Generating types...')
    const result = await quicktype({
      inputData,
      lang: 'typescript',
      rendererOptions: {
        'just-types': 'true',
        'explicit-unions': 'true',
        'acronym-style': 'original',
      },
      alphabetizeProperties: true,
      inferMaps: true,
      inferEnums: true,
      inferDateTimes: true,
      combineClasses: false,
      allPropertiesOptional: false,
    })

    let content = result.lines.join('\n')

    content = content.replace(/\s*\[property: string\]: any;?\s*/g, '')

    // content = content.replace(/export interface API \{[\s\S]*?\}\n\n/, '')

    const header = '// Generated types from OpenAPI schema\n\n'
    content = header + content

    try {
      consola.start('Applying ESLint formatting...')
      const eslint = new ESLint({
        fix: true,
      })

      const results = await eslint.lintText(content, {
        filePath: args.output,
      })

      if (results[0]?.output) {
        content = results[0].output
        consola.success('ESLint formatting applied')
      }
      else {
        consola.success('No ESLint fixes needed')
      }
    }
    catch (error) {
      consola.warn(`ESLint formatting skipped: ${error.message}`)
    }

    await writeFile(args.output, content)

    consola.success(`Finished generating types to ${args.output}`)
  },
})

runMain(main)
