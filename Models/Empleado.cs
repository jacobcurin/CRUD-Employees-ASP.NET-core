using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RHCRUD.Models;

public partial class Empleado
{
    [Required(ErrorMessage ="Este campo es obligatorio")]
    public string RutEmpleado { get; set; } = null!;

    public string? NombreEmpleado { get; set; }

    public string? ApPaternoEmpleado { get; set; }

    public string? ApMaternoEmpleado { get; set; }

    public string? DireccionEmpleado { get; set; }

    public string? CorreoEmpleado { get; set; }

    public string? SexoEmpleado { get; set; }

    public string? EstadoCivilEmpleado { get; set; }

    public virtual ICollection<EmpCap> EmpCaps { get; set; } = new List<EmpCap>();
}
