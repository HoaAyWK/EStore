using FluentValidation;

namespace EStore.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
