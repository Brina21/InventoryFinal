namespace InventoryFinal.EscribirLogsFichero
{
    public class EscribirFichero
    {
        private static string directorio = @"C:Logs";
        private static string fichero = "log.txt";
        private string pathFichero = Path.Combine(directorio, fichero);

        // Constructor que crea el directorio y el fichero si no existen
        public EscribirFichero()
        {
            

            if (!Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            if (!File.Exists(pathFichero))
            {
                File.Create(pathFichero).Close();
            }
        }

        public static void Escribir(string mensaje)
        {
            using (StreamWriter sw = new StreamWriter(fichero, append: true))
            {
                sw.WriteLine($" {DateTime.Now} -> {mensaje}");
            }
        }
    }
}
