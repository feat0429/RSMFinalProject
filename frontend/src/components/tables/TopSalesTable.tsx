import { keepPreviousData, useQuery } from '@tanstack/react-query'
import { getTopSales } from '../../services/adventure-works/top-sales-service'
import { type TopSales } from '../../utils/types/top-sales-report'
import { useTopSalesTableColumns } from '../../hooks/useTopSalesTableColumns'
import { MaterialReactTable, useMaterialReactTable } from 'material-react-table'
import { TopSalesToolbar } from '../toolbar/TopSalesToolbar'

export function TopSalesTable () {
  const {
    data: results = [],
    isLoading,
    isError,
    isFetching
  } = useQuery<TopSales[]>({
    queryKey: [
      'topSales'
    ],
    queryFn: async () => await getTopSales(),
    placeholderData: keepPreviousData
  })

  const { columns } = useTopSalesTableColumns()

  const table = useMaterialReactTable<TopSales>({
    columns,
    data: results,
    enableTopToolbar: true,
    enableSorting: false,
    enableFilters: false,
    initialState: {
      density: 'compact'
    },
    state: {
      isLoading,
      showAlertBanner: isError,
      showLoadingOverlay: isLoading,
      showProgressBars: isFetching && !isLoading
    },
    muiToolbarAlertBannerProps: isError
      ? {
          color: 'error',
          children: 'Error loading data. Please try again.'
        }
      : undefined,
    renderTopToolbarCustomActions: ({ table }) => <TopSalesToolbar rows={table.getRowModel().rows} columns={columns} isLoading={isFetching || isLoading} />
  })
  return (
        <>
            <MaterialReactTable table={table} />
        </>
  )
}
