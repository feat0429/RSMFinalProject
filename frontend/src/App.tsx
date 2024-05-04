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
  const pathname = window.location.pathname
  switch (pathname) {
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
        Adventure Works
      </Typography>
      <Box
        gap={4}
        display='flex'
        alignItems='center'
        justifyContent='center'
        margin={3}
      >
        <Button
          variant={`${pathname === '/' ? 'contained' : 'outlined'}`}
          href='/'>
            Top Sales Report
        </Button>
        <Button
          variant={`${pathname === '/sales-orders-report' ? 'contained' : 'outlined'}`}
          href='/sales-orders-report'>
            Sales Orders Report
        </Button>
      </Box>

      {component}
    </ThemeProvider>
  )
}

export default App
