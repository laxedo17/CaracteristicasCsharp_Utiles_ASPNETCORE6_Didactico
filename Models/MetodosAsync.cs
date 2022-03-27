namespace CaracteristicasLinguaxe.Models
{
    public class MetodosAsync
    {
        public static Task<long?> GetPageLength()
        {
            HttpClient cliente = new HttpClient();
            var httpTask = cliente.GetAsync("http://aldea.com");
            return httpTask.ContinueWith((Task<HttpResponseMessage> antecedente) =>
            {
                return antecedente.Result.Content.Headers.ContentLength;
            });
            //tipico metodo async que usa return 2 veces con ContinueWith e pasa a outra Task
            //isto pode crear confusion en xente que programa
            //por iso Microsoft engadiu keywords a C# para simplificar os metodos asincronicos
        }

        //Codigo anterior simplificado con async e await
        public async static Task<long?> GetLonxitudePaxina()
        {
            HttpClient cliente = new HttpClient();
            var httpMensaxe = await cliente.GetAsync("http://aldea.com"); //usamos await cando chamamos ao metodo asincronico
            //aplicar await significa que podemos tratar o resultado do metodo GetAsync como se fose un metodo regular
            //e simplemente asignar o obxecto HttpResponseMessage ao que se fai return nunha variable
            //incluso millor, podemos usar a palabra clave return de forma normal para producir un resultado de outro metodo
            //neste caso do valor da propiedade ContentLength
            //asi non nos preocupamos do metodo ContinueWith e de multiples usos da palabra clave return
            return httpMensaxe.Content.Headers.ContentLength;
        }

        //Uso de Asynchronous Enumerable
        //Un asynchronous enumerable describe unha secuencia de valores que se xeneraran co tempo.
        //Para demostrar o problema que estas caracteristica soluciona, temos o codigo seguinte:

        /// <summary>
        /// Fai HTTP requests a unha serie de sitios web e obten a sua lonxitude. Os requests fanse asincronicamente, pero non hai forma de devolver os resultados ao metodo que chama segun chegan. En cambio, o que fai o metodo e esperar ata que os requests estan completos e devolve todos os resultados dunha sola vez.
        /// </summary>
        /// <param name="saida">List<string> a que engadimos mensaxes para demostrar como funciona o codigo.</param>
        /// <param name="urls"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<long?>> GetPaxinaLonxitudes(List<string> saida, params string[] urls)
        {
            List<long?> resultados = new List<long?>();
            HttpClient cliente = new HttpClient();
            foreach (string url in urls)
            {
                saida.Add($"Comenzou peticion de {url}");
                var mensaxeHttp = await cliente.GetAsync($"http://{url}");
                resultados.Add(mensaxeHttp.Content.Headers.ContentLength);
                saida.Add($"Completada peticion para {url}");
            }

            return resultados;
        }

        public static async IAsyncEnumerable<long?> GetPaxinaLonxitudesAsynchronousEnumerable(List<string> saida, params string[] urls)
        {
            HttpClient cliente = new HttpClient();
            foreach (string url in urls)
            {
                saida.Add($"Comenzou peticion de {url}");
                var httpMensaxe = await cliente.GetAsync($"http://{url}");
                saida.Add($"Completada peticion para {url}");
                yield return httpMensaxe.Content.Headers.ContentLength;
            }
        }
    }
}