using Microsoft.VisualBasic;
using System.Reflection;
using System.Reflection.Metadata;

namespace ProductosPrueba.Modelos
{
    public class Proveedores
    {
        public int Id { get; set; }
        public string Ruc { get; set; }
        public string NombreProveedor { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        //public string Estado { get; set; }
        public bool Estado { get; set; }

    }
}
