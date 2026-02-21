using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public record ImageFileDto(
    byte[] Content,
    string FileName,
    string ContentType
);
}
