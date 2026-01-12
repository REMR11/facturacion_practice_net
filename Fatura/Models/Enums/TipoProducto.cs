namespace Fatura.Models.Enums
{
    /// <summary>
    /// Enumeración para distinguir entre productos físicos y servicios.
    /// </summary>
    public enum TipoProducto
    {
        /// <summary>
        /// Producto físico con inventario.
        /// </summary>
        Producto = 0,
        
        /// <summary>
        /// Servicio sin inventario.
        /// </summary>
        Servicio = 1
    }
}
