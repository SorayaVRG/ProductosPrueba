namespace ProductosPrueba.Modelos
{
    public class Productos
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public string NombreProducto { get; set;}
        public string Marca { get; set; }
        public string UnidadMedida { get; set; }
        public string Precio { get; set; }
        public string Stock { get; set;}
        public DateTime FechaVencimiento { get; set; }
        public int IdProveedor { get; set; }
        public string DocumentoTecnico { get; set; }
        public string Imagen { get; set; }
        public bool Estado { get; set; }
    }
}
