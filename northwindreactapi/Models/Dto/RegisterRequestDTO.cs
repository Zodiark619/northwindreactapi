using System.ComponentModel.DataAnnotations;

namespace northwindreactapi.Models.Dto
{
    public class RegisterRequestDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]

        public string Password { get; set; } = string.Empty;

    }
}
