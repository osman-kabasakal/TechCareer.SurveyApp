namespace TechCareer.WernerHeisenberg.Survey.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public bool SystemUser { get; set; }
    public bool Deleted { get; set; }

    public List<RoleDto> Roles { get; set; }
}