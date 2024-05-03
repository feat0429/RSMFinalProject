export interface SalesOrder {
  orderDate: Date
  customerName: string
  salesPersonName: string
  salesTerritory: string
  shippingAddress: string
  billingAddress: string
  subTotal: number
  totalDue: number
  salesOrderDetails: SalesOrderDetail[]
}

export interface SalesOrderDetail {
  productName: string
  productCategory: string
  unitPrice: number
  quantity: number
  lineTotal: number
}
