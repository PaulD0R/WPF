using WPFServer.DTOs.Person;
using WPFServer.Models;

namespace WPFServer.Extensions.Mappers
{
    public static class PersonMapper
    {
        public static Person ToPerson(this UpdatePersonRequest updatePersonRequest)
        {
            return new Person
            {
                UserName = updatePersonRequest.Name,
                Email = updatePersonRequest.Email
            };
        }

        public static Person ToPerson(this NewPersonRequest newPersonRequest)
        {
            return new Person {
                UserName = newPersonRequest.Name,
                Email = newPersonRequest.Email,
            };
        }

        public static PersonDto ToPersonDto(this Person person, string token)
        {
            return new PersonDto
            {
                Id = person.Id,
                Name = person.UserName ?? string.Empty,
                Email = person.Email ?? string.Empty,
                Token = token
            };
        }

        public static LitePersonDto ToLitePersonDto(this Person person)
        {
            return new LitePersonDto
            {
                Id = person.Id,
                Name = person.UserName ?? string.Empty
            };
        }

        public static PrivatePersonDto ToPrivatePersonDto(this Person person)
        {
            return new PrivatePersonDto
            {
                Id = person.Id,
                Name = person.UserName ?? string.Empty,
                Email = person.Email ?? string.Empty
            };
        }

        public static FullPersonDto ToFullPersonDto(this Person person)
        {
            return new FullPersonDto
            {
                Id = person.Id,
                Name = person.UserName ??  string.Empty,
                Image = person.Files.Image ?? []
            };
        }

        public static FullPrivatePersonDto ToFullPrivatePersonDto(this Person person)
        {
            return new FullPrivatePersonDto
            {
                Id = person.Id,
                Name = person.UserName ??  string.Empty,
                Email = person.Email ??  string.Empty,
                Image = person.Files.Image ?? []
            };
        }
    }
}
