namespace RSMFinalProject.BLL.Validation.SalesOrderHeader
{
    using FluentValidation;
    using RSMFinalProject.DTO.SalesOrderHeader.Filters;

    public class TopSalesByRegionFiltersReportDtoValidator : AbstractValidator<TopSalesByRegionReportFiltersDto>
    {
        public TopSalesByRegionFiltersReportDtoValidator()
        {
            RuleFor(filter => filter.ResultCount)
                .NotNull()
                .InclusiveBetween(1, 100);

            RuleFor(filter => filter.ProductCategoryId)
                .GreaterThan(0);

            RuleFor(filter => filter.TerritoryId)
                .GreaterThan(0);
        }
    }
}
