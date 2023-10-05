namespace TechCareer.WernerHeisenberg.Survey.Application.DTOs;

public class RoleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Deleted { get; set; }
    public bool SystemRole { get; set; }
    public List<UserDto> AssignedUsers { get; set; }

}