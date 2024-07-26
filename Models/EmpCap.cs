using System;
using System.Collections.Generic;

namespace RHCRUD.Models;

public partial class EmpCap
{
    public int IdEmpeCap { get; set; }

    public string? RutEmpleado { get; set; }

    public int? IdCapacitacion { get; set; }
    public DateOnly? FechaEmpCap { get; set; }

    public virtual Capacitacion? IdCapacitacionNavigation { get; set; }

    public virtual Empleado? RutEmpleadoNavigation { get; set; }
}
