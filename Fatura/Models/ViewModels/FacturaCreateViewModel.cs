using System.ComponentModel.DataAnnotations;

namespace Fatura.Models.ViewModels
{
    public class FacturaCreateViewModel
    {
        [Required]
        public int ClienteId { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoDocumento { get; set; } = "01";

        [StringLength(20)]
        public string? SerieFactura { get; set; }

        public DateTime? FechaEmision { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        [Required]
        [StringLength(5)]
        public string MonedaSimbolo { get; set; } = "S/";

        public List<FacturaItemCreateViewModel> Items { get; set; } = new();
    }

    public class FacturaItemCreateViewModel
    {
        [Required]
        public int IdProducto { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; } = 1;

        [Range(0, 99999999.99)]
        public decimal Descuento { get; set; } = 0;

        [Range(0, 99999999.99)]
        public decimal PrecioUnitario { get; set; } = 0;
    }
}
