namespace Fatura.Models.Enums
{
    /// <summary>
    /// Enumeración para tipos de transacciones en el historial.
    /// </summary>
    public enum TipoTransaccion
    {
        /// <summary>
        /// Factura creada.
        /// </summary>
        FacturaCreada = 0,
        
        /// <summary>
        /// Factura modificada.
        /// </summary>
        FacturaModificada = 1,
        
        /// <summary>
        /// Factura marcada como pagada.
        /// </summary>
        FacturaPagada = 2,
        
        /// <summary>
        /// Factura cancelada.
        /// </summary>
        FacturaCancelada = 3,
        
        /// <summary>
        /// Cliente registrado.
        /// </summary>
        ClienteCreado = 4,
        
        /// <summary>
        /// Producto/servicio creado.
        /// </summary>
        ProductoCreado = 5,
        
        /// <summary>
        /// Usuario inició sesión.
        /// </summary>
        UsuarioLogin = 6,
        
        /// <summary>
        /// Usuario cerró sesión.
        /// </summary>
        UsuarioLogout = 7
    }
}
