using System.Text.Json.Serialization;
public class ResponseBody
{
    [JsonPropertyName("result")]
    public string[] Result { get; set; }

    [JsonPropertyName("error")]
    public string Error { get; set; }
}

