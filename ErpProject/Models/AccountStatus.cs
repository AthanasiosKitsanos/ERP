namespace ErpProject.Models;

public class AccountStatus
{
    public int Id { get; set; }

    public int SelectedStatus { get; set; }
    public List<AccountStatus> StatusList { get; set; } = new List<AccountStatus>();
}
