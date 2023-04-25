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
        TripleDES tripleDES = new TripleDES();

        string option;
        Console.WriteLine("Hello, World! This is 3DES");
        Console.WriteLine("Choose what do you want to do:");
        Console.WriteLine("1. Read from file data.txt \n2. Enter your text \n");
        option = Console.ReadLine();

        string text = userInteraction(option);
        Console.WriteLine($"CHOSEN TEXT: \n {text}");
/*
        byte[] plainText = Converter.StringToBytesUTF8(text);
        //wyspietlamy na razie tylko pierwszy klucz
        Console.WriteLine("generated Key {0} \nText in bytes: {1}\nBytes in text: {2}",Converter.BytesConverterToString(tripleDES.Key11.NewKey), Converter.BytesConverterToString(plainText), Converter.BytesConverterToUTF8(plainText));

        byte[] cypher = tripleDES.AlgorithmBase(plainText, tripleDES.Key11.SubKeys, tripleDES.Key21.SubKeys, tripleDES.Key31.SubKeys, true);
        Console.WriteLine("encrypted text: {0}", Converter.BytesConverterToUTF8(cypher));
        byte[] decrypt = tripleDES.AlgorithmBase(plainText, tripleDES.Key11.SubKeys, tripleDES.Key21.SubKeys, tripleDES.Key31.SubKeys, false);
        Console.WriteLine("decrypted text: {0}", Converter.BytesConverterToUTF8(decrypt));
    */}

}