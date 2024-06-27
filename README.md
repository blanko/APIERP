# Proyecto API Minimas CRUD de ERP

### Curso Net Core 8

#### Requisitos de la API

- CRUD de todas las tablas
- Implementar lo que podais de buenas practicas y cosas ya aprendidas

### Caracteristicas Añadidas

Tenéis que construir la interfaz de usuario con las siguientes restricciones:

- Paginador como metodo .Paginar()
- Almacen de Archivo en wwwroot, he borrado versión que estaba haciendo de storage microsoft
- Guardor url del archivo subido y guardado en la base datos en Categories.ImageUrl
- Comienzo un repo de tipo T, para refactorizar código, de momento el delete usa este neuvo de tipo T en casi todos
- CRUD de las tablas
- Grupos de endpoint aislados
- Repositorio por cada grupo de enpoints
- Añadida las primeras validaciones
- Modificación de cabecera para entregar información de cabecera con el paginador
- Control de errores, mostrado un error generico al usuario
- Guardo el error con Guid, fecha en la base datos para ir registrandolos
- Tener en cuenta escenario futuro desde una app móvil (secundario)
- Habilitado CORS y un regla de CORS libre, para un endpoint de pruebas
- Habilito los archivos estatidos y los codigos de error, para cuando este en producción, junto a los errores de antes.
- Utilizo automapper y creo DTO de creación, aparte de los dto de ver.
- Uso Filtro, Extender funcionalidad de clases existen,...
