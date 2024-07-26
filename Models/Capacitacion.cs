using System;
using System.Collections.Generic;

namespace RHCRUD.Models;

public partial class Capacitacion
{
    public int IdCapacitacion { get; set; }

    public string? NombreCapacitacion { get; set; }

    public string? DescripcionCapacitacion { get; set; }

    public DateOnly? FechaCapacitacion { get; set; }

    public virtual ICollection<EmpCap> EmpCaps { get; set; } = new List<EmpCap>();
}
