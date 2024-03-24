using System.Text;
using System.Text.Json;

namespace TransactionMiddleware.Tests;

public class JsonContent : StringContent
{
    public JsonContent(string content) : base(content, Encoding.UTF8, "application/json")
    {
    }

    public JsonContent(object content) : base(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json")
    {
    }
}