using Application.Common.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture_CQRS_inAction.Exceptions
{
    public sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // تحديد تفاصيل المشكلة بناءً على نوع الاستثناء
            ProblemDetails problem = exception switch
            {
                // حالة استثناءات التحقق من البيانات (FluentValidation)
                ValidationException ex => new ValidationProblemDetails(
                    ex.Errors.GroupBy(e => e.PropertyName)
                             .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Title = "Validation failed",
                    Status = StatusCodes.Status400BadRequest
                },

                // حالة استثناء "العنصر غير موجود" (Custom Exception)
                NotFoundException ex => new ProblemDetails
                {
                    Title = "Resource Not Found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound
                },

                // حالة أي خطأ غير متوقع آخر
                _ => new ProblemDetails
                {
                    Title = "Server Error",
                    Detail = "An unexpected error occurred on the server.",
                    Status = StatusCodes.Status500InternalServerError
                }
            };

            // ضبط كود الحالة في الاستجابة
            httpContext.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;

            // كتابة تفاصيل المشكلة في استجابة الـ HTTP
            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = problem,
                Exception = exception
            });
        }
    }
}
