namespace adonet_db_videogame;

class Program
{
    static void Main(string[] args)
    {
        string connectionDatabase = "Data Source=localhost,1433;Database=videogames_db;User Id=sa;Password=dockerStrongPwd123;";
        VideogameManager manager = new VideogameManager(connectionDatabase);
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Inserisci nuovo videogioco");
            Console.WriteLine("2. Ricerca videogioco per ID");
            Console.WriteLine("3. Ricerca videogiochi per nome");
            Console.WriteLine("4. Cancella videogioco");
            Console.WriteLine("5. Chiudi programma");
            Console.Write("Seleziona un'opzione: ");

            try
            {
                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.Write("Inserisci il nome del videogioco: ");
                        string? name = Console.ReadLine();
                        Console.Write("Inserisci la descrizione del videogioco: ");
                        string? overview = Console.ReadLine();
                        Console.Write("Inserisci la data di rilascio del videogioco: ");
                        DateTime releaseData = Convert.ToDateTime(Console.ReadLine());
                        manager.InsertVideogame(new Videogame(name, overview, releaseData));
                        break;
                    case 2:
                        Console.Write("Inserisci l'ID del videogioco: ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        var game = manager.GetVideogameById(id);
                        if (game != null)
                        {
                            Console.WriteLine(game);
                        }
                        else
                        {
                            Console.WriteLine("Videogioco non trovato.");
                        }
                        break;
                    case 3:
                        Console.Write("Inserisci il nome del videogioco da cercare: ");
                        string? searchName = Console.ReadLine();
                        var results = manager.SearchVideogameByName(searchName);
                        if (results.Count > 0)
                        {
                            Console.WriteLine("Videogiochi trovati:");
                            results.ForEach(Console.WriteLine);
                        }
                        else
                        {
                            Console.WriteLine("Nessun videogioco trovato.");
                        }
                        break;
                    case 4:
                        Console.Write("Inserisci il nome del videogioco da cancellare: ");
                        string? deleteName = Console.ReadLine();
                        manager.DeleteVideogame(deleteName);
                        break;
                    case 5:
                        running = false;
                        Console.WriteLine("Programma chiuso.");
                        break;
                    default:
                        Console.WriteLine("Opzione non valida.");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Errore: input non valido.");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Errore: {e.Message}");
            }
        }
    }
}

