using System;

namespace ErpProject.Models;

public class Customer
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
