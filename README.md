# RoyalState

## Descripción breve
Royal Estate es una aplicación web diseñada para la gestión de propiedades inmobiliarias. Permite a los usuarios buscar propiedades, marcar sus favoritas y contactar con agentes. Los agentes pueden gestionar sus propiedades y los administradores tienen control total sobre los usuarios y las propiedades del sistema.

[<img src="https://img.youtube.com/vi/fPpkIvz85UQ/maxresdefault.jpg" width="100%">](https://youtu.be/fPpkIvz85UQ?target="_blank")

## Funcionalidades
- **Búsqueda de Propiedades**: Los usuarios pueden buscar propiedades por código, tipo, rango de precio, cantidad de habitaciones y baños.
- **Detalles de Propiedad**: Al seleccionar una propiedad, se muestra un detalle completo incluyendo imágenes, descripción y mejoras.
- **Registro de Usuarios**: Los usuarios pueden registrarse como clientes o agentes, con flujos de activación específicos para cada rol.
- **Autenticación de Usuarios**: Inicio de sesión con validación de credenciales y manejo de roles (cliente, agente, administrador).
- **Gestión de Propiedades**: Los agentes pueden crear, editar y eliminar propiedades, incluyendo imágenes y mejoras.
- **Panel de Administrador**: Los administradores pueden gestionar agentes, desarrolladores, tipos de propiedades, tipos de ventas y mejoras.

## Herramientas y Tecnologías
- **ASP.NET MVC 7**: Para la construcción de la aplicación web.
- **Entity Framework**: Para la persistencia de datos con el enfoque Code First.
- **Bootstrap**: Para un diseño responsivo y atractivo.
- **Arquitectura ONION**: Asegurando una separación clara de responsabilidades.
- **Identity**: Para la gestión de usuarios y roles.
- **JWT**: Para la seguridad de la API.
- **CQRS y Mediator**: Patrones de diseño para una estructura de código clara de la API.
- **Swagger**: Para la documentación de la API.

## También utilizado
- Uso de viewmodels y validaciones desde los mismos.
- Implementación de la arquitectura ONION.
- Repositorio y servicio genéricos.
- Automapper para el mapeo de objetos de datos.
- Documentación clara y completa de la API.

## Seguridad
- Restricciones de acceso basadas en roles y autorización mediante JWT en el API.
- Creación de usuarios predeterminados para roles de administrador y desarrollador.
