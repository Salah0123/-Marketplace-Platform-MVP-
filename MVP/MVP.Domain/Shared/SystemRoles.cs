using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Domain.Shared;

public static class SystemRoles
{
    public const string Customer = "Customer";
    public const string Provider = "Provider";
    public const string Admin = "Admin";

    public static IReadOnlyList<string> All =>
    new[]
    {
        Customer,
        Provider,
        Admin
    };
}
