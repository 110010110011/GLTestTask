using System.Text.Json.Serialization;
public class PostBody
{
    [JsonPropertyName("expr")]
    public string[] Expr { get; set; }

    [JsonPropertyName("precision")]
    public int Precision { get; set; }
}
