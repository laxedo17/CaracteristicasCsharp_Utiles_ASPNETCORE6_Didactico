namespace CaracteristicasLinguaxe.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            //return View(new string[] { "C#", "Linguaxe", "Caracteristicas" });
            Producto?[] productos = Producto.GetProductos();

            //Usando o null-coalescing operator para abreviar codigo
            return View(new string[] { productos[0]?.Nome ?? "Non conten un valor" });
            //os operadors ? e ?? non sempre poden usarse, e a veces necesitaremos if para buscar valores null
            //Por exemplo coas palabras clave await/async, que non se integran ben co null conditional operator

            //facer Overriding Null State Analysis con Null-Forgiving Operator
            //O compilador de C# ten unha sofisticada comprension de cando unha variable pode ser null
            //pero non sempre acerta.
            //A veces como humanos sabemos se un valor null pode ocurrir ou non, comparado co compiler

            //o Null-Forgiving Operator e unha exclamacion e usase
            //para indicarlle ao compiler que productos[0] non e null
            //anque o null state analysis do compiler identifica que poderia ser null
            return View(new string[] { productos[0]!.Nome });
            //usando o operador ! estamos indicando ao compiler
            //que sabemos mellor se unha variable pode ser null
            //e naturalmente esto SOLO debemos facelo se estamos completamente segur@s de estar no certo

            //outras maneiras diferentes de facer as linhas de codigo de arriba

            /*
            Producto? p = productos[0];
            string val;
            if (p != null)
            {
                val = p.Nome;
            }
            else
            {
                val = "Non conten un valor";
            }
            return View(new string[] { val });
            */
            /*
            string? val = productos[0]?.Nome;
            if (val != null)
            {
                return View(new string[] { val });
            }
            return View(new string[] { "Non conten un valor" });
            */

            //usando String Interpolation
            //os interpolated strings prefixanse co caracter $ e conteñen holes
            //os holes son as referencias a valores contidas dentro dos caracteres { e }
            //cando se evalua o string os holes enchense cos valores actuales das variables ou constants especificadas
            //string interpolation soporta especificadores de string format
            //co cal $Precio: {precio:C2} formatea o valor do precio como valor de currency -moeda- con 2 dixitos decimales
            return View(new string[]{
                $"Nome: {productos[0]?.Nome}, Precio: {productos[0]?.Precio}"
            });

            //inicializando un Dictionary usando un Index Initializer
            //Forma clasica de facelo
            Dictionary<string, Producto> productos2 = new Dictionary<string, Producto>{
                {"Kayak", new Producto {Nome="Kayak", Precio=275M}},
                {"Chaleco", new Producto{Nome="Chaleco", Precio=48.95M}}
            };
            return View("Index", productos2.Keys);

            //mesmo efecto pero con Index Initializer, moito mais simple
            Dictionary<string, Producto> productos3 = new Dictionary<string, Producto>
            {
                ["Kayak"] = new Producto { Nome = "Kayak", Precio = 275M },
                ["Chaleco"] = new Producto { Nome = "Chaleco", Precio = 48.95M }
            };
            return View("Index", productos3.Keys);

            //Target-Type New Expressions   
            //As mesmas expresions de antes pero con Target-Typed New Expressions
            //basicamente new() en vez de escribir os types outra vez 
            Dictionary<string, Producto> productos4 = new()
            {
                ["Kayak"] = new Producto { Nome = "Kayak", Precio = 275M },
                ["Chaleco"] = new Producto { Nome = "Chaleco", Precio = 48.95M }
            };
            return View("Index", productos4.Keys);
        }

        //Usando Pattern Matching, tipico de F# pero en C#
        public ViewResult PatternMatching()
        {
            object[] datos = new object[] { 275M, 29.95M,
            "pera", "banana", 100, 10 };
            decimal total = 0;
            for (int i = 0; i < datos.Length; i++)
            {
                //a palabra clave is fai un type check
                //e se o valor e do tipo especificado
                //asignara o valor a unha nova variable
                //a valor de datos[i] asignase a variable d
                //que permite utilizarse en statements sucesivos
                //sen necesidade de facer type conversions
                //is solo permitira que se corresponda co type especificado
                //co cal solo dous valores en TODO o array de datos
                //se procesaran (os outros elementos son valores string e int)
                if (datos[i] is decimal d)
                {
                    total += d;
                }
            }
            return View("PatternMatching", new string[] { $"Total: {total:C2}" });
            //Total: 304.95€
        }

        public ViewResult PatternMatchingEnSwitchStatements()
        {
            object[] datos = new object[] { 275M, 29.95M,
            "pera", "banana", 100, 10 };
            decimal total = 0;
            for (int i = 0; i < datos.Length; i++)
            {
                switch (datos[i])
                {
                    case decimal decimalValor:
                        total += decimalValor;
                        break;
                    case int intValor when intValor > 50:
                        total += intValor;
                        break;
                }
            }
            //agora sumamos todo
            //pero para int solo cando (when) o int e superior a 50, engadimolo na suma
            return View("PatternMatchingEnSwitchStatements", new string[] { $"Total: {total:C2}" });
            //Total: 404.95€
        }

        public ViewResult ExtensionMethods()
        {
            CestaCompra cesta = new CestaCompra { ProductosInicial = Producto.GetProductos() };
            decimal totalCesta = cesta.PreciosTotales(); //instruccion clave. Chamamos o metodo PreciosTotales como se fose parte da clase CestaCompra 
            return View("ExtensionMethods", new string[] { $"Total: {totalCesta:C2}" });
        }

        public ViewResult ExtensionMethodsConInterfaces()
        {
            CestaCompra cesta = new CestaCompra { ProductosInicial = Producto.GetProductos() };

            Producto[] productoArray ={
                new Producto{Nome = "Kayak", Precio = 275M },
                new Producto{Nome = "Chaleco", Precio = 48.95M }
            };

            decimal cestaTotales = cesta.PreciosTotales();
            decimal arrayTotales = productoArray.PreciosTotales();

            return View("ExtensionMethodsConInterfaces", new string[]{
                $"Total Cesta: {cestaTotales:C2}",
                $"Total Array: {arrayTotales:C2}"
            });
        }

        public ViewResult ExtensionMethodsConFiltering()
        {
            CestaCompra cesta = new CestaCompra { ProductosInicial = Producto.GetProductos() };

            Producto[] productoArray ={
                new Producto {Nome = "Kayak", Precio = 275M },
                new Producto {Nome = "Chaleco", Precio = 48.95M },
                new Producto {Nome = "Pelota futbol", Precio=19.50m },
                new Producto {Nome = "Bandeira corner", Precio=34.95M }
            };

            decimal totalArray = productoArray.FiltrarPorPrecio(20).PreciosTotales();

            return View("ExtensionMethodsConFiltering", new string[]{
                $"Total Array: {totalArray:C2}"
            });
        }

        public ViewResult ExtensionMethodsConFilteringPrecioENomeAoMesmoTempo()
        {
            CestaCompra cesta = new CestaCompra { ProductosInicial = Producto.GetProductos() };

            Producto[] productoArray ={
                new Producto {Nome = "Kayak", Precio = 275M },
                new Producto {Nome = "Chaleco", Precio = 48.95M },
                new Producto {Nome = "Pelota futbol", Precio=19.50m },
                new Producto {Nome = "Bandeira corner", Precio=34.95M }
            };

            decimal filtroPreciosTotal = productoArray.FiltrarPorPrecio(20).PreciosTotales();
            decimal filtroNomeTotal = productoArray.FiltrarPorNome('P').PreciosTotales();

            return View("ExtensionMethodsConFilteringPrecioENomeAoMesmoTempo", new string[]{
                $"Total Precio: {filtroPreciosTotal:C2}",
                $"Total filtrado por Nome: {filtroNomeTotal:C2}"
            });
            /*
            Total Precio: 358.90€
            Total filtrado por Nome: 19.50€
            */
        }

        bool FiltrarPorPrecio(Producto? p)
        {
            return (p?.Precio ?? 0) >= 20;
        }

        public ViewResult FiltrarPorPrecioENomeConFunc()
        {
            CestaCompra cesta = new CestaCompra { ProductosInicial = Producto.GetProductos() };

            Producto[] productoArray ={
                new Producto {Nome = "Kayak", Precio = 275M },
                new Producto {Nome = "Chaleco", Precio = 48.95M },
                new Producto {Nome = "Pelota futbol", Precio=19.50m },
                new Producto {Nome = "Bandeira corner", Precio=34.95M }
            };

            // Func<Producto?, bool> nomeFiltro = delegate (Producto? producto)
            // {
            //     return producto?.Nome[0] == 'P';
            // };

            // decimal filtroPreciosTotal = productoArray.Filtro(FiltrarPorPrecio).PreciosTotales();
            // decimal filtroNomeTotal = productoArray.Filtro(nomeFiltro).PreciosTotales();

            //LAMBDA EXPRESSIONS que solucionan todo o codigo complexo comentado anterior e xa non nos fai falta a funcion FiltrarPorPrecio
            decimal filtroPreciosTotal = productoArray
            .Filtro(p => (p?.Precio ?? 0) >= 20)
            .PreciosTotales();

            decimal filtroNomeTotal = productoArray
            .Filtro(p => p?.Nome?[0] == 'P')
            .PreciosTotales();
            //as Lambda Expressions permiten que os parametros se expresen sin especificar un type
            //o cal inferirase automaticamente
            //os caracteres => en voz alta leense como "goes to" (vai a)
            //e enlazan o parametro ao resultado da lambda expression
            //nos exemplos un parametro Producto? chamado p goes to un resultado bool
            //o cal sera true se a propiedade Precio e igual o maior que 20 na primeira expresion
            //ou se a propiedade Nome comenza por S na segunda expresion
            //Este codigo funciona da mesma forma que un metodo separado e unha function delegate
            //pero e mais conciso e -para a maioria da xente- mais facil de ler

            return View("ExtensionMethodsConFilteringPrecioENomeAoMesmoTempo", new string[]{
                $"Total Precio: {filtroPreciosTotal:C2}",
                $"Total filtrado por Nome: {filtroNomeTotal:C2}"
            });
            /*
            Total Precio: 358.90€
            Total filtrado por Nome: 19.50€
            */
        }

        public ViewResult FiltroDeAntesMoitoMaisEleganteConLambdaExpressions()
        {
            CestaCompra cesta = new CestaCompra { ProductosInicial = Producto.GetProductos() };

            Producto[] productoArray ={
                new Producto {Nome = "Kayak", Precio = 275M },
                new Producto {Nome = "Chaleco", Precio = 48.95M },
                new Producto {Nome = "Pelota futbol", Precio = 19.50m },
                new Producto {Nome = "Bandeira corner", Precio = 34.95M }
            };

            Func<Producto?, bool> nomeFiltro = delegate (Producto? producto)
            {
                return producto?.Nome[0] == 'P';
            };

            decimal filtroPreciosTotal = productoArray.Filtro(FiltrarPorPrecio).PreciosTotales();
            decimal filtroNomeTotal = productoArray.Filtro(nomeFiltro).PreciosTotales();

            return View("ExtensionMethodsConFilteringPrecioENomeAoMesmoTempo", new string[]{
                $"Total Precio: {filtroPreciosTotal:C2}",
                $"Total filtrado por Nome: {filtroNomeTotal:C2}"
            });
            /*
            Total Precio: 358.90€
            Total filtrado por Nome: 19.50€
            */
        }

        /// <summary>
        /// As lambda expressions poden usarse para implementar constructores, metodos e properties.
        /// No desenvolvemente ASP.NET Core, normalmente terminaremos con metodos que conteñen unha simple instruccion
        /// que selecciona os datos a mostrar e a vista a renderizar.
        /// Aqui o metodo Index() reescrito seguindo ese patron comun
        /// </summary>
        /// <returns></returns>
        public ViewResult IndexLambdaExpressionMethodEProperties()
        {
            return View(Producto.GetProductos().Select(p => p?.Nome));
            //o action method obten -get- unha coleccion de obxectos Producto do metodo estatico
            //Producto.GetProductos e usa LINQ para proxectar os valores da propiedade Nome
            //os cales son enton usados como view model para a vista por defecto
            //O resultado serian (se so temos na lista esos dous obxetos)
            /*
                Kayak
                Chaleco
            */
        }

        //Cando o body dun metodo ou constructor inclue unha sola linha, podese reescribir como unha lambda expression
        //e moitimos a palabra clave return e usamos => (goes to) para asociar a firma do metodo (incluidos os argumentos) coa sua implementacion
        public ViewResult MetodoAnteriorSobreescritoCunhaSoloLinhaDeCodigo() => View(Producto.GetProductos().Select(p => p?.Nome));

        //Este enfoque basico tamen se pode usar para definir as Properties
        //Na clase Producto engadimos unha property que usa unha lambda expression

        //Usando Type Inference e Anonymous Types
        public ViewResult IndexConTypeInference_E_AnonymousTypes()
        {
            var nomes = new[] { "Kayak", "Chaleco", "Pelota de futbol" };
            return View(nomes);
            //var indica que definimos unha variable local sen indicar que type e. Iso chamase type inference ou implicit typing
            //non e que nomes non pertenza a un type
            //o que estamos facendo e pedirlle ao compiler que infira o type usando o codigo
            //o compiler examina a declaracion array e deduce que e un string array
            /*
            Kayak
            Chaleco
            Pelota de futbol
            */
        }

        //Usando Anonymous Types
        //combinando object initializers e type inference, podes crear obxectos simple view model
        //que son utiles para transferir datos entre un controller e unha view
        //sen ter que definir unha class ou struct
        public ViewResult UsoAnonymousTypes()
        {
            var productos = new[]
            {
                new {Nome = "Kayak", Precio = 275M },
                new {Nome = "Chaleco", Precio = 48.95M },
                new {Nome = "Pelota futbol", Precio = 19.50m },
                new {Nome = "Bandeira corner", Precio = 34.95M }
            };
            //Cada obxecto no array productos e un anonymously typed object. Non e dynamic ao estilo Javascript.
            //Simplemente significa que a type definition vaina crear automaticamente o compiler.
            //Strong typing aconsellase mais.
            //Podes facer get e set unicamente das propiedades definidas no initializer
            //O compilador de C# xenera a clase basandose no nome e type dos parametros do initializer.
            //Dous obxectos anonymously typed que teñen os mesmos nomes de propiedades e types definidos
            //no mesmo orden asignaranse a mesma clase xenerada automaticamente.
            //Isto significa que todos os obxectos do array productos
            //teran o mesmo type porque definen as mesmas properties
            //Consello: usamos a palabra clave var para definir os anonymously typed objects porque o type
            //non se crea ata que se compila o codigo, asi que non sabeoms o nome do type a usar
            //Os elementos dun array de anonymously typed objects deben definir todos as mesmas propiedades
            //do contrario o compilador non pode adivinhar que tipo de array deberia ser

            return View(productos.Select(p => p.Nome));
        }

        public ViewResult UsoAnonymousTypesQueVeCompiladorDeCsharp()
        {
            var productos = new[]
            {
                new {Nome = "Kayak", Precio = 275M },
                new {Nome = "Chaleco", Precio = 48.95M },
                new {Nome = "Pelota futbol", Precio = 19.50m },
                new {Nome = "Bandeira corner", Precio = 34.95M }
            };
            //Cada obxecto no array productos e un anonymously typed object. Non e dynamic ao estilo Javascript.
            //Simplemente significa que a type definition vaina crear automaticamente o compiler.
            //Strong typing aconsellase mais.
            //Podes facer get e set unicamente das propiedades definidas no initializer
            //O compilador de C# xenera a clase basandose no nome e type dos parametros do initializer.
            //Dous obxectos anonymously typed que teñen os mesmos nomes de propiedades e types definidos
            //no mesmo orden asignaranse a mesma clase xenerada automaticamente.
            //Isto significa que todos os obxectos do array productos
            //teran o mesmo type porque definen as mesmas properties
            //Consello: usamos a palabra clave var para definir os anonymously typed objects porque o type
            //non se crea ata que se compila o codigo, asi que non sabeoms o nome do type a usar
            //Os elementos dun array de anonymously typed objects deben definir todos as mesmas propiedades
            //do contrario o compilador non pode adivinhar que tipo de array deberia ser

            return View(productos.Select(p => p.GetType().Name));
            //posible resultado, pode variar en cada caso
            /*
            <>f__AnonymousType0`2
            <>f__AnonymousType0`2
            <>f__AnonymousType0`2
            <>f__AnonymousType0`2
            */
        }

        //Default Implementations en Interfaces
        //C# permite definir default implementations para propiedades e metodos definidos en interfaces.
        //Por extrano que pareza dado que as interfaces estan pensadas para ser
        //unha descripcion de caracteristicas sen especificar unha implementacion, 
        //esta capacidade de C# permite actualizar interfaces sen romper as implementacions existentes
        public ViewResult IProductoSeleccion()
        {
            IProductoSeleccion cesta = new CestaCompra(
                new Producto { Nome = "Kayak", Precio = 275M },
                new Producto { Nome = "Chaleco", Precio = 48.95M },
                new Producto { Nome = "Pelota futbol", Precio = 19.50m },
                new Producto { Nome = "Bandeira corner", Precio = 34.95M }
            );
            return View(cesta.ProductosInicial?.Select(p => p.Nome));
        }
        //Se queremos engadir unha nova caracterista a interface, debemos localizar e actualizar todas as clases que a implementan.
        //Isto pode ser dificil, especialmente se a interface esta sendo usada por outros equipos de desenvolvemento nos seus proxectos.
        //Aqui e cando a default implementation se pode utilizar, permitindo engadir novas caracteristicas a unha interface.
        public ViewResult IProductoSeleccionInterfaceAmpliadaDefaultImplementation()
        {
            IProductoSeleccion cesta = new CestaCompra(
                new Producto { Nome = "Kayak", Precio = 275M },
                new Producto { Nome = "Chaleco", Precio = 48.95M },
                new Producto { Nome = "Pelota futbol", Precio = 19.50m },
                new Producto { Nome = "Bandeira corner", Precio = 34.95M }
            );
            return View(cesta.Nomes);
            //A clase CestaCompra non se modificou para nada, pero este metodo pode usar a default implementation
            //da property Nomes
        }

        //Metodo asincronico con async
        /// <summary>
        /// O obxecto obtido por esta clase aporta detalles da view que deberia renderizarse e os datos requeridos.
        /// </summary>
        /// <returns>Task que produce un obxecto ViewResult cando se completa</returns>
        public async Task<ViewResult> IndexAsync()
        {
            long? lonxitude = await MetodosAsync.GetLonxitudePaxina();
            return View(new string[] { $"Lonxitude; {lonxitude}" });
        }

        //Uso de Asynchronous Enumerable
        //Un asynchronous enumerable describe unha secuencia de valores que se xeneraran co tempo.
        //Para demostrar o problema que estas caracteristica soluciona, temos o codigo en metodo GetPaxinaLonxitudes no arquivo MetodosAsync.cs
        public async Task<ViewResult> SenAsynchronousEnumerable()
        {
            List<string> saida = new List<string>();
            foreach (long? lonx in await MetodosAsync.GetPaxinaLonxitudes(saida, "aldea.com", "microsoft.com", "bing.com"))
            {
                saida.Add($"Lonxitude Paxina: {lonx}");

            }
            return View(saida);
            //O action method enumera a secuencia producida polo metodo GetPaxinaLonxitudes
            //e engade cada resultado ao obxecto List<string>, o cal produce unha secuencia ordenada de mensaxes
            //mostrando a interaccion entre o loop foreach no metodo Index que procesa os resultados
            //e o loop foreach no metodo GetPaxinaLonxitudes que os xenera
            //Resultado
            /*
            Comenzou peticion de aldea.com
            Completada peticion para aldea.com
            Comenzou peticion de microsoft.com
            Completada peticion para microsoft.com
            Comenzou peticion de bing.com
            Completada peticion para bing.com
            Lonxitude paxina: 26973
            Lonxitude paxina: 199526
            Lonxitude paxina: 357777
            */

            //O metodo SenAsynchronousEnumerable NON recibe os resultados ata que TODAS as HTTP requests sexan completadas
        }

        //Este e o problema que resolve a caracteristica Asynchronous Enumerable
        //O resultado do metodo e IAsyncEnumerable<long?> o que denota un secuencia asincronica
        //de valores long nullables. Este tipo de resultado ten soporte especial en .NET Core
        //e funciona coas instruccions yield return standar, o cal e doutra maneira imposible
        //debido a restriccions de metodos asincronicos que teñen conflicto coa palabra clave yield
        public async Task<ViewResult> ConAsynchronousEnumerable()
        {
            List<string> saida = new List<string>();
            await foreach (long? lonx in MetodosAsync.GetPaxinaLonxitudesAsynchronousEnumerable(saida, "aldea.com", "microsoft.com", "bing.com"))
            {
                saida.Add($"Lonxitude Paxina: {lonx}");
            }
            return View(saida);
        }
        //a diferencia e que a palabra clave await aplicase antes da palabra clave foreach
        //e non antes da chamada ao metodo async
        //O resultado cambia e e mais como o esperado.
        //Resultado
        /*       
            Comenzou peticion de aldea.com
            Completada peticion para aldea.com
            Lonxitude paxina: 26973
            Comenzou peticion de microsoft.com
            Completada peticion para microsoft.com
            Lonxitude paxina: 199526
            Comenzou peticion de bing.com
            Completada peticion para bing.com
            Lonxitude paxina: 357777   
        */

        //Obtendo Names
        //Hai moitas tarefas no desarrollo dunha app web nas cales necesitamos referirnos ao nome dun argumento, variable, metodo, ou clase.
        //Exemplos comuns incluen cando lanzas unha excepcion ou creas un erro de validacion
        //cando se procesa unha entrada de usuario.
        //O metodo tradicional era usar un valor string hard-codeado co nome

        public ViewResult ObterNamesHardcodeado()
        {
            var productos = new[]
            {
            new {Nome = "Kayak", Precio = 275M },
            new {Nome = "Chaleco", Precio = 48.95M },
            new {Nome = "Pelota futbol", Precio = 19.50m },
            new {Nome = "Bandeira corner", Precio = 34.95M }
        };
            return View(productos.Select(p => $"Nome: {p.Nome}, Precio: {p.Precio}"));
            //A chamado ao metodo Select de LINQ xenera unha secuencia de strings, cada cal conten unha referencia hardcodeada
            //as propiedades Nome e Precio
            //Resultado
            /*
            Nome: Kayak, Precio: 275
            Nome: Chaleco, Precio: 48.95
            Nome: Pelota futbol, Precio: 19.50
            Nome: Bandeira corner, Precio: 34.95
            */

            //Esta forma de proceder e propensa a erros, ben porque o nome podes escribilo mal
            //ou porque se refactorizou o codigo e o nome no string non se actualizou correctamente
            //C# soporta a expresion nameof, na cal o compiler toma a responsabilidade
            //de producir un nome de string
        }

        public ViewResult ObterNamesConNameOf()
        {
            var productos = new[]
            {
            new {Nome = "Kayak", Precio = 275M },
            new {Nome = "Chaleco", Precio = 48.95M },
            new {Nome = "Pelota futbol", Precio = 19.50m },
            new {Nome = "Bandeira corner", Precio = 34.95M }
        };
            return View(productos.Select(p =>
            $"{nameof(p.Nome)}: {p.Nome}, {nameof(p.Precio)}: {p.Precio}"));
            //o resultado e o mesmo que antes, pero se cambiamos algunha vez a propiedade Nome a Denominacion, ou outra cousa
            //no exemplo anterior seguiria aparecendo como Nome
            //pero con nameof o compiler procesa de maneira que p.Nome so aparece coa ultima parte incluida no string
            //producindo a mesma saida que o exemplo anterior.
            //Hai soporte de IntelliSense para as expresions nameof,
            //asi que se che pedira seleccionar referencias, e as expresions estaran correctamente actualizadas
            //cando refactorizas o codigo.
            //Dado que o compiler e responsable de manexar nameof, usar unha referencia non valida cause un erro de compilacion
            //o cal evita que se che escapen referencias incorrectas ou desactualizadas

            //Todas estas cousas son caracteristicas de C# que un/unha programador/a de ASP.NET Core e bo que sepa
        }
    }
}