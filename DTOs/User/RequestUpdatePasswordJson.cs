using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs.User;

public class RequestUpdatePasswordJson
{
    [Required]
    [StringLength(8)]
    public string CurrentPassword { get; set; }

    [Required]
    [StringLength(8)]
    public string NewPassword { get; set; }
}
