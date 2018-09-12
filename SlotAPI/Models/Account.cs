using System.ComponentModel.DataAnnotations;

namespace SlotAPI.Models
{
    public class Account
    {
        [Key]
        public int PlayerId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Token { get; set; }

    }
}
