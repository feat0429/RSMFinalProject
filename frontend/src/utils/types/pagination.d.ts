export interface PaginationParameters {
  pageSize: number
  currentPage: number
}

export interface PaginationResults<T> extends PaginationParameters {
  totalItemCount: number
  totalPageCount: number
  hasPrevious: boolean
  hasNext: boolean
  items: T[]
}
