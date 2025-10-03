using Microsoft.AspNetCore.Identity;
using WPFServer.DTOs.Person;
using WPFServer.Exceptions;
using WPFServer.Extensions.Mappers;
using WPFServer.Interfaces.Repositories;
using WPFServer.Interfaces.Services;
using WPFServer.Models;

namespace WPFServer.Services.Controllers;

public class AuthenticationService(
    IPersonRepository personRepository,
    IJwtRepository jwtRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IPersonsFilesRepository personsFilesRepository,
    UserManager<Person> userManager,
    SignInManager<Person> signInManager)
    : IAuthenticationService
{
    public async Task<TokensDto> SigninAsync(SigninRequest signinRequest)
    {
        var person = await personRepository.GetByNameAsync(signinRequest.Name!);
        if (person == null) throw new BadRequestException("Person with this name not found");
        
        var result = await signInManager.CheckPasswordSignInAsync(person, signinRequest.Password!, false);
        if (!result.Succeeded) throw new Exception("Incorrect password");

        var token = await jwtRepository.CreateJwtAsync(person);
        var refreshToken = await refreshTokenRepository.CreateNewRefreshTokenAsync(person);

        return new TokensDto
        {
            Jwt = token,
            RefreshToken = refreshToken
        };
    }

    public async Task<TokensDto> SignupAsync(NewPersonRequest newPersonRequest)
    {
        var person = newPersonRequest.ToPerson();
        var createPerson = await userManager.CreateAsync(person, newPersonRequest.Password!);

        if (!createPerson.Succeeded)
            throw new UsernameAlreadyExistsException(person.UserName!);
            
        var roleResult = await userManager.AddToRoleAsync(person, "User");
        if (!roleResult.Succeeded)
            throw new Exception("failed to issue license");
            
        await personsFilesRepository.CreateNewAsync(person.Id);

        var token = await jwtRepository.CreateJwtAsync(person);
        var refreshToken = await refreshTokenRepository.CreateNewRefreshTokenAsync(person);

        return new TokensDto 
        {
            Jwt = token,
            RefreshToken = refreshToken
        };
    }

    public async Task<bool> LogoutAsync(string id)
    {
        return await refreshTokenRepository.DeleteRefreshToken(id) ?
                true : throw new NotFoundException($"Person {id} not found");
    }
}