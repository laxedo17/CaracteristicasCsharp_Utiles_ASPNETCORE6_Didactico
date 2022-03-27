namespace CaracteristicasLinguaxe.Models
{
    public interface IProductoSeleccion
    {
        IEnumerable<Producto>? ProductosInicial { get; }

        //A instruccion define unha propiedade Nomes e engade unha default implementation
        //o cal significa que quenes consumen a interface IProductoSeleccion poden
        //usar a property Nomes incluso se non esta definida polas clases de implementacion
        IEnumerable<string>? Nomes => ProductosInicial?.Select(p => p.Nome);
    }
}