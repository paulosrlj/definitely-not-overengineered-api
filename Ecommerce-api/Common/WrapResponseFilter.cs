using Ecommerce_api.Common.Pagination;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ecommerce_api.Common;

public class WrapResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is not ObjectResult objectResult) return;
        if (objectResult.Value is null) return;

        // não embrulha respostas já paginadas
        // Aqui, graças a interface, não precisamos declarar o "T" do generic se fossemos usar direto o "PaginatedResponse<>"
        if (objectResult.Value is IPaginatedResponse) return;

        // não embrulha erros (ProblemDetails, ValidationProblemDetails, etc.)
        if (objectResult.Value is ProblemDetails) return;

        objectResult.Value = new { data = objectResult.Value };
    }
}