import axios from '../../utils/axios/axios-config'
import { type SalesTerritory } from '../../utils/types/sales-territory'

export async function getSalesTerritories (): Promise<SalesTerritory[]> {
  const response = await axios.get<SalesTerritory[]>('/api/sales-territory')

  return response.data
}
