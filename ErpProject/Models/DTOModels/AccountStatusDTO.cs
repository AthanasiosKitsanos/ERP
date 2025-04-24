using ErpProject.Models.AccountStatusModel;

namespace ErpProject.Models.DTOModels;

public class AccountStatusDTO
{
    public int Id { get; set; }

    public int SelectedStatus { get; set; }
    public List<AccountStatus> StatusList { get; set; } = new List<AccountStatus>();
}
