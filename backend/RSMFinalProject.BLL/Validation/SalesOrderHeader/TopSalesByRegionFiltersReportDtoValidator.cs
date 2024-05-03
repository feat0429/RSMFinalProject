namespace RSMFinalProject.BLL.Validation.SalesOrderHeader
{
    using FluentValidation;
    using RSMFinalProject.DTO.SalesOrderHeader.Filters;

    public class TopSalesByRegionFiltersReportDtoValidator : AbstractValidator<TopSalesByRegionReportFiltersDto>
    {
        public TopSalesByRegionFiltersReportDtoValidator()
        {
            RuleFor(filter => filter.FilterDate)
                .NotNull();

            RuleFor(filter => filter.ProductCategoryId)
                .GreaterThan(0);

            RuleFor(filter => filter.TerritoryId)
                .GreaterThan(0);
        }
    }
}
