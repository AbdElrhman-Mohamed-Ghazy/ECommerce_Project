using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (! validators.Any())
            {
                return await next();
            }
            var context= new ValidationContext<TRequest>(request);
            var results= await Task.WhenAll(validators.Select(e => e.ValidateAsync(context, cancellationToken)));
            var faluior= results.SelectMany(r=>r.Errors).Where(r=> r is not null).ToList();
            if (faluior.Count != 0)
            {
                throw new ValidationException(faluior);
            }
            return await next(cancellationToken);
        }
    }
}
