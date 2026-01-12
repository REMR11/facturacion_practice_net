using Fatura.Models.Core;

namespace Fatura.Models.Identity
{
    /// <summary>
    /// Configuración personalizada del dashboard por usuario.
    /// Permite a cada usuario personalizar qué widgets y gráficos ver.
    /// </summary>
    public partial class ConfiguracionDashboard : BaseEntity
    {
        public int IdConfiguracion { get; set; }
        
        /// <summary>
        /// ID del usuario propietario de esta configuración.
        /// </summary>
        public int IdUsuario { get; set; }
        
        /// <summary>
        /// Mostrar widget de ingresos del mes.
        /// </summary>
        public bool MostrarIngresosMes { get; set; } = true;
        
        /// <summary>
        /// Mostrar widget de facturas emitidas.
        /// </summary>
        public bool MostrarFacturasEmitidas { get; set; } = true;
        
        /// <summary>
        /// Mostrar widget de clientes activos.
        /// </summary>
        public bool MostrarClientesActivos { get; set; } = true;
        
        /// <summary>
        /// Mostrar widget de tasa de cobro.
        /// </summary>
        public bool MostrarTasaCobro { get; set; } = true;
        
        /// <summary>
        /// Mostrar gráfico de ingresos mensuales.
        /// </summary>
        public bool MostrarIngresosMensuales { get; set; } = true;
        
        /// <summary>
        /// Mostrar tabla de facturas recientes.
        /// </summary>
        public bool MostrarFacturasRecientes { get; set; } = true;
        
        /// <summary>
        /// Número de meses a mostrar en el gráfico de ingresos.
        /// </summary>
        public int PeriodoGraficoMeses { get; set; } = 7;

        /// <summary>
        /// Relación con el usuario propietario.
        /// </summary>
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
