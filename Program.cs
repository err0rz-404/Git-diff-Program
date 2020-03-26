using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Git_diff
{
    // class command is used to store all my local variables
    class command
    {
        // a variable for the ability to have commands
        public static string triedcommand = "";
        // a variable for the repositorie that they want to check if they are the same
        public static int git1;
        public static int git2;
        // two classes for an array of the files
        public static string[] file1;

        public static string[] file2;
        public static string[] filePaths;
    }
    // class program is where all the main code is
    class Program
    {
        static void direct() { 
            int i = 0; 
            while (i < command.filePaths.Length)
            {
                Console.WriteLine(i + " " + command.filePaths[i]);
                i++;
            }
        }
        // static void main asks the user to select a file this will be looped until a valid file is selected
        static void Main(string[] args)
        {
            
            Console.WriteLine("here is a full list of files");
            command.filePaths = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gits\"), "*.txt", SearchOption.AllDirectories);
            Console.WriteLine(command.filePaths.Length);
            direct();
            Console.WriteLine("use git diff and the number of 2 files to pick which one you would like to use");
            // while command.triedcommand is not equal to any command loop
            while (command.triedcommand == "") { 
            Console.Write(">");
                triedcommandcheck();
            }
            // check if values in both array are the same
            Check();
        }

        
        // this wis where i check if the command exists
        static void triedcommandcheck()
        {
            command.triedcommand = Console.ReadLine();
            // if its less than 4 then make it the alphabet this was just for a refrence string
            if (command.triedcommand.Length < 4)
            {
                command.triedcommand = "abcdefghijklmnopqrstuvwxyz";
            }
            // if the string == help then tell the user what commands are avalible
            if (command.triedcommand.Substring(0, 4) == "help")
            {
                Console.WriteLine("Here is a list of commands");
                Console.WriteLine("git diff <git>");
                command.triedcommand = "";
            }
            // if not help and is smaller than 8 then make it the alphabet
            if (command.triedcommand.Length < 8)
            {
                command.triedcommand = "abcdefghijklmnopqrstuvwxyz";
            }
            // if command is git diff then run this section
            if (command.triedcommand.Substring(0, 8) == "git diff")
            {
                // quick check to see if a valid number is selected
                bool check1;
                bool check2;
                // create a string that gets the last values of the text
                string input = (command.triedcommand.Substring(command.triedcommand.Length - 4));
                // strip that line down to have no spaces
                input = input.Replace(" ", String.Empty);
                // create a char array of the values to split it and make it a single value
                char[]  values = input.ToCharArray();
                // error check to see if numbers
                check1 = int.TryParse(values[0].ToString(), out command.git1);
                check2 = int.TryParse(values[1].ToString(), out command.git2);
                // if it is a valid number then load in the right repos
                if (check1 && check2 && (command.git1 <command.filePaths.Length && command.git1 >= 0) && (command.git2 < command.filePaths.Length && command.git2>=0)) { 
                Console.WriteLine(command.git1);
                    load();
                }
                // else tell them its invalid and re loop
                else
                {
                    Console.WriteLine("> please pick a valid git repo the ones avalible");
                    direct();
                    command.triedcommand = "";
                }
            }
            // let the user know to pick a valid comand
            else if (command.triedcommand != "git diff" || command.triedcommand != "help")
            {
                Console.WriteLine("> pick a valid command");
                command.triedcommand = "";
            }
        }
        // load in the right variubles
        static void load()
        {
            int count = 0;
            string path1 = "";
            string path2 = "";
            string line1;
            string line2;

            //switch case for which repo they want to use
            path1 = command.filePaths[command.git1];
            path2 = command.filePaths[command.git2];
            StreamReader file1 = new StreamReader(path1);
            StreamReader file2 = new StreamReader(path2);
            // add them to the valid arrays
            while(( line1 = file1.ReadLine()) != null && (line2 = file2.ReadLine()) !=null)
            {
                Array.Resize(ref command.file1, (count + 1)); Array.Resize(ref command.file2, (count + 1));
                command.file1[count] = line1; command.file2[count] = line2;
                count++;
            }
        }
        //checks if the files are the same and prints them out
        static void Check()
        {
            // if the value of file1 and file2 the same then say the files are the same 
            bool isEqual = Enumerable.SequenceEqual(command.file1, command.file2);
            if (isEqual)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The files are the same");
            }
            //else tell them it is not the same 
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The files are NOT the same");
            }
        }
    }
}
