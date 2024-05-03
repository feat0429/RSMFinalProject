import { Box, Button } from '@mui/material'
import { type MRT_ColumnDef, type MRT_Row } from 'material-react-table'
import jsPDF from 'jspdf'
import autoTable from 'jspdf-autotable'
import { mkConfig, generateCsv, download } from 'export-to-csv'
import currencyFormatter from '../../utils/formatters/currency-formatter'
import { type TopSales } from '../../utils/types/top-sales-report'
import percentageFormatter from '../../utils/formatters/percentage-formatter'

interface Props {
  isLoading: boolean
  rows: Array<MRT_Row<TopSales>>
  columns: Array<MRT_ColumnDef<TopSales>>
}

export function TopSalesToolbar ({ rows, columns }: Props) {
  const csvConfig = mkConfig({
    fieldSeparator: ',',
    decimalSeparator: '.',
    useKeysAsHeaders: true
  })

  const handlePDF = () => {
    // eslint-disable-next-line new-cap
    const pdf = new jsPDF()
    const tableData = rows.map((row) => {
      return [
        row.original.productName,
        row.original.productCategory,
        currencyFormatter.format(row.original.totalSales),
        row.original.territory,
        percentageFormatter.format(row.original.salesByRegion),
        percentageFormatter.format(row.original.categorySalesByRegion),
        row.original.categoryLastQuarter,
        row.original.territoryLastQuarter
      ]
    })

    const tableHeaders = columns.map((c) => c.header)

    autoTable(pdf, {
      head: [tableHeaders],
      body: tableData
    })

    pdf.save('top-sales-report.pdf')
  }
  const handleCSV = () => {
    const rowData = rows.map((row) => {
      return {
        productName: row.original.productName,
        productCategory: row.original.productCategory,
        totalSales: row.original.totalSales,
        territory: row.original.territory,
        salesByRegion: row.original.salesByRegion,
        categorySalesByRegion: row.original.categorySalesByRegion,
        categoryLastQuarter: row.original.categoryLastQuarter,
        territoryLastQuarter: row.original.territoryLastQuarter

      }
    })

    const csvOutput = generateCsv(csvConfig)(rowData)

    download(csvConfig)(csvOutput)
  }

  return (
        <>
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: '10px' }}>
              <Button
                size='small'
                variant='outlined'
                onClick={() => { handlePDF() }}
                color='secondary'
              >
                  PDF
              </Button>
              <Button
                size='small'
                variant='outlined'
                onClick={() => { handleCSV() }}
                color='secondary'

              >
                  CSV
              </Button>
            </Box>
        </>
  )
}
