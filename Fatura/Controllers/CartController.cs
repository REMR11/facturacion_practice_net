using Fatura.Models;
using Fatura.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Fatura.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly IFacturaService _facturaService;
        private readonly IClienteService _clienteService;
        private readonly xstoreContext _context;
        private readonly IConfiguration _config;

        public CartController(
            IProductoService productoService,
            IFacturaService facturaService,
            IClienteService clienteService,
            xstoreContext context,
            IConfiguration config)
        {
            _productoService = productoService;
            _facturaService = facturaService;
            _clienteService = clienteService;
            _context = context;
            _config = config;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            ViewBag.CuentaBancaria = _config["Pagos:CuentaBancaria"] ?? "";
            ViewBag.NombreTitular = _config["Pagos:NombreTitular"] ?? "Motos Rodriguez";
            ViewBag.Banco = _config["Pagos:Banco"] ?? "";
            ViewBag.NotaTarjeta = _config["Pagos:NotaTarjeta"] ?? "El pago con tarjeta se acredita a la cuenta del negocio.";
            return View(cart);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var cart = GetCart();
            return Json(new { count = cart.Items.Sum(x => x.Cantidad) });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest? request)
        {
            try
            {
                var cantidad = (request?.Cantidad ?? 0) < 1 ? 1 : (request?.Cantidad ?? 1);
                var productoId = request?.ProductoId ?? 0;

                if (productoId <= 0)
                {
                    return Json(new { success = false, message = "Datos inválidos", cartCount = 0 });
                }

                var producto = await _productoService.GetByIdAsync(productoId);
                if (producto == null)
                {
                    return Json(new { success = false, message = "Producto no encontrado", cartCount = 0 });
                }

                var cart = GetCart();
                var existingItem = cart.Items.FirstOrDefault(x => x.ProductoId == productoId);

                if (existingItem != null)
                {
                    existingItem.Cantidad += cantidad;
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductoId = producto.IdProducto,
                        Nombre = producto.NombreProducto ?? "",
                        Precio = producto.Precio ?? 0,
                        Cantidad = cantidad,
                        Imagen = GetProductImage(producto.NombreProducto ?? "")
                    });
                }

                SaveCart(cart);
                var totalItems = cart.Items.Sum(x => x.Cantidad);
                return Json(new { success = true, message = "Producto agregado al carrito", cartCount = totalItems });
            }
            catch (Exception)
            {
                try
                {
                    var cart = GetCart();
                    return Json(new { success = false, message = "Intenta de nuevo", cartCount = cart.Items.Sum(x => x.Cantidad) });
                }
                catch
                {
                    return Json(new { success = false, message = "Intenta de nuevo", cartCount = 0 });
                }
            }
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productoId, int cantidad)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ProductoId == productoId);

            if (item != null)
            {
                if (cantidad <= 0)
                {
                    cart.Items.Remove(item);
                }
                else
                {
                    item.Cantidad = cantidad;
                }
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult RemoveItem(int productoId)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(x => x.ProductoId == productoId);
            if (item != null)
            {
                cart.Items.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(
            string metodoPago,
            string nombreCliente,
            string nitCliente,
            string? direccionCliente,
            string? telefonoCliente,
            string? emailCliente,
            string? nombreEnTarjeta,
            string? ultimos4Digitos,
            string? referenciaTarjeta)
        {
            try
            {
                var cart = GetCart();
                if (cart.Items.Count == 0)
                {
                    TempData["Error"] = "El carrito está vacío";
                    return RedirectToAction(nameof(Index));
                }

                // Buscar o crear cliente
                var cliente = await _clienteService.GetClientesActivosAsync();
                var clienteExistente = !string.IsNullOrWhiteSpace(nitCliente)
                    ? cliente.FirstOrDefault(c => c.NitDui == nitCliente.Trim())
                    : null;

                if (clienteExistente == null && (!string.IsNullOrWhiteSpace(nitCliente) || !string.IsNullOrWhiteSpace(nombreCliente)))
                {
                    clienteExistente = new Models.Facturacion.Cliente
                    {
                        Nombre = string.IsNullOrWhiteSpace(nombreCliente) ? "Cliente General" : nombreCliente.Trim(),
                        NitDui = string.IsNullOrWhiteSpace(nitCliente) ? "00000000-0" : nitCliente.Trim(),
                        Direccion = string.IsNullOrWhiteSpace(direccionCliente) ? null : direccionCliente.Trim(),
                        Telefono = string.IsNullOrWhiteSpace(telefonoCliente) ? null : telefonoCliente.Trim(),
                        Email = string.IsNullOrWhiteSpace(emailCliente) ? null : emailCliente.Trim(),
                        Activo = true
                    };
                    await _clienteService.CreateAsync(clienteExistente);
                }
                else if (clienteExistente != null && (!string.IsNullOrWhiteSpace(direccionCliente) || !string.IsNullOrWhiteSpace(telefonoCliente) || !string.IsNullOrWhiteSpace(emailCliente)))
                {
                    if (!string.IsNullOrWhiteSpace(direccionCliente)) clienteExistente.Direccion = direccionCliente.Trim();
                    if (!string.IsNullOrWhiteSpace(telefonoCliente)) clienteExistente.Telefono = telefonoCliente.Trim();
                    if (!string.IsNullOrWhiteSpace(emailCliente)) clienteExistente.Email = emailCliente.Trim();
                    await _clienteService.UpdateAsync(clienteExistente.Id, clienteExistente);
                }
                else if (clienteExistente == null)
                {
                    clienteExistente = cliente.FirstOrDefault();
                    if (clienteExistente == null)
                    {
                        TempData["Error"] = "Debe proporcionar datos del cliente (nombre y NIT/DUI).";
                        return RedirectToAction(nameof(Index));
                    }
                }

                // Obtener usuario activo
                var usuarioId = await _context.Usuarios
                    .Where(u => u.Activo)
                    .Select(u => u.IdUsuario)
                    .FirstOrDefaultAsync();

                if (usuarioId == 0)
                {
                    TempData["Error"] = "No hay usuarios activos";
                    return RedirectToAction(nameof(Index));
                }

                // Crear factura
                var detalles = new List<Models.Facturacion.DetalleFactura>();
                foreach (var item in cart.Items)
                {
                    var producto = await _productoService.GetByIdAsync(item.ProductoId);
                    if (producto != null)
                    {
                        detalles.Add(new Models.Facturacion.DetalleFactura
                        {
                            IdProducto = producto.IdProducto,
                            NombreProducto = producto.NombreProducto,
                            Cantidad = item.Cantidad,
                            PrecioUnitario = item.Precio,
                            Descuento = 0
                        });
                    }
                }

                var subTotal = detalles.Sum(d => d.Total);
                var iva = subTotal * 0.13m;

                var esTarjeta = string.Equals(metodoPago, "Tarjeta", StringComparison.OrdinalIgnoreCase);
                var referenciaPago = "";
                if (esTarjeta)
                {
                    var partes = new List<string>();
                    if (!string.IsNullOrWhiteSpace(nombreEnTarjeta))
                        partes.Add("Titular: " + nombreEnTarjeta.Trim());
                    if (!string.IsNullOrWhiteSpace(ultimos4Digitos))
                        partes.Add("****" + ultimos4Digitos.Trim());
                    if (!string.IsNullOrWhiteSpace(referenciaTarjeta))
                        partes.Add("Ref: " + referenciaTarjeta.Trim());
                    referenciaPago = partes.Count > 0 ? string.Join(" | ", partes) : "Pago con tarjeta";
                }

                var factura = new Models.Facturacion.Factura
                {
                    ClienteId = clienteExistente.Id,
                    ClienteNitDui = clienteExistente.NitDui,
                    ClienteNombre = clienteExistente.Nombre,
                    ClienteDireccion = clienteExistente.Direccion,
                    TipoDocumento = "01",
                    SerieFactura = "F001",
                    FechaCreacion = DateTime.UtcNow,
                    FechaPago = DateTime.UtcNow,
                    ReferenciaMetodoPago = string.IsNullOrWhiteSpace(referenciaPago) ? null : referenciaPago,
                    MonedaSimbolo = "USD",
                    Estado = Models.Enums.EstadoFactura.Pagada,
                    UsuarioId = usuarioId,
                    SubTotal = subTotal,
                    Iva = iva,
                    Total = subTotal + iva,
                    DetalleFacturas = detalles
                };

                // Validar que hay productos en el carrito
                if (detalles.Count == 0)
                {
                    TempData["Error"] = "El carrito está vacío. Agrega productos antes de realizar el pago.";
                    return RedirectToAction(nameof(Index));
                }

                // Validar datos del cliente
                if (string.IsNullOrWhiteSpace(nombreCliente) && string.IsNullOrWhiteSpace(nitCliente))
                {
                    TempData["Error"] = "Debe proporcionar al menos el nombre o NIT del cliente.";
                    return RedirectToAction(nameof(Index));
                }

                var facturaCreada = await _facturaService.CreateAsync(factura);

                // Limpiar carrito solo si la factura se creó exitosamente
                ClearCart();

                TempData["Success"] = $"Factura #{facturaCreada.IdFactura} creada exitosamente. Método de pago: {metodoPago}";
                return RedirectToAction("Details", "Facturas", new { id = facturaCreada.IdFactura });
            }
            catch (Fatura.Exceptions.BusinessRuleException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Fatura.Exceptions.EntityNotFoundException ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al procesar el pago: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private CartViewModel GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return new CartViewModel { Items = new List<CartItem>() };
            }
            var cart = JsonSerializer.Deserialize<CartViewModel>(cartJson) ?? new CartViewModel { Items = new List<CartItem>() };
            if (cart.Items == null)
                cart.Items = new List<CartItem>();
            return cart;
        }

        private void SaveCart(CartViewModel cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString("Cart", cartJson);
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove("Cart");
        }

        private string GetProductImage(string nombreProducto)
        {
            var nombreLower = nombreProducto.ToLower();
            if (nombreLower.Contains("casco"))
                return "~/css/Image/Casco.png";
            if (nombreLower.Contains("aceite"))
                return "~/css/Image/Aceite.png";
            if (nombreLower.Contains("guante"))
                return "~/css/Image/Guantes.png";
            if (nombreLower.Contains("bujía") || nombreLower.Contains("bujia"))
                return "~/css/Image/Bujia.png";
            return "~/css/Image/Casco.png"; // Imagen por defecto
        }
    }

    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal SubTotal => Items.Sum(x => x.SubTotal);
        public decimal Iva => SubTotal * 0.13m;
        public decimal Total => SubTotal + Iva;
    }

    public class CartItem
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string Imagen { get; set; } = string.Empty;
        public decimal SubTotal => Precio * Cantidad;
    }

    public class AddToCartRequest
    {
        [JsonPropertyName("productoId")]
        public int ProductoId { get; set; }
        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; } = 1;
    }
}
