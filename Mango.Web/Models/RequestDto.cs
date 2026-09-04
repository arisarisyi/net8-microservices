using static Mango.Web.Utility.SD;

namespace Mango.Web.Models;

public class RequestDto
{
    public ApiTypes ApiType { get; set; } = ApiTypes.Get;
    public string Url { get; set; }
    public object Data { get; set; }
    public string AccessToken { get; set; }
    
    public ContentType ContentType { get; set; } = ContentType.Json;
}