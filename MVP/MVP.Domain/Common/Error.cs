using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Domain.Common;

public record Error(string Code, string Description, int? StatusCode)
{
    public static readonly Error None = new(string.Empty, string.Empty, null);
}
