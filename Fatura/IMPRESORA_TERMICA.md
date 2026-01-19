# Configuración de Impresora Térmica RPT004

## Instrucciones de Instalación

### 1. Conectar la Impresora
- Conecta la impresora térmica RPT004 a tu computadora mediante USB
- Enciende la impresora

### 2. Instalar el Driver
- Windows debería detectar automáticamente la impresora
- Si no se detecta, descarga e instala el driver desde el sitio del fabricante
- Verifica que la impresora aparezca en "Dispositivos e impresoras" de Windows

### 3. Configurar el Nombre de la Impresora
La aplicación busca la impresora con el nombre "RPT004" por defecto.

**Opción A: Renombrar la impresora en Windows**
1. Ve a Panel de Control > Dispositivos e impresoras
2. Haz clic derecho en tu impresora térmica
3. Selecciona "Propiedades de impresora"
4. Cambia el nombre a "RPT004"

**Opción B: Usar el nombre actual**
Si tu impresora tiene otro nombre, la aplicación intentará detectar automáticamente impresoras que contengan:
- "RPT"
- "POS"
- "Thermal"

### 4. Probar la Impresión
1. Abre una factura en el sistema
2. Haz clic en el botón "Imprimir Ticket"
3. El ticket se imprimirá automáticamente en la impresora térmica

## Solución de Problemas

### La impresora no se encuentra
**Error:** "No se encontró la impresora 'RPT004'"

**Solución:**
1. Verifica que la impresora esté encendida y conectada
2. Abre PowerShell y ejecuta:
   ```powershell
   Get-Printer | Select-Object Name
   ```
3. Busca el nombre exacto de tu impresora en la lista
4. Renombra la impresora a "RPT004" o modifica el código para usar tu nombre

### El ticket no se imprime
**Posibles causas:**
- La impresora está sin papel
- El cable USB está desconectado
- El driver no está instalado correctamente
- La impresora está en modo offline

**Solución:**
1. Verifica que haya papel en la impresora
2. Revisa las conexiones físicas
3. En "Dispositivos e impresoras", haz clic derecho y selecciona "Ver lo que se está imprimiendo"
4. Si hay trabajos pendientes, cancélalos y vuelve a intentar

### Formato del Ticket
El ticket está diseñado para impresoras térmicas de **80mm** de ancho.

**Contenido del ticket:**
- Nombre de la tienda
- Número de factura
- Datos del cliente
- Lista de productos con cantidades y precios
- Subtotal, IGV y Total
- Mensaje de agradecimiento

## Especificaciones Técnicas

- **Ancho del papel:** 80mm
- **Caracteres por línea:** 32
- **Fuente:** Courier New 9pt
- **Método de impresión:** PrintDocument de Windows (System.Drawing.Printing)

## Notas Importantes

⚠️ **Solo Windows:** La funcionalidad de impresión térmica solo está disponible en Windows debido al uso de `System.Drawing.Printing`.

💡 **Alternativa PDF:** Si tienes problemas con la impresión directa, puedes usar el endpoint `/Facturas/{id}/TicketPdf` para generar un PDF del ticket y luego imprimirlo manualmente.
