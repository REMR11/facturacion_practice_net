using Microsoft.AspNetCore.Mvc.Razor;

namespace Fatura.Infrastructure;

/// <summary>
/// Permite que ClientesController encuentre vistas en Views/Cliente/
/// (la carpeta usa singular "Cliente" mientras el controlador se llama "Clientes").
/// </summary>
public class ClienteViewLocationExpander : IViewLocationExpander
{
    public void PopulateValues(ViewLocationExpanderContext context) { }

    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
    {
        if (string.Equals(context.ControllerName, "Clientes", StringComparison.OrdinalIgnoreCase))
        {
            return viewLocations.Concat(new[] { "/Views/Cliente/{0}.cshtml", "/Views/Cliente/Shared/{0}.cshtml" });
        }
        return viewLocations;
    }
}
