import axios from '../../utils/axios/axios-config'
import { type SalesOrdersFilters } from '../../utils/types/filters'
import { type PaginationParameters, type PaginationResults } from '../../utils/types/pagination'
import { type SalesOrder } from '../../utils/types/sales-orders'

export async function searchSales (filters: SalesOrdersFilters & PaginationParameters): Promise<PaginationResults<SalesOrder>> {
  const response = await axios.get<PaginationResults<SalesOrder>>('/api/sales-order-header/search', {
    params: filters
  })

  return response.data
}
