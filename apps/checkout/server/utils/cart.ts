export function parseCartItems(tokens: string) {
  return tokens.split(',').reduce((items, item) => {
    const [product, qtyStr] = item.split(':')
    const qty = qtyStr ? Number.parseInt(qtyStr, 10) : 1
    if (!product || Number.isNaN(qty) || qty < 1) {
      throw new Error('Invalid format')
    }
    return {
      ...items,
      [product]: qty,
    }
  }, {} as Record<string, number>)
}
