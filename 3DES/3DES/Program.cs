
using _3DES;

internal class Program
{
    private const string Path = "..\\..\\..\\data.txt";

    private static string userInteraction(string choice)
    {
        string input = "";
        if (choice == "1")
        {
            try
            {
                StreamReader sr = new StreamReader(Path);
                input = sr.ReadLine();
                while (input != null)
                {
                    Console.WriteLine(input);
                    input = sr.ReadLine();
                }
                //close the file
                sr.Close();
                Console.ReadLine();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }
        }
        else if (choice == "2")
        {
            try
            {
                Console.WriteLine("Enter text:");
                input = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        else
        {
            Console.WriteLine("ERROR");
        }
        return input;
    }

    private static void Main(string[] args)
    {
        //Key key1 = new Key();

        string option;
        Console.WriteLine("Hello, World! This is 3DES");
        Console.WriteLine("Choose what do you want to do:");
        Console.WriteLine("1. Read from file data.txt \n2. Enter your text \n");
        option = Console.ReadLine();

        string text = userInteraction(option);
        Console.WriteLine($"CHOSEN TEXT: \n {text}");
        byte[] key = Key.GenerateTESTKey();
    }
}