namespace Employees.Domain;

public enum FormMode
{
    View,
    Update,
    Create
}

public class DetailsModel
{
    public int Id { get; set; }
    public string Aft { get; set; } = string.Empty;
    public FormMode Mode { get; set; } = FormMode.View;
}
