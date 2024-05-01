namespace RSMFinalProject.BLL.Services
{
    using AutoMapper;
    using FluentValidation;
    using RSMFinalProject.BLL.Services.Contract;
    using RSMFinalProject.DAL.Repositories.Contract;
    using RSMFinalProject.DTO.PagedList;
    using RSMFinalProject.DTO.Pagination;
    using RSMFinalProject.DTO.SalesOrderHeader;
    using RSMFinalProject.DTO.SalesOrderHeader.Filters;
    using System.Collections.Generic;

    public class SalesOrderHeaderService : ISalesOrderHeaderService
    {
        private readonly ISalesOrderHeaderRepository _salesOrderHeaderRepository;
        private readonly IValidator<SalesSearchtFiltersDto> _salesReportFiltersDtoValidator;
        private readonly IValidator<PaginationParamsDto> _paginationParamsDtoValidator;
        private readonly IValidator<TopSalesByRegionReportFiltersDto> _topSalesByRegionReportFilterDtoValidator;
        private readonly IMapper _mapper;

        public SalesOrderHeaderService(ISalesOrderHeaderRepository salesOrderHeaderRepository, IMapper mapper, IValidator<SalesSearchtFiltersDto> salesReportFiltersDtoValidator, IValidator<PaginationParamsDto> paginationParamsDtoValidator, IValidator<TopSalesByRegionReportFiltersDto> topSalesByRegionReportFilterDtoValidator)
        {
            _salesOrderHeaderRepository = salesOrderHeaderRepository;
            _mapper = mapper;
            _salesReportFiltersDtoValidator = salesReportFiltersDtoValidator;
            _paginationParamsDtoValidator = paginationParamsDtoValidator;
            _topSalesByRegionReportFilterDtoValidator = topSalesByRegionReportFilterDtoValidator;
        }

        public async Task<PagedListDto<SalesReportDto>> GetSalesReport(PaginationParamsDto paginationParams, SalesSearchtFiltersDto filterCriteria)
        {
            var paginationValidationResults = _paginationParamsDtoValidator.Validate(paginationParams);
            var filtersValidationResults = _salesReportFiltersDtoValidator.Validate(filterCriteria);

            if (!filtersValidationResults.IsValid || !paginationValidationResults.IsValid)
                throw new ValidationException(filtersValidationResults.Errors.Concat(paginationValidationResults.Errors));

            var salesOrders = await _salesOrderHeaderRepository.SearchSalesOrders(paginationParams, filterCriteria);

            var salesReportDtos = _mapper.Map<PagedListDto<SalesReportDto>>(salesOrders);

            return salesReportDtos;
        }

        public Task<IEnumerable<TopProductSalesByRegionDto>> GetTopSalesByRegionReport(TopSalesByRegionReportFiltersDto filterCriteria)
        {
            var validationResults = _topSalesByRegionReportFilterDtoValidator.Validate(filterCriteria);

            if (!validationResults.IsValid)
                throw new ValidationException(validationResults.Errors);

            return _salesOrderHeaderRepository.GetTopSalesByRegionReport(filterCriteria);
        }
    }
}
