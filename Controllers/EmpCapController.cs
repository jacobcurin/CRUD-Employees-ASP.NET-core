using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RHCRUD.Models;

namespace RHCRUD.Controllers
{
    public class EmpCapController : Controller
    {
        private readonly RhcrudContext _context;

        public EmpCapController(RhcrudContext context)
        {
            _context = context;
        }

        // GET: EmpCap
        public async Task<IActionResult> Index()
        {
            var rhcrudContext = _context.EmpCaps.Include(e => e.IdCapacitacionNavigation).Include(e => e.RutEmpleadoNavigation);
            return View(await rhcrudContext.ToListAsync());
        }

        // GET: EmpCap/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empCap = await _context.EmpCaps
                .Include(e => e.IdCapacitacionNavigation)
                .Include(e => e.RutEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpeCap == id);
            if (empCap == null)
            {
                return NotFound();
            }

            return View(empCap);
        }

        // Json para Obtener capacitaciones
        [HttpGet]
        public async Task<IActionResult> GetCapacitaciones()
        {
            var capacitaciones = await _context.Capacitacions.Select(c => new {
                c.IdCapacitacion,
                c.NombreCapacitacion
            }).ToListAsync();
            return Json(capacitaciones);
        }

        // GET: EmpCap/Create
        public IActionResult Create()
        {
            ViewData["RutEmpleado"] = new SelectList(_context.Empleados, "RutEmpleado", "NombreEmpleado");
            ViewBag.Capacitaciones = new MultiSelectList(_context.Capacitacions.ToList(), "IdCapacitacion", "NombreCapacitacion");
            return View();
        }

        // POST: EmpCap/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string RutEmpleado, List<int> IdCapacitacion, List<string> FechaEmpCaps)
        {
            if (ModelState.IsValid)
            {
                if (IdCapacitacion != null && IdCapacitacion.Any() && FechaEmpCaps != null && FechaEmpCaps.Any())
                {
                    for (int i = 0; i < IdCapacitacion.Count; i++)
                    {
                        var idCapacitacion = IdCapacitacion[i];
                        var fechaString = FechaEmpCaps[i];

                        if (DateOnly.TryParseExact(fechaString, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly fecha))
                        {
                            var empCap = new EmpCap
                            {
                                RutEmpleado = RutEmpleado,
                                IdCapacitacion = idCapacitacion,
                                FechaEmpCap = fecha
                            };

                            _context.Add(empCap);
                        }
                        else
                        {
                            ModelState.AddModelError("", $"La fecha '{fechaString}' para la capacitación {idCapacitacion} es inválida");
                        }
                    }
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    ModelState.AddModelError("", "Debe seleccionar al menos una capacitación y proporcionar una fecha para cada una.");
                }
                
            }
            ViewData["RutColaborador"] = new SelectList(_context.Empleados, "RutColaborador", "NombreColaborador", RutEmpleado);
            ViewBag.Capacitaciones = new MultiSelectList(_context.Capacitacions.ToList(), "IdCapacitacion", "NombreCapacitacion", IdCapacitacion);
            return View();
            
        }

        // GET: EmpCap/CreateEC
        public IActionResult CreateEC(int id)
        {
            ViewData["IdCapacitacion"] = id;
            ViewBag.Empleados = new SelectList(_context.Empleados, "RutEmpleado", "NombreEmpleado");
            return View();
        }

        // POST: EmpCap/CreateEC
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEC(int IdCapacitacion, List<string> RutEmpleado, List<string> FechaEmpCaps)
        {
            if (ModelState.IsValid)
            {
                if (RutEmpleado != null && RutEmpleado.Any() && FechaEmpCaps != null && FechaEmpCaps.Any())
                {
                    for (int i = 0; i < RutEmpleado.Count; i++)
                    {
                        var rutEmpleado = RutEmpleado[i];
                        var fechaString = FechaEmpCaps[i];

                        if (DateOnly.TryParseExact(fechaString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly fecha))
                        {
                            var empCap = new EmpCap
                            {
                                IdCapacitacion = IdCapacitacion,
                                RutEmpleado = rutEmpleado,
                                FechaEmpCap = fecha
                            };

                            _context.Add(empCap);
                        }
                        else
                        {
                            ModelState.AddModelError("", $"La fecha '{fechaString}' para el empleado {rutEmpleado} es inválida");
                        }
                    }
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Debe seleccionar al menos un empleado y proporcionar una fecha para cada uno.");
                }
            }
            ViewData["IdCapacitacion"] = IdCapacitacion;
            ViewBag.Empleados = new SelectList(_context.Empleados, "RutEmpleado", "NombreEmpleado");
            return View();
        }

        // GET: EmpCap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empCap = await _context.EmpCaps.FindAsync(id);
            if (empCap == null)
            {
                return NotFound();
            }
            ViewData["IdCapacitacion"] = new SelectList(_context.Capacitacions, "IdCapacitacion", "NombreCapacitacion", empCap.IdCapacitacion);
            ViewData["RutEmpleado"] = new SelectList(_context.Empleados, "RutEmpleado", "NombreEmpleado", empCap.RutEmpleado);
            return View(empCap);
        }

        // POST: EmpCap/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpeCap,RutEmpleado,IdCapacitacion,FechaEmpCap")] EmpCap empCap)
        {
            if (id != empCap.IdEmpeCap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empCap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpCapExists(empCap.IdEmpeCap))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCapacitacion"] = new SelectList(_context.Capacitacions, "IdCapacitacion", "NombreCapacitacion", empCap.IdCapacitacion);
            ViewData["RutEmpleado"] = new SelectList(_context.Empleados, "RutEmpleado", "NombreEmpleado", empCap.RutEmpleado);
            return View(empCap);
        }
        
        // GET: EmpCap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empCap = await _context.EmpCaps
                .Include(e => e.IdCapacitacionNavigation)
                .Include(e => e.RutEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpeCap == id);
            if (empCap == null)
            {
                return NotFound();
            }

            return View(empCap);
        }

        // POST: EmpCap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empCap = await _context.EmpCaps.FindAsync(id);
            if (empCap != null)
            {
                _context.EmpCaps.Remove(empCap);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpCapExists(int id)
        {
            return _context.EmpCaps.Any(e => e.IdEmpeCap == id);
        }
    }
}
