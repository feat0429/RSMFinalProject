import { keepPreviousData, useQuery } from '@tanstack/react-query'
import { type PaginationResults } from '../../utils/types/pagination'
import { type MRT_PaginationState, MaterialReactTable, useMaterialReactTable } from 'material-react-table'
import { type SalesOrder } from '../../utils/types/sales-orders'
import { useState } from 'react'
import { searchSales } from '../../services/adventure-works/sales-order-header-service'
import { SalesOrdersDetail } from '../details-panel/SalesOrdersDetail'
import { SalesOrderHeaderToolbar } from '../toolbar/SalesOrderHeaderToolbar'
import { type SalesOrdersFilters } from '../../utils/types/filters'
import { useSalesOrdersTableColumns } from '../../hooks/useSalesOrdersTableColumns'

export function SalesOrderTable () {
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: 0,
    pageSize: 10
  })
  const [filters, setFilters] = useState<SalesOrdersFilters>({})

  const handleFilters = (filters: SalesOrdersFilters) => {
    setFilters(filters)
  }
  const {
    data: results,
    isLoading,
    isError,
    isFetching
  } = useQuery<PaginationResults<SalesOrder>>({
    queryKey: [
      'salesOrders',
      pagination.pageIndex,
      pagination.pageSize,
      filters
    ],
    queryFn: async () => await searchSales({ currentPage: pagination.pageIndex + 1, pageSize: pagination.pageSize, ...filters }),
    placeholderData: keepPreviousData
  })

  const { columns } = useSalesOrdersTableColumns()

  const table = useMaterialReactTable<SalesOrder>({
    columns,
    data: results?.items ?? [],
    manualPagination: true,
    pageCount: results?.totalPageCount,
    onPaginationChange: setPagination,
    paginationDisplayMode: 'pages',
    enableSorting: false,
    enableColumnActions: false,
    rowCount: results?.totalItemCount,
    enableExpandAll: false,
    enableRowNumbers: true,
    enableFilters: false,
    enableTopToolbar: true,
    enableHiding: false,
    enableFullScreenToggle: false,
    enableDensityToggle: false,
    initialState: {
      density: 'compact'
    },
    state: {
      isLoading,
      pagination,
      showAlertBanner: isError,
      showLoadingOverlay: isLoading,
      showProgressBars: isFetching && !isLoading
    },
    muiPaginationProps: {
      showRowsPerPage: false,
      siblingCount: 2
    },
    muiToolbarAlertBannerProps: isError
      ? {
          color: 'error',
          children: 'Error loading data. Please try again.'
        }
      : undefined,
    renderDetailPanel: ({ row }) => <SalesOrdersDetail salesOrderDetails={row.original.salesOrderDetails} />,
    renderTopToolbarCustomActions: ({ table }) => <SalesOrderHeaderToolbar rows={table.getPaginationRowModel().rows} columns={columns} isLoading={isFetching || isLoading} handleFilters={handleFilters} />
  })
  return (
        <>
            <MaterialReactTable table={table} />
        </>
  )
}
