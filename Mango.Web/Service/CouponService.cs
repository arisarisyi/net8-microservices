using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;

namespace Mango.Web.Service;

public class CouponService : ICouponService
{
    private readonly IBaseService _baseService;
    public CouponService(IBaseService baseService)
    {
        _baseService = baseService;
    }
    
    public async Task<ResponseDto?> GetCouponAsync(string couponCode)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiTypes.Get,
            Url = SD.CouponAPIBase + "/api/couponAPI/GetByCode/" + couponCode,
        });
    }

    public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiTypes.Put,
            Data = couponDto,
            Url = SD.CouponAPIBase + "/api/couponAPI" ,
        });
    }

    public async Task<ResponseDto?> DeleteCouponAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiTypes.Delete,
            Url = SD.CouponAPIBase + "/api/couponAPI/"+id ,
        });
    }

    public async Task<ResponseDto?> GetAllCouponsAsync()
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiTypes.Get,
            Url = SD.CouponAPIBase + "/api/couponAPI",
        });
    }

    public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiTypes.Post,
            Data = couponDto,
            Url = SD.CouponAPIBase + "/api/couponAPI" ,
        });
    }

    public async Task<ResponseDto?> GetCouponByIdAsync(int id)
    {
        return await _baseService.SendAsync(new RequestDto()
        {
            ApiType = SD.ApiTypes.Get,
            Url = SD.CouponAPIBase + "/api/couponAPI/" + id,
        });
    }
}