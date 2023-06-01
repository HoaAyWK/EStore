using FluentValidation;

namespace EStore.Application.ProductAttributes.Queries.GetProductAttributeById;

public class GetProductAttributeByIdQueryValidator : AbstractValidator<GetProductAttributeByIdQuery>
{
    public GetProductAttributeByIdQueryValidator()
    {
        RuleFor(x => x.ProductAttributeId).NotEmpty();
    }
}
