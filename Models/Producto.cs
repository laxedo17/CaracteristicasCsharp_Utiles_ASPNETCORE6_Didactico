namespace CaracteristicasLinguaxe.Models
{
    public class Producto
    {
        public string Nome { get; set; } = string.Empty; //asignando valor por defecto para evitar valor null
        public decimal? Precio { get; set; }

        //Lambda expression para unha property
        public bool NomeComenzaConP => Nome?[0] == 'P';

        //property moi similar a anterior, sen lamdba expression
        public bool NomeComenzaConS
        {
            get
            {
                return Nome?[0] == 'S';
            }
        }

        public static Producto?[] GetProductos()
        {
            //Object initializer
            //Permite crear un obxecto e especificar os valores das propiedades nun unico paso
            //esta e unha caracteristica syntactic sugar que fai que C# sexa mais facil de usar
            //se un object initializer teriamos que chamar ao constructor de Producto
            //e usar o novo obxecto creado para establecer cada unha das suas propiedades
            //tal que asi:
            /*
            Producto kayak = new Producto();
            kayak.Nome = "Kayak";
            kayak.Precio = 275M;
            */
            Producto kayak = new Producto { Nome = "Kayak", Precio = 275M };
            //Object initializer
            Producto chaleco = new Producto { Nome = "Chaleco salvavidas", Precio = 275M };

            return new Producto?[] { kayak, chaleco, null };

            //Collection initiliazer
            //De forma similar a object initializer, permite a creacion dunha coleccion e o seu contido, nun paso
            //Sen un initializer, crear un string array, por ex., require o tamanho do array
            //e os seus elementos especificados de forma separada
            //codigo de exemplo que poderia ir en HomeController.cs
            /*
            string[] nomes = new string[3];
            nomes[0] = "Perico";
            nomes[1] = "Janice";
            nomes[2] = "Alicia";
            return View("Index", nomes);
            */

            //equivalente con Collection Initializer
            /*
            return View("Index", new string[] { "Perico", "Janice", "Alicia" });
            */
        }
    }
}