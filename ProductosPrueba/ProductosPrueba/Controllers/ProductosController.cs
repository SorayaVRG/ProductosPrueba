using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosPrueba.Data;
using ProductosPrueba.Modelos;

namespace ProductosPrueba.Controllers
{
    public class ProductosController : Controller
    {
        //conexion a la base de datos
        private readonly DataContext _context;

        public ProductosController(DataContext dataContext)
        {
            _context = dataContext;
        }

        //Index
        //Para Categorias
        public async Task<IActionResult> Index(int id)
        {
            var productos = await _context.Productos.Where(t => t.IdCategoria.Equals(id)).ToListAsync();
            var modelCategorias = await _context.Categorias.FindAsync(id);
            ViewBag.Categorias = modelCategorias;
            return View(productos);
        }

        //Create
        public IActionResult Create(int idCategorias)
        {
            var productos = new Productos { IdCategoria = idCategorias };
            return View(productos);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Productos model)
        {
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = model.IdCategoria });
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            var productos = await _context.Productos.FindAsync(id);
            return View(productos);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Productos model)
        {
            var modelOld = await _context.Productos.FindAsync(model.Id);
            modelOld.NombreProducto = model.NombreProducto;
            _context.Update(modelOld);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = model.IdCategoria });
        }

        //Delete
        public async Task<IActionResult> Delete(int id)
        {
            var idCategorias = 0;
            var productos = await _context.Productos.FindAsync(id);
            if (productos != null)
            {
                idCategorias = productos.IdCategoria;
                _context.Remove(productos);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { id = idCategorias });
        }

        //Para Proveedores
        //public async Task<IActionResult> IndexProveedor(int id)
        //{
        //    var productos1 = await _context.Productos.Where(t => t.IdProveedor.Equals(id)).ToListAsync();
        //    var modelProveedores = await _context.Proveedores.FindAsync(id);
        //    ViewBag.Proveedores = modelProveedores;
        //    return View(productos1);
        //}
        
    }
}
