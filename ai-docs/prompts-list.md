Actua como un experto en Backend .NET developer, con alta experiencia en Clean Architecture.

Quiero generar un documento inicial que sera usado como guia por Cursor (AI generativa) para plater un diseño inicial para un challenge de backend developer que te adjuntare a continuacion.

Estoy pensando en usar la arquitectura Clean y tambien dividir la solucion en 3 proyectos, que cubran el dominio, la infraestructura, la API y tambien el proyectos de test unitarios y de integracion (Usando XUnit).

El resultado debe ser un documento sencillo y simple en markdown, podras usar mermaid para explicar puntos importantes y tambien Diagrama de Secuencia para ver los flujos mas importantes de la API

Puedes usar como referencia basica en siguiente enlace para conceptos bases de clean arquitecture https://medium.com/@diego.coder/introducci%C3%B3n-a-las-clean-architectures-723fe9fe17fa. Tambien puedes revisar documentacion online para mejorar la propuesta inicial.


Por ahora quisiera manejar los 3 endpoind que solicitan para el manejo de imagenes, aunque se plantea el uso de la base de datos, solo quiero usar una estructura basica que pueda 
explicar al leader para la implementacion del proyecto a futuro
obtener detalles del productos podria manejar una consulta para obtener informacion basica como titulo, descripcion, precio y podemos agregar 
otros endpoints para cargar imagenes, y comentarios
Por ahora funciones avanzandas no se agregaran, el enfoque seria solo en el endpoint de obtener productos.
Quisiera un punto intermedio demostrar el enfoque inicial que te describi con los elementos que quiero usar en el proyecto, me tocara defender el proyecto ante otros developer asi que quiero usar los principales elementos para poder repasarlos y estudiarlos.


el documento teniendo en cuenta lo siguiente.
1. Se va a manejar un JSON para almacenar la data de los customer/leads. quiero que esta informacion basica este en el documento tambien. los datos que van en un customer/lead por ahora es la siguiente
    - Name.
    - Email.
    - Phone Number.
    - Address.
2. Para efectos de simplicar el proyecto quisiera usar esta estructura, teniendo en cuenta dejar el manejo de errores y los demas puntos que ya mencionates en el documento
Malte.Clean.API (.NET Soluction estructure)
    - src
        - Malte.Clean.API (Project .NET 8)
        - Malte.Clean.Data (Project .NET 8)
        - Malte.Clean.Domain (Project .NET 8)
    - test
        - Malte.Clean.API.IntegrationTests (Project .NET 8 Xunit)
        - Malte.Clean.API.UnitTests (Project .NET 8 Xunit)



IMPORTANTE EL DOCUMENTO DEBE ESTAR EN INGLES