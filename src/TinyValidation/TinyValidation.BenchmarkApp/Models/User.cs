namespace TinyValidation.BenchmarkApp.Models;


public sealed class User
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
}
