using System;
using System.Collections.Generic;
using System.Text;

namespace MVP.Services.IServices;

public interface IOpenAIService
{
    Task<string> GenerateServiceDescriptionAsync(string title, string description);
}
