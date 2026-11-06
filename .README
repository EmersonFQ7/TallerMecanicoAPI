# TallerMecanicoAPI

API REST en C# (.NET 10) para gestionar un taller mecánico: clientes, vehículos y órdenes de servicio.

## Despliegue

- **API publicada (Render):** 
- **Swagger UI:** https://tallermecanicoapi.onrender.com/swagger/index.html

> Nota: en el plan gratuito de Render el servicio puede tardar cerca de un minuto en responder la primera vez.

## Base de datos

| Entorno | Motor | Ubicación |
|---|---|---|
| Producción | PostgreSQL | Neon (nube) |
| Desarrollo | SQL Server | Local |

## Pruebas realizadas

Probadas desde Swagger UI:

| Prueba | Resultado |
|---|---|
| PUT con id de la URL distinto al del cuerpo | 400 Bad Request |
| POST de cliente con correo duplicado | 409 Conflict |
| POST de vehículo con placa duplicada | 409 Conflict |
| POST de vehículo con cliente inexistente | 400 Bad Request |
| POST de orden de servicio con vehículo inexistente | 400 Bad Request |
| DELETE de cliente con vehículos registrados | 409 Conflict |
| DELETE de vehículo con órdenes de servicio | 409 Conflict |
| GET por id (clientes, vehículos, órdenes) | 200 OK |
