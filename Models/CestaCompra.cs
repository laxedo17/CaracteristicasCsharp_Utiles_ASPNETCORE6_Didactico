using System.Collections;

namespace CaracteristicasLinguaxe.Models
{
    /// <summary>
    /// Clase simple que actua como wrapper arredor dunha secuencia de obxectos Producto.
    /// Usamos esta clase para explicar os Extension Methods.
    /// Imaxinamos que non temos o codigo fuente pero queremos ampliar a clase para que nos indique o valor total dos productos.
    /// Utilizamos un Extension Method para elo. A clase MeusExtensionMethods.
    /// </summary>
    /// Os extension methods tamen se poden usar con interfaces.
    /// O cal permite chamar ao extension method en TODAS as clases que implementen esa interface
    public class CestaCompra : IEnumerable<Producto?>, IProductoSeleccion
    {
        public IEnumerable<Producto?>? ProductosInicial { get; set; }

        private List<Producto> productos = new();

        public CestaCompra(params Producto[] prods)
        {
            productos.AddRange(prods);
        }

        public IEnumerable<Producto>? ProductosDefaultImplementation { get => productos; }

        public IEnumerator<Producto?> GetEnumerator() => ProductosInicial?.GetEnumerator() ?? Enumerable.Empty<Producto?>().GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}