using System.ComponentModel.DataAnnotations;

namespace usecase_01_call_external_api.Models;

public class User
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Website { get; set; }
}