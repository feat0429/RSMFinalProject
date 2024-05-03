import { Table, TableBody, TableCell, TableHead, TableRow } from '@mui/material'
import { type SalesOrderDetail } from '../../utils/types/sales-orders'
import currencyFormatter from '../../utils/formatters/currency-formatter'

interface Props {
  salesOrderDetails: SalesOrderDetail[]
}

export function SalesOrdersDetail ({ salesOrderDetails }: Props) {
  return (
        <Table size='small'>
            <TableHead>
                <TableRow>
                    <TableCell>Product</TableCell>
                    <TableCell>Category</TableCell>
                    <TableCell>Unit Price</TableCell>
                    <TableCell>Quantity</TableCell>
                    <TableCell>Line Total</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {
                    salesOrderDetails.map((detail, index) => (
                        <TableRow
                            key={index}
                            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                        >
                            <TableCell>{detail.productName}</TableCell>
                            <TableCell>{detail.productCategory}</TableCell>
                            <TableCell>{currencyFormatter.format(detail.unitPrice)}</TableCell>
                            <TableCell>{detail.quantity}</TableCell>
                            <TableCell>{currencyFormatter.format(detail.lineTotal)}</TableCell>
                        </TableRow>
                    ))
                }
            </TableBody>
        </Table>
  )
}
