using WPFServer.DTOs.Person;
using WPFServer.Models;

namespace WPFServer.Extensions.Mappers
{
    public static class PersonMapper
    {
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
                Name = person.UserName,
                Email = person.Email,
                Token = token
            };
        }

        public static LightPersonDto ToLightPersonDto(this Person person)
        {
            return new LightPersonDto
            {
                Id = person.Id,
                Name = person.UserName
            };
        }

        public static PrivatePersonDto ToPrivatePersonDto(this Person person)
        {
            return new PrivatePersonDto
            {
                Id = person.Id,
                Name = person.UserName,
                Email = person.Email
            };
        }

        public static FullPersonDto ToFullPersonDto(this Person person)
        {
            return new FullPersonDto
            {
                Id = person.Id,
                Name = person.UserName,
                Image = person.Files?.Image,
                Exercises = person.Exercises?.Select(x => x.ToExerciseDto(false)).ToList()
            };
        }

        public static FullPrivatePersonDto ToFullPrivatePersonDto(this Person person)
        {
            return new FullPrivatePersonDto
            {
                Id = person.Id,
                Name = person.UserName,
                Email = person.Email,
                Image = person.Files?.Image,
                Exercises = person.Exercises?.Select(x => x.ToExerciseDto(true)).ToList()
            };
        }
    }
}
