using DirectoryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DirectoryApi
{
    public class Directory
    {
        public int Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }

        [RegularExpression(@"^[0-9]{1,10}$", ErrorMessage = "Telefon numarası sadece rakam ve en fazla 9 haneli olmalıdır.")]
        public string Telephone { get; set; }
        public int  UserId { get; set; }
        public User? User { get; set; }

    }
}
