import { Box, Button, FormControl, InputLabel, MenuItem, Select, TextField } from '@mui/material'
import { DateField, LocalizationProvider } from '@mui/x-date-pickers'
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import dayjs, { type Dayjs } from 'dayjs'
import { useEffect, useState } from 'react'
import { type SalesTerritory, type SalesTerritoryId } from '../../utils/types/sales-territory'
import { getSalesTerritories } from '../../services/adventure-works/sales-terrytory-service'
import { type SalesOrdersFilters } from '../../utils/types/filters'
import { type MRT_ColumnDef, type MRT_Row } from 'material-react-table'
import { type SalesOrder } from '../../utils/types/sales-orders'
import jsPDF from 'jspdf'
import autoTable from 'jspdf-autotable'
import { mkConfig, generateCsv, download } from 'export-to-csv'
import currencyFormatter from '../../utils/formatters/currency-formatter'

interface Props {
  handleFilters: (filters: SalesOrdersFilters) => void
  isLoading: boolean
  rows: Array<MRT_Row<SalesOrder>>
  columns: Array<MRT_ColumnDef<SalesOrder>>
}

export function SalesOrderHeaderToolbar ({ handleFilters, isLoading, rows, columns }: Props) {
  const [salesTerritories, setSalesTerritories] = useState<SalesTerritory[]>([])
  const [startDate, setStartDate] = useState<Dayjs | null>(dayjs(new Date()).add(-13, 'years'))
  const [endDate, setEndDate] = useState<Dayjs | null>(dayjs(new Date()).add(-10, 'years'))
  const [searchCustomer, setSearchCustomer] = useState<string>('')
  const [searchSalesPerson, setSearchSalesPerson] = useState<string>('')
  const [selectSalesTerritory, setSelectSalesTerritory] = useState<SalesTerritoryId>(0)

  const csvConfig = mkConfig({
    fieldSeparator: ',',
    decimalSeparator: '.',
    useKeysAsHeaders: true
  })

  useEffect(() => {
    getSalesTerritories()
      .then(territories => {
        setSalesTerritories(territories)
      })
      .catch(e => { console.log(e) })
  }, [])

  const handleSubmitFilters = () => {
    handleFilters({
      customerName: searchCustomer,
      salesPersonName: searchSalesPerson,
      startDate: startDate?.toDate(),
      endDate: endDate?.toDate(),
      salesTerritoryId: selectSalesTerritory === 0 ? undefined : selectSalesTerritory
    })
  }

  const handlePDF = () => {
    // eslint-disable-next-line new-cap
    const pdf = new jsPDF()
    const tableData = rows.map((row) => {
      return [
        new Date(row.original.orderDate).toLocaleDateString(),
        row.original.customerName,
        row.original.salesPersonName,
        row.original.salesTerritory,
        row.original.shippingAddress,
        row.original.billingAddress,
        currencyFormatter.format(row.original.subTotal),
        currencyFormatter.format(row.original.totalDue)
      ]
    })

    const tableHeaders = columns.map((c) => c.header)

    autoTable(pdf, {
      head: [tableHeaders],
      body: tableData
    })

    pdf.save('sales-order-header.pdf')
  }
  const handleCSV = () => {
    const rowData = rows.map((row) => {
      return {
        orderDate: new Date(row.original.orderDate).toLocaleDateString(),
        customerName: row.original.customerName,
        salesPersonName: row.original.salesPersonName,
        salesTerritory: row.original.salesTerritory,
        shippingAddres: row.original.shippingAddress,
        billingAddress: row.original.billingAddress,
        subTotal: row.original.subTotal,
        totalDue: row.original.totalDue
      }
    })

    const csvOutput = generateCsv(csvConfig)(rowData)

    download(csvConfig)(csvOutput)
  }

  return (
        <>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
                <DateField
                    label='Start Date'
                    value={startDate}
                    onChange={value => { setStartDate(value) }}
                    sx={{ maxWidth: '145px' }}
                    size='small'
                />
                <DateField
                    label='End Date'
                    value={endDate}
                    onChange={value => { setEndDate(value) }}
                    sx={{ maxWidth: '145px' }}
                    size='small'
                />
            </LocalizationProvider>
             <TextField
                id='customer-name'
                label='Search Customer'
                value={searchCustomer}
                onChange={event => { setSearchCustomer(event.target.value) }}
                placeholder='José Pérez'
                sx={{ maxWidth: '175px' }}
                size='small'
            />
            <TextField
                id='customer-name'
                label='Search Sales Person'
                value={searchSalesPerson}
                onChange={event => { setSearchSalesPerson(event.target.value) }}
                placeholder='Alejandra Palma'
                sx={{ maxWidth: '200px' }}
                size='small'
            />
            <FormControl size='small'>
                <InputLabel id='territory-select-label'>Territory</InputLabel>
                <Select
                    id='territory-select'
                    label='Territory'
                    labelId='territory-select-label'
                    value={selectSalesTerritory}
                    onChange={event => { setSelectSalesTerritory(Number(event.target.value)) }}
                >
                    <MenuItem value={0}>None</MenuItem>
                {
                    salesTerritories.map(territory => <MenuItem key={territory.territoryId} value={territory.territoryId}>{territory.name}</MenuItem>)
                }
                </Select>
            </FormControl>
            <Box sx={{ display: 'flex', flexDirection: 'row', gap: '10px' }}>
                <Button
                    onClick={() => { handleSubmitFilters() }}
                    disabled={isLoading}
                    size='small'
                    variant='contained'
                >
                    Search
                </Button>
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
