using Google.GenAI;
using MVP.Services.IServices;
using System.Net.Http.Json;

namespace MVP.Services.Services;

public class OpenAIService : IOpenAIService
{

    public async Task<string> GenerateServiceDescriptionAsync(string title, string description)
    {
        var client = new Client(apiKey: "");



        var prompt = $@"
            Analyze the following service request:
            Title: {title}
            Description: {description}

            Tasks:
            1. Category: Choose the best category from (Plumbing, Electrical, Cleaning, Moving).
            2. RefinedDescription: Rewrite the description to be more professional and clear in the SAME language as the original input.
            3. SuggestedPrice: Suggest a fair price in USD as a single number.

            Return the response ONLY as a valid JSON object with these keys: 
            {{
              ""category"": """",
              ""refinedDescription"": """",
              ""suggestedPrice"": 0
            }}";



        var response = await client.Models.GenerateContentAsync(
            model: "gemini-3.1-flash-lite-preview",
            contents: prompt
        );
        return response?.Candidates?[0]?.Content?.Parts?[0]?.Text ?? "";
        //return response.Candidates[0].Content.Parts[0].Text ?? " ";
        
    }

}
