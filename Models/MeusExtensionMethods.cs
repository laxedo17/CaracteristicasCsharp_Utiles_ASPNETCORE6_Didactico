namespace CaracteristicasLinguaxe.Models
{
    public static class MeusExtensionMethods
    {
        /// <summary>
        /// Extension method para a cesta da compra. Os extension methods son sempre static.
        /// Os extension methods non rompen as regras de acceso que as clases definen por metodos, fields e properties.
        /// Extenden a funcionalidade da clase so usando class members aos que xa tiñamos acceso de todas formas.
        /// </summary>
        /// <param name="cestaParam"></param>
        /// <returns></returns>
        /*
        public static decimal PreciosTotales(this CestaCompra cestaParam)
        {
            decimal total = 0;
            if (cestaParam.Productos != null)
            {
                foreach (Producto? producto in cestaParam.Productos)
                {
                    //se o producto non e null sumamos o precio ao total, se e null sumamos 0 ao total do precio
                    total += producto?.Precio ?? 0;
                }
            }
            return total;
        }
        */


        /// <summary>
        /// Metodo como MeusExtensionMethods pero usando un extension method da interface IEnumerable.
        /// O primeiro parametro cambiou a IEnumerable<Producto?> o cal significa que o loop foreach no body do metodo
        /// traballa directamente sobre obxectos Producto?.
        /// O cambio a unha interface significa que podemos calcular o valor do obxectos Producto enumerados por calquera IEnumerable<Producto?>
        /// o cal inclue instancias de CestaCompra pero tamen arrays de obxectos Producto.
        /// </summary>
        /// <param name="productos"></param>
        /// <returns></returns>
        public static decimal PreciosTotales(this IEnumerable<Producto?> productos)
        {
            decimal total = 0;
            foreach (Producto? producto in productos)
            {
                total += producto?.Precio ?? 0;
            }
            return total;
        }

        /// <summary>
        /// Extension Method con Filtering.
        /// Os extension methods poden usarse para filtrar coleccions de obxetos. Un extension metodo operando nun IEnumerable<T> que tamen devolve
        /// IEnumerable<T> pode usar a palabra clave yield para aplicar un criterio de seleccion a elementos nos datos de orixen para producir
        /// un conxunto reducido de resultados.
        /// </summary>
        /// <param name="productoEnum"></param>
        /// <param name="precioMinimo">Parametro adicional que permite filtrar productos de tal forma que os obxectos Producto cuxa propiedade se corresponda ou exceda o parametro, devolvese no resultado</param>
        /// <returns></returns>
        public static IEnumerable<Producto?> FiltrarPorPrecio(this IEnumerable<Producto?> productoEnum, decimal precioMinimo)
        {
            foreach (Producto? producto in productoEnum)
            {
                if ((producto?.Precio ?? 0) >= precioMinimo)
                {
                    yield return producto;
                }
            }
        }

        //Cando usar Lambda Expressions
        //As lambda expressions resolven problemas como o que se ve agora.
        //Temos o metodo FiltrarPorPrecio e imaxinamos que tamen queremos filtrar por nome
        //co cal temos que crear un segundo metodo se queremos filtrar por nome.
        //Tal que asi.
        public static IEnumerable<Producto?> FiltrarPorNome(this IEnumerable<Producto?> productoEnum, char primeiraLetra)
        {
            foreach (Producto? producto in productoEnum)
            {
                if (producto?.Nome[0] == primeiraLetra)
                {
                    yield return producto;
                }
            }
        }

        //Definindo Functions
        //Podemos repetir o proceso anterior indefinidamente para crear metodos de filtro para cada propiedade e combinacion de propiedades que queiramos.
        //Unha forma mais elegante e separar o codigo que procesa a enumeracion do criterio de seleccion.
        //En C# isto pode facerse facilmente porque permite que haxa funcions que se pasen como obxectos.
        //O segundo argumento do metodo Filtro e unha function Func que acepta un obxecto Producto? e devolve un bool
        //O metodo filtro chama a Function por cada obxecto Producto e inclueo no resultado se a funcion devolve true
        public static IEnumerable<Producto?> Filtro(this IEnumerable<Producto?> productoEnum, Func<Producto?, bool> selector)
        {
            foreach (Producto? producto in productoEnum)
            {
                if (selector(producto))
                {
                    yield return producto;
                }
            }
        }

        //Cos cambios feitos en HomeController.cs e nesta clase observamos que 
        //FiltrarPorPrecio complica a definicion dunha clase
        //Crear un obxecto Func<Producto?, bool> evita este problema pero usa unha extraña sintaxis
        //dificil de ler e de manter.
        //Este e un problema que as lambda expressions solucionan permitindo as functions que se definan
        //nunha forma mais elegante e expresiva
    }
}