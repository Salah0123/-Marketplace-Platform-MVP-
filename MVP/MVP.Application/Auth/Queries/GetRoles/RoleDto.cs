using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Application.Auth.Queries.GetRoles;

public record RoleDto
{
    public string RoleKey { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
}
