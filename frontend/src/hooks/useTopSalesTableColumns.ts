import { type MRT_ColumnDef } from 'material-react-table'
import { useMemo } from 'react'
import currencyFormatter from '../utils/formatters/currency-formatter'
import { type TopSales } from '../utils/types/top-sales-report'
import percentageFormatter from '../utils/formatters/percentage-formatter'

export function useTopSalesTableColumns () {
  const columns = useMemo<Array<MRT_ColumnDef<TopSales>>>(() => [
    {
      accessorKey: 'productName',
      header: 'Product Name'
    },
    {
      accessorKey: 'productCategory',
      header: 'Product Category'
    },
    {
      accessorFn: (row) => currencyFormatter.format(row.totalSales),
      id: 'totalSales',
      header: 'Total Sales'
    },
    {
      accessorKey: 'territory',
      header: 'Territory'
    },
    {
      accessorFn: (row) => percentageFormatter.format(row.salesByRegion),
      id: 'salesByRegion',
      header: 'Sales By Region'
    },
    {
      accessorFn: (row) => percentageFormatter.format(row.categorySalesByRegion),
      id: 'categorySalesByRegion',
      header: 'Category Sales By Region'
    },
    {
      accessorKey: 'categoryLastQuarter',
      header: 'Previous Quarter Category Growth'
    },
    {
      accessorKey: 'territoryLastQuarter',
      header: 'Previous Quarter Territory Growth'
    }

  ], [])

  return { columns }
}
