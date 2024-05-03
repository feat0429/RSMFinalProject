import { type TopSales } from '../../utils/types/top-sales-report'
import axios from '../../utils/axios/axios-config'

export async function getTopSales (): Promise<TopSales[]> {
  const response = await axios.get<TopSales[]>('/api/sales-order-header/report/top-by-region')

  return response.data
}
