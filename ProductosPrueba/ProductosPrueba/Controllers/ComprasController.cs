using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductosPrueba.Data;
using ProductosPrueba.Modelos;

namespace ProductosPrueba.Controllers
{
    public class ComprasController : Controller
    {
        //conexion a la base de datos
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ComprasController(DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> BuscarTrabajador(string busqueda)
        {
            var proveedores = await _context.Trabajadores.Where(t => t.Nombres.Contains(busqueda)).ToListAsync();
            return PartialView(proveedores);
        }

        //GET : /<Controller>/
        [HttpGet]
        public IActionResult Index()
        {

            var compras = _context.PR_COMPRAS_S01.FromSqlRaw("exec PR_COMPRAS_S01");
            return View(compras);
        }


        //CREATE ACTUALIZADO
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var Zonas = await _context.Zonas.ToListAsync();
            ViewData["IdZona"] = new SelectList(Zonas, "Id", "NombreZona");

            var productos = new Compras { FechaVencimiento = DateTime.Now.Date };
            return PartialView(productos);

            /*
             ViewBag de Departamento
             ViewBag de Tipos de documento
            */
            // return View();
        }

        [HttpGet]
        public async Task<JsonResult> CargarCategorias(int id)
        {
            var listado = await _context.Categorias.Where(t => t.IdZona.Equals(id)).ToListAsync();
            return Json(listado);
        }

        [HttpGet]
        public async Task<JsonResult> CargarProductos(int id)
        {
            var listado = await _context.Productos.Where(t => t.IdCategoria.Equals(id)).ToListAsync();
            return Json(listado);
        }

        //CREAR NUEVO REGISTRO
        [HttpPost]
        public async Task<IActionResult> Create(Compras model)
        {
            //PARA CREAR DOCUMENTO TECNICO

            if (model.BoletaIFormFile != null)
            {
                model.Boleta = await CargarDocumento(model.BoletaIFormFile, "Boleta");
            }

            model.IdTrabajador = 2;
            _context.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //PARA CARGAR FICHA
        private async Task<string> CargarDocumento(IFormFile BoletaIFormFile, string ruta)
        {
            var guid = Guid.NewGuid().ToString();
            var fileName = guid + Path.GetExtension(BoletaIFormFile.FileName);
            //Obtengo extension del documento
            var carga1 = Path.Combine(_webHostEnvironment.WebRootPath, "images", ruta);
            /*var carga = Path.Combine(_webHostEnvironment.WebRootPath, string.Format("images\\{0}", ruta));*/
            using (var fileStream = new FileStream(Path.Combine(carga1, fileName), FileMode.Create))
            {
                await BoletaIFormFile.CopyToAsync(fileStream);
            }
            return string.Format("/images/{0}/{1}", ruta, fileName);
        }

        //Edit
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _context.Productos.FindAsync(id);

            var Zonas = await _context.Zonas.ToListAsync();
            ViewBag.IdZona = new SelectList(Zonas, "Id", "NombreZona", model.IdZona);

            var Categorias = await _context.Categorias.Where(t => t.IdZona.Equals(model.IdZona)).ToListAsync();
            ViewBag.IdCategoria = new SelectList(Categorias, "Id", "NombreCategoria", model.IdCategoria);

            /*
             ViewBag de Departamento
             ViewBag de Tipos de documento
            */

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Productos model)
        {
            var modelOld = await _context.Productos.FindAsync(model.Id);

            modelOld.IdZona = model.IdZona; // Actualizar el campo IdZona
            modelOld.IdCategoria = model.IdCategoria; // Actualizar el campo IdCategoria
            modelOld.NombreProducto = model.NombreProducto; // Actualizar el campo NombreProducto
            modelOld.Marca = model.Marca; // Actualizar el campo Marca
            modelOld.UnidadMedida = model.UnidadMedida; // Actualizar el campo UnidadMedida
            modelOld.Precio = model.Precio; // Actualizar el campo Precio
            modelOld.Stock = model.Stock; // Actualizar el campo Stock
            modelOld.FechaVencimiento = model.FechaVencimiento; // Actualizar el campo FechaVencimiento
            modelOld.IdProveedor = model.IdProveedor; // Actualizar el campo IdProveedor

            // Actualizar DocumentoTecnico si se proporciona un nuevo archivo
            if (model.DocumentoTecnicoIFormFile != null)
            {
                // Eliminar el DocumentoTecnico anterior si existe
                if (!string.IsNullOrEmpty(modelOld.DocumentoTecnico))
                {
                    var DocumentoTecnicoPath = Path.Combine(_webHostEnvironment.WebRootPath, modelOld.DocumentoTecnico.TrimStart('/'));
                    if (System.IO.File.Exists(DocumentoTecnicoPath))
                    {
                        System.IO.File.Delete(DocumentoTecnicoPath);
                    }
                }
                // Cargar el nuevo DocumentoTecnico
                modelOld.DocumentoTecnico = await CargarDocumento(model.DocumentoTecnicoIFormFile, "Ficha");
            }

            // Actualizar imagen si se proporciona un nuevo archivo
            if (model.ImagenIFormFile != null)
            {
                // Eliminar la imagen anterior si existe
                if (!string.IsNullOrEmpty(modelOld.Imagen))
                {
                    var ImagenPath = Path.Combine(_webHostEnvironment.WebRootPath, modelOld.Imagen.TrimStart('/'));
                    if (System.IO.File.Exists(ImagenPath))
                    {
                        System.IO.File.Delete(ImagenPath);
                    }
                }

                // Cargar la nueva foto
                modelOld.Imagen = await CargarDocumento(model.ImagenIFormFile, "Foto");
            }

            _context.Update(modelOld);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //PARA ACTIVAR Y DESACTIVAR PRODUCTOS
        public async Task<IActionResult> Activar(int id)
        {
            var productos = await _context.Productos.FindAsync(id);
            productos.Estado = true; // Establecer el estado como activado
            _context.Update(productos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Anular PROVEEDORES method GET
        public async Task<IActionResult> Anular(int id)
        {
            var productos = await _context.Productos.FindAsync(id);
            productos.Estado = false; // Establecer el estado como anulado
            _context.Update(productos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}

//Para Proveedores
//public async Task<IActionResult> IndexProveedor(int id)
//{
//    var productos1 = await _context.Productos.Where(t => t.IdProveedor.Equals(id)).ToListAsync();
//    var modelProveedores = await _context.Proveedores.FindAsync(id);
//    ViewBag.Proveedores = modelProveedores;
//    return View(productos1);
//}