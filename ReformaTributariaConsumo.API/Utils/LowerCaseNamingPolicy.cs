using System.Text.Json;

namespace ReformaTributaria.API.Utils;

public class LowerCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        return name.ToLower();
    }
}