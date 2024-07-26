using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RHCRUD.Models;

public partial class RhcrudContext : DbContext
{
    public RhcrudContext()
    {
    }

    public RhcrudContext(DbContextOptions<RhcrudContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Capacitacion> Capacitacions { get; set; }

    public virtual DbSet<EmpCap> EmpCaps { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            //                optionsBuilder.UseSqlServer("server=localhost; database=MVCCRUD; integrated security=true;");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Capacitacion>(entity =>
        {
            entity.HasKey(e => e.IdCapacitacion).HasName("PK__CAPACITA__9574E3F3FAE771EB");

            entity.ToTable("CAPACITACION");

            entity.Property(e => e.IdCapacitacion).HasColumnName("ID_CAPACITACION");
            entity.Property(e => e.DescripcionCapacitacion)
                .HasColumnType("text")
                .HasColumnName("DESCRIPCION_CAPACITACION");
            entity.Property(e => e.FechaCapacitacion).HasColumnName("FECHA_CAPACITACION");
            entity.Property(e => e.NombreCapacitacion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_CAPACITACION");
        });

        modelBuilder.Entity<EmpCap>(entity =>
        {
            entity.HasKey(e => e.IdEmpeCap).HasName("PK__EMP_CAP__6BAD76D473E069C8");

            entity.ToTable("EMP_CAP");

            entity.Property(e => e.IdEmpeCap).HasColumnName("ID_EMPE_CAP");
            entity.Property(e => e.IdCapacitacion).HasColumnName("ID_CAPACITACION");
            entity.Property(e => e.FechaEmpCap).HasColumnName("FECHA_EMP_CAP");             
            entity.Property(e => e.RutEmpleado)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("RUT_EMPLEADO");

            entity.HasOne(d => d.IdCapacitacionNavigation).WithMany(p => p.EmpCaps)
                .HasForeignKey(d => d.IdCapacitacion)
                .HasConstraintName("FK_CAPACITACION_EMP_CAP");

            entity.HasOne(d => d.RutEmpleadoNavigation).WithMany(p => p.EmpCaps)
                .HasForeignKey(d => d.RutEmpleado)
                .HasConstraintName("FK_EMPLEADO_EMP_CAP");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.RutEmpleado).HasName("PK__EMPLEADO__D1B531C099C4C85B");

            entity.ToTable("EMPLEADO");

            entity.Property(e => e.RutEmpleado)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("RUT_EMPLEADO");
            entity.Property(e => e.ApMaternoEmpleado)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AP_MATERNO__EMPLEADO");
            entity.Property(e => e.ApPaternoEmpleado)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AP_PATERNO_EMPLEADO");
            entity.Property(e => e.CorreoEmpleado)
                .HasMaxLength(360)
                .IsUnicode(false)
                .HasColumnName("CORREO_EMPLEADO");
            entity.Property(e => e.DireccionEmpleado)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DIRECCION_EMPLEADO");
            entity.Property(e => e.EstadoCivilEmpleado)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ESTADO_CIVIL_EMPLEADO");
            entity.Property(e => e.NombreEmpleado)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NOMBRE_EMPLEADO");
            entity.Property(e => e.SexoEmpleado)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SEXO_EMPLEADO");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
