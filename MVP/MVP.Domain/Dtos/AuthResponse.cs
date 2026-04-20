using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Domain.Dtos;

public class AuthResponse
{
    public string? Username { get; set; }
    public string? Token { get; set; }
    public int ExpiresIn { get; set; }
}
