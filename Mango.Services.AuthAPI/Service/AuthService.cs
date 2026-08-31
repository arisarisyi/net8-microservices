using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    
    public async Task<string> RegisterAsync(RegistrationRequestDto registrationRequestDto)
    {
        ApplicationUser user = new()
        {
            Email = registrationRequestDto.Email,
            UserName = registrationRequestDto.Email,
            NormalizedEmail = registrationRequestDto.Email.ToUpper(),
            Name = registrationRequestDto.Name,
            PhoneNumber = registrationRequestDto.PhoneNumber,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            if (result.Succeeded){
                var userToReturn = _context.Users.FirstOrDefault(x => x.Email == registrationRequestDto.Email);

                UserDto userDto = new()
                {
                    Email = userToReturn.Email,
                    Name = userToReturn.Name,
                    PhoneNumber = userToReturn.PhoneNumber,
                    ID = userToReturn.Id
                };
                
                return "";
            }
            else
            {
                return result.Errors.FirstOrDefault()?.Description;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
    {
        throw new NotImplementedException();
    }
}