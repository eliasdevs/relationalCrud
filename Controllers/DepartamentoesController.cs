using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using TestJoin1.DataAccess;

namespace TestJoin1.Controllers
{
    public class DepartamentoesController : Controller
    {
        private readonly TestJoinContext _context;
        private readonly IStringLocalizer<DepartamentoesController> _stringLocalizer;
        private readonly ILogger<DepartamentoesController> _logger;


        public DepartamentoesController(TestJoinContext context, IStringLocalizer<DepartamentoesController> stringLocalizer, ILogger<DepartamentoesController> logger)
        {
            _context = context;
            _stringLocalizer = stringLocalizer;
            _logger = logger;
        }

        // GET: Departamentoes
        public async Task<IActionResult> Index()
        {
            _logger.LogTrace($"{nameof(DepartamentoesController)} - {nameof(Index)} - Tracer Level Log");
            _logger.LogDebug($"{nameof(DepartamentoesController)} - {nameof(Index)} - Debug Level Log");
            _logger.LogInformation($"{nameof(DepartamentoesController)} - {nameof(Index)} - Information Level log");
            _logger.LogWarning($"{nameof(DepartamentoesController)} - {nameof(Index)} - LogWarning Level log");
            _logger.LogError($"{nameof(DepartamentoesController)} - {nameof(Index)} -  LogError Level log");
            _logger.LogCritical($"{nameof(DepartamentoesController)} - {nameof(Index)} - LogCritical Level log");
            ViewBag.VistaAccionCrear = _stringLocalizer["VistaAccionCrear"];
            return View(await _context.Departamentos.ToListAsync());
        }

        // GET: Departamentoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Departamentos == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        // GET: Departamentoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departamentoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departamento);
        }

        // GET: Departamentoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Departamentos == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }

        // POST: Departamentoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Departamento departamento)
        {
            if (id != departamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartamentoExists(departamento.Id))
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
            return View(departamento);
        }

        private void resourcesDelete()
        {
            ViewBag.deseaDelete = _stringLocalizer["deseaDelete"];
            ViewBag.Departamento = _stringLocalizer["Departamento"];
            ViewBag.BackToList = _stringLocalizer["BackToList"];
            ViewBag.VistaAccionEliminar = _stringLocalizer["VistaAccionEliminar"];
        }
        // GET: Departamentoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            resourcesDelete();

            if (id == null || _context.Departamentos == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        // POST: Departamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            resourcesDelete();
            if (_context.Departamentos == null)
            {
                return Problem("Entity set 'TestJoinContext.Departamentos'  is null.");
            }
            var departamento = await _context.Departamentos.FindAsync(id);
            //aqui se va analizar si esta en uso por algun empleado
            var empleados =await _context.Empleados.Include(e => e.Departamento).Where(e => e.Departamento.Id == id).ToListAsync();
            
            if (empleados== null || empleados.Count == 0)//elimina el departamento
            {
                if (departamento != null)
                {
                    _context.Departamentos.Remove(departamento);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    
                    ModelState.AddModelError("", _stringLocalizer["errorDep"]);
                    return View(departamento);
                }

            }
            else
            {
                ModelState.AddModelError("", _stringLocalizer["errorDepUser"]);
                return View(departamento);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DepartamentoExists(int id)
        {
          return _context.Departamentos.Any(e => e.Id == id);
        }
    }
}
