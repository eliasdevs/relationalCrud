﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestJoin1.DataAccess;

namespace TestJoin1.Controllers
{
    public class EmpleadoesController : Controller
    {
        private readonly TestJoinContext _context;

        public EmpleadoesController(TestJoinContext context)
        {
            _context = context;
        }

        // GET: Empleadoes
        public async Task<IActionResult> Index()
        {
            var empleados = await _context.Empleados.Include(e => e.Departamento).ToListAsync();
            return View(empleados);
            
        }

        // GET: Empleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.Include(e=>e.Departamento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleadoes/Create
        public IActionResult Create()
        {
            var departamentos = _context.Departamentos.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Nombre
            }).ToList();
            ViewBag.Departamentos = departamentos;

            return View();
                      
        }

        // POST: Empleadoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Departamento")] Empleado empleado)
        {
                // Buscamos el departamento seleccionado por el usuario en la base de datos
                var departamento = await _context.Departamentos.FindAsync(empleado.Departamento.Id);                
                if (departamento != null)
                {                    
                    empleado.Departamento = departamento;
                    _context.Add(empleado);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }     
            ViewData["Departamentos"] = new SelectList(_context.Departamentos, "Id", "Nombre");
            return View(empleado);
        }


        // GET: Empleadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }            
            var department = _context.Departamentos.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Nombre
            }).ToList();
            ViewData["Departamento"] = department;
            ;
            var empleado = await _context.Empleados.Include(e => e.Departamento).FirstOrDefaultAsync(e => e.Id == id);
            ViewBag.SelectedDepartamento = empleado.Departamento.Id;
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleadoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Departamento")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            
                try
                {
                var departamento = await _context.Departamentos.FindAsync(empleado.Departamento.Id);
                if (departamento.Id != 0)
                {
                    empleado.Departamento = departamento;
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();

                }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            return View(empleado);
        }

        // GET: Empleadoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleados == null)
            {
                return Problem("Entity set 'TestJoinContext.Empleados'  is null.");
            }
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
          return _context.Empleados.Any(e => e.Id == id);
        }
    }
}
