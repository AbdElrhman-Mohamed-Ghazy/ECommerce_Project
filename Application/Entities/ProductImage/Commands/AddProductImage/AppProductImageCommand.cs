using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.ProductImage.Commands.AddProductImage
{
    public sealed record  AppProductImageCommand(Guid ProductId,List<ImageFileDto> Images):IRequest
    {
    }
}
