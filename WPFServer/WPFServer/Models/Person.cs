using Microsoft.AspNetCore.Identity;

namespace WPFServer.Models
{
    public class Person : IdentityUser
    {
        public PersonsFiles? Files { get; set; }
        public ICollection<Exercise>? Exercises { get; set; }
    }
}
