# Configurar Gmail para enviar facturas por correo

Para que el envío de PDF por correo funcione con **Gmail**, sigue estos pasos:

## 1. Activar verificación en 2 pasos

1. Entra en tu cuenta de Google: https://myaccount.google.com
2. Ve a **Seguridad** → **Verificación en 2 pasos** y actívala si no lo está.

## 2. Crear una contraseña de aplicación

1. Abre: https://myaccount.google.com/apppasswords
2. En "Seleccionar app" elige **Correo** (o "Otra" y escribe "Sistema Facturación").
3. Genera la contraseña. Verás **16 caracteres** (puede aparecer en 4 grupos).
4. **Cópiala sin espacios** y úsala como `SmtpPassword` en la configuración.

> ⚠️ **No uses tu contraseña normal de Gmail.** Gmail la rechazará. Solo sirve la contraseña de aplicación.

## 3. Configurar la aplicación

Edita `appsettings.Development.json` o usa **User Secrets** (recomendado):

### Opción A: appsettings.Development.json

```json
"Email": {
  "SmtpHost": "smtp.gmail.com",
  "SmtpPort": 587,
  "SmtpUsuario": "tu-email@gmail.com",
  "SmtpPassword": "abcdefghijklmnop",
  "SmtpFrom": "tu-email@gmail.com",
  "SmtpFromName": "Sistema de Facturación"
}
```

Sustituye `tu-email@gmail.com` por tu Gmail y `abcdefghijklmnop` por la contraseña de aplicación de 16 caracteres.

### Opción B: User Secrets (más seguro, no subas credenciales a Git)

En la carpeta del proyecto `Fatura`:

```bash
dotnet user-secrets set "Email:SmtpUsuario" "tu-email@gmail.com"
dotnet user-secrets set "Email:SmtpPassword" "tu-16-chars-app-password"
dotnet user-secrets set "Email:SmtpFrom" "tu-email@gmail.com"
```

Los valores de User Secrets tienen prioridad sobre `appsettings`.

## 4. Probar

Crea una factura, pon un email en el campo correspondiente, y al **Descargar PDF** o **Enviar PDF por correo** se enviará también al destinatario. Si falla, revisa los logs de la aplicación para ver el error exacto.
