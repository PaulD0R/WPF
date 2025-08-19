using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WPFServer.Models
{
    [Index(nameof(UserName), IsUnique = true)]
    public class Person : IdentityUser
    {
        public PersonsFiles? Files { get; set; }
        public ICollection<Exercise>? Exercises { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
