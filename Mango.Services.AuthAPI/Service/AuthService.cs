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
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenGenerator = jwtTokenGenerator;
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
        var user = _context.ApplicationUsers.FirstOrDefault(u =>
            u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

        bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

        if (user == null || isValid == false)
        {
            return new LoginResponseDto() { User = null, Token = "" };
        }

        UserDto userDto = new()
        {
            Email = user.Email,
            Name = user.Name,
            ID = user.Id,
            PhoneNumber = user.PhoneNumber,
        };

        var roles = await _userManager.GetRolesAsync(user);
        LoginResponseDto loginResponseDto = new LoginResponseDto()
        {
            User = userDto,
            Token = _jwtTokenGenerator.GenerateToken(user, roles)
        };
        
        return loginResponseDto;
    }

    public async Task<bool> AssignRoleAsync(string email, string roleName)
    {
        var user = _context.ApplicationUsers.FirstOrDefault(u =>
            u.Email.ToLower() == email.ToLower());
        if (user != null)
        {
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                //Create role if it doesn't exist
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }
        return false;
    }
}