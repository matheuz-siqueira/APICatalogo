using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.DTOs.User;

public class RequestLoginJson
{
    [Required]
    [StringLength(100)]
    public string Email { get; set; }
    
    [Required]
    [StringLength(60)]
    public string Password { get; set; }
}
