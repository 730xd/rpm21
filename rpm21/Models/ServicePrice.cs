using System;
using System.Collections.Generic;

namespace rpm21.Models;

public partial class ServicePrice
{
    public int Id { get; set; }

    public int AtelierId { get; set; }

    public int ServiceId { get; set; }

    public decimal Price { get; set; }

    public virtual Atelier Atelier { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
