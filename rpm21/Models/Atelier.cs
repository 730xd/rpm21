using System;
using System.Collections.Generic;

namespace rpm21.Models;

public partial class Atelier
{
    public int Id { get; set; }

    public int Number { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? PhotoPath { get; set; }

    public virtual ICollection<ServicePrice> ServicePrices { get; set; } = new List<ServicePrice>();
}
