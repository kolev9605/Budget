namespace Budget.Domain.Models.Admin;

public class ChangeUserRoleRequestModel
{
    public string UserId { get; set; } = null!;

    public string RoleName { get; set; } = null!;
}
