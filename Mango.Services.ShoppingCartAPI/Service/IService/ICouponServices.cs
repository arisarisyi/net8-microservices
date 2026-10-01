using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Service.IService;

public interface ICouponServices
{
    Task<CouponDto> GetCoupon(string couponCode);
}