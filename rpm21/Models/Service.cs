using System;
using System.Collections.Generic;

namespace rpm21.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Duration { get; set; }

    public virtual ICollection<ServicePrice> ServicePrices { get; set; } = new List<ServicePrice>();
}
