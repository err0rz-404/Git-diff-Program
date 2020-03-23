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
        public static int git;
        // two classes for an array of the files
        public static string[] file1;

        public static string[] file2;
    }
    // class program is where all the main code is
    class Program
    {
        // static void main asks the user to select a file this will be looped until a valid file is selected
        static void Main(string[] args)
        {
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
            if (command.triedcommand.Length< 4 || command.triedcommand.Length < 8)
            {
                command.triedcommand = "abcdefghijklmnopqrstuvwxyz";
            }
            // if command is git diff then run this section
            if (command.triedcommand.Substring(0, 8) == "git diff")
            {
                // quick check to see if a valid number is selected
                bool check;
                check = int.TryParse(command.triedcommand.Substring(8, command.triedcommand.Length - 8), out command.git);
                // if it is a valid number then load in the right repos
                if (check && command.git <4 && command.git > 0) { 
                Console.WriteLine(command.git);
                    load();
                }
                // else tell them its invalid and re loop
                else
                {
                    Console.WriteLine("> please pick a valid git repo the ones avalible are 1, 2 or 3");
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
            string path = "";
            string path2 = "";
            string line1;
            string line2;
            //switch case for which repo they want to use
            switch (command.git)
            {

                case 1:
                    // get location of txt file at the location of where this text file is saved
                    path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gits\GitRepositories_1a.txt");
                    path2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gits\GitRepositories_1b.txt");
                    break;
                case 2:
                    path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gits\GitRepositories_2a.txt");
                    path2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gits\GitRepositories_2b.txt");
                    break;
                case 3:
                    path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gits\GitRepositories_3a.txt");
                    path2 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gits\GitRepositories_3b.txt");
                    break;
            }
            StreamReader file1 = new StreamReader(path);
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
                Console.WriteLine("The files are the same");
            }
            //else tell them it is not the same 
            else
            {
                Console.WriteLine("The files are NOT the same");
            }
        }
    }
}
