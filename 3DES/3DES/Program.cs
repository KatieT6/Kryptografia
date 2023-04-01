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
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(Path);
                //Read the first line of text
                input = sr.ReadLine();
                //Continue to read until you reach end of file
                while (input != null)
                {
                    //write the line to console window
                    Console.WriteLine(input);
                    //Read the next line
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
        Console.WriteLine("Hello, World! This is 3DES");
        Console.WriteLine("1. Read from file data.txt \n2. Enter your text ");
        string option;
        Console.WriteLine("Choose what do you want to do:");
        option = Console.ReadLine();

        string text = userInteraction(option);
        Console.WriteLine($"CHOSEN TEXT: \n {text}");
        
    }
}