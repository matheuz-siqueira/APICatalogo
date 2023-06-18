using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs.User;

public class RequestUpdatePasswordJson
{
    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    public string NewPassword { get; set; }
}
