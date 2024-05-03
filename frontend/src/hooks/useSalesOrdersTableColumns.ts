import { type MRT_ColumnDef } from 'material-react-table'
import { useMemo } from 'react'
import { type SalesOrder } from '../utils/types/sales-orders'
import currencyFormatter from '../utils/formatters/currency-formatter'

export function useSalesOrdersTableColumns () {
  const columns = useMemo<Array<MRT_ColumnDef<SalesOrder>>>(() => [
    {
      accessorFn: (row) => new Date(row.orderDate).toLocaleDateString(),
      id: 'orderDate',
      header: 'Order Date',
      maxSize: 50
    },
    {
      accessorKey: 'customerName',
      header: 'Customer',
      maxSize: 100

    }, {
      accessorFn: (row) => row.salesPersonName == null || row.salesPersonName === '' ? 'N/A' : row.salesPersonName,
      id: 'salesPersonName',
      header: 'Sales Person',
      maxSize: 100
    },
    {
      accessorKey: 'salesTerritory',
      header: 'Territory',
      maxSize: 80
    },
    {
      accessorKey: 'shippingAddress',
      header: 'Shipping Address'
    },
    {
      accessorKey: 'billingAddress',
      header: 'Billing Address'
    },
    {
      accessorFn: (row) => currencyFormatter.format(row.subTotal),
      id: 'subTotal',
      header: 'Sub Total',
      maxSize: 80
    },
    {
      accessorFn: (row) => currencyFormatter.format(row.totalDue),
      id: 'totalDue',
      header: 'Total Due',
      maxSize: 80
    }

  ], [])

  return { columns }
}
