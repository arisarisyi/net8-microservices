using System.Net;
using System.Text;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using static Mango.Web.Utility.SD;
using Newtonsoft.Json;


namespace Mango.Web.Service;

public class BaseService : IBaseService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    public BaseService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
    {
        try
        {
        HttpClient client = _httpClientFactory.CreateClient("MangoApi");
        HttpRequestMessage message = new();
        message.Headers.Add("Accept","application/json");
        //TODO: Token

        message.RequestUri = new Uri(requestDto.Url);
        if (requestDto.Data != null)
        {
            message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data),
                Encoding.UTF8, "application/json");
        }

        HttpResponseMessage? apiResponse = null;

        switch (requestDto.ApiType)
        {
            case ApiTypes.Post:
                message.Method = HttpMethod.Post;
                break;
            case ApiTypes.Put:
                message.Method =  HttpMethod.Put;
                break;
            case ApiTypes.Delete:
                message.Method = HttpMethod.Delete;
                break;
            default :
                message.Method = HttpMethod.Get;
                break;
        }
        
        apiResponse = await client.SendAsync(message);

        switch (apiResponse.StatusCode)
        {
            case HttpStatusCode.NotFound:
                return new() {IsSuccess = false, Message = "Not found"};
            case HttpStatusCode.Forbidden:
                return new() {IsSuccess = false, Message = "Forbidden"};
            case HttpStatusCode.Unauthorized:
                return new() {IsSuccess = false, Message = "Unauthorized"};
            case HttpStatusCode.InternalServerError:
                return new() {IsSuccess = false, Message = "Internal Server Error"};
            default:
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                return apiResponseDto;
        }
        }
        catch (Exception e)
        {
            var dto = new ResponseDto()
            {
                IsSuccess = false,
                Message = e.Message.ToString(),
            };
            Console.WriteLine(e);
            return dto;
        }
    }
}