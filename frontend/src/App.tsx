import { Box, Button, ThemeProvider, Typography, createTheme } from '@mui/material'
import './App.css'
import { useMemo } from 'react'
import SalesOrdersReport from './pages/SalesOrdersReportPage'
import TopSalesReport from './pages/TopSalesReportPage'

function App () {
  const globalTheme = useMemo(
    () =>
      createTheme({
        palette: {
          mode: 'dark'
        }
      })
    , [])

  let component

  switch (window.location.pathname) {
    case '/':
      component = <TopSalesReport />
      break
    case '/sales-orders-report':
      component = <SalesOrdersReport />
      break
  }
  return (
    <ThemeProvider theme={globalTheme}>
      <Typography variant='h1'>
        Aventure Works
      </Typography>
      <Box
        gap={4}
        display='flex'
        alignItems='center'
        justifyContent='center'
        margin={3}
      >
        <Button
          variant='outlined'
          href='/top-sales-report'>
            Top Sales Ordes
        </Button>
        <Button
          variant='outlined'
          href='/'>
            Sales Orders Report
        </Button>
      </Box>

      {component}
    </ThemeProvider>
  )
}

export default App
