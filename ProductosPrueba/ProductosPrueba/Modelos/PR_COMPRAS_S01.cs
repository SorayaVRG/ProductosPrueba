using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace ProductosPrueba.Modelos
{
    public class PR_COMPRAS_S01
    {
        [DisplayName("IdCompras")]
        public int Id { get; set; }

        [DisplayName("IdZona")]
        public int IdZona { get; set; }

        [DisplayName("NombreZona")]
        public string NombreZona { get; set; }

        [DisplayName("IdCategoria")]
        public int IdCategoria { get; set; }

        [DisplayName("NombreCategoria")]
        public string NombreCategoria { get; set; }

        [DisplayName("IdProducto")]
        public int IdProducto { get; set; }

        [DisplayName("NombreProducto")]
        public string NombreProducto { get; set; }

        [DisplayName("IdTrabajador")]
        public int IdTrabajador { get; set; }

        [DisplayName("NombreTrabajador")]
        public string NombreTrabajador { get; set; }

        [DisplayName("TipoDocumento")]
        public string TipoDocumento { get; set; }

        [DisplayName("NumeroDocumento")]
        public string NumeroDocumento { get; set; }

        [DisplayName("Sexo")]
        public string Sexo { get; set; }

        [DisplayName("Cantidad")]
        public int Cantidad { get; set; }

        [DisplayName("PrecioUnitario")]
        public decimal PrecioUnitario { get; set; }

        [DisplayName("PrecioFinal")]
        public decimal PrecioFinal { get; set; }

        [DisplayName("Boleta")]
        public string Boleta { get; set; }
        [NotMapped]
        public IFormFile BoletaIFormFile { get; set; }
        public string BoletaURL => Boleta == null ? "" : Boleta;

        public string BoletaURL2
        {
            get
            {
                if (string.IsNullOrEmpty(Boleta))
                {
                    return "";
                }
                else
                {
                    return $"https://localhost:7083/{Boleta}";
                }
            }
        }

        [DisplayName("Estado")]
        public bool Estado { get; set; }

        [DisplayName("Observaciones")]
        public string? Observaciones { get; set; }
        

        [NotMapped]
        public string TrabajadorDescripcion { get; set; }
    }
}
