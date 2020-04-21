using System;
using System.IO;
using System.Linq;
using System.Globalization;
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

        public static string[] filePaths;
        public static string logfile;
    }
    class file
    {
        string[] Filetext;
        public string[] filetext
        {
            get
            {
                return Filetext;
            }
            set
            {
                Filetext = value;
            }
        }

    }
    // class program is where all the main code is
    class Program
    {
        public static string[] test = { "cake" };
        public static file file1text = new file();

        public static file file2text = new file();
        //this static void is used to print out the name of the files with the number assigned to them this is to allow for quick and easy file input by number
        static void direct()
        {
            int sublen = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Length;
            int i = 0;
            //simple while loop to loop until end of array command.filepaths
            while (i < command.filePaths.Length)
            {
                Console.WriteLine(i + "|" + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Substring(0,2) + "..." + command.filePaths[i].Substring(sublen));
                i++;
            }
        }
        // static void main asks the user to select a file this will be looped until a valid file is selected
        public static void Main(string[] args)
        {
            string local = Path.Combine((Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Substring(0, (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Length-23)), @"gits\");
            string files = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gits\");
            copy(local , files);
            //print a message to say that their is a list of files
            Console.WriteLine("here is a full list of files");
            //grab all files in the directiory of gits even in diffrent folders that end in .txt
            command.filePaths = Directory.GetFiles(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"gits\"), "*.txt", SearchOption.AllDirectories);
            //write the total size this was used for debugging and is now commented out to allow me to uncomment it if i ran into a problem
            Console.WriteLine(command.filePaths.Length);
            //issue the direct void to allow for file priting
            direct();
            //mentions the name of the command and how to use it
            Console.WriteLine("use git diff and the number of 2 files to pick which one you would like to use");
            // while command.triedcommand is not equal to any command loop this is to do error handeling
            while (command.triedcommand == "")
            {
                //display > on the line to look profesinal 
                Console.Write(">");
                triedcommandcheck();
            }
            filecreate();
            // check if values in both array are the same
            Check();
        }
        public static void copy(string source, string dest)
        {
            DirectoryInfo dir = new DirectoryInfo(source);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + source);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }
            FileInfo[] files = dir.GetFiles();
            foreach(FileInfo file in files)
            {
                string tempp = Path.Combine(dest, file.Name);
                file.CopyTo(tempp, true);
            }
            foreach(DirectoryInfo subdir in dirs)
            {
                string tempp = Path.Combine(dest, subdir.Name);
                copy(subdir.FullName, tempp);
            }
        }

        public static void filecreate()
        {
            // create a string of the names to create a file
            string comparisons = (command.git1.ToString() + "_" + command.git2.ToString());
            //puts string together
            bool exists = Directory.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Logs\" + comparisons));
            // if the fold does note exist create folder
            if (!exists)
                Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Logs\" + comparisons + "_Logs"));
            //create name for the file
            command.logfile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Logs\" + comparisons + "_Logs" + '\\' + "log_" + DateTime.Now.ToString("yyyy_MM_dd_HH__mm_ss") + ".txt");
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
                char[] values = input.ToCharArray();
                // error check to see if numbers
                check1 = int.TryParse(values[0].ToString(), out command.git1);
                check2 = int.TryParse(values[1].ToString(), out command.git2);
                // if it is a valid number then load in the right repos
                if (check1 && check2 && (command.git1 < command.filePaths.Length && command.git1 >= 0) && (command.git2 < command.filePaths.Length && command.git2 >= 0))
                {
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
            string[] temp = { "" };
            //switch case for which repo they want to use
            path1 = command.filePaths[command.git1];
            path2 = command.filePaths[command.git2];
            StreamReader file1 = new StreamReader(path1);
            StreamReader file2 = new StreamReader(path2);
            // add them to the valid arrays
            while ((line1 = file1.ReadLine()) != null)
            {
                Array.Resize(ref temp, (count + 1));
                temp[count] = line1;
                count++;
            }
            file1text.filetext = temp;
            Array.Resize(ref temp, 0);
            count = 0;
            while ((line2 = file2.ReadLine()) != null)
            {
                Array.Resize(ref temp, (count + 1));
                temp[count] = line2;
                count++;
            }
            file2text.filetext = temp;
            file1.Close();
            file2.Close();


        }
        //checks if the files are the same and prints them out
        static void Check()
        {
            //calls charcheck on the values
            charcheck();

            // if the value of file1 and file2 the same then say the files are the same 
            bool isEqual = Enumerable.SequenceEqual(file1text.filetext, file2text.filetext);
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
        // in char check we split it into words and check the indervidual 
        public static void charcheck()
        {
            // load a file with streamwriter to allow for writing
            using (StreamWriter sw = new StreamWriter(command.logfile))
            {
                //create to varibles for pluss's and takeaways
                int diffrencep = 0;
                int diffrencet = 0;
                //start a count used for every line with a value of 0
                int count = 0;
                //get the minimum length of the files so it does not cause a stack overflow
                int leg1 = Math.Min(file1text.filetext.Length, file2text.filetext.Length);
                //while a count is smaller than the length
                while (count < leg1)
                {
                    // create varibles for this section for finding leg2 and a counter and the sentence being split up into words
                    string[] words = file1text.filetext[count].Split();
                    string[] words2 = file2text.filetext[count].Split();
                    int count2 = 0;
                    int leg2 = Math.Min(words.Length, words2.Length);
                    while (count2 < leg2)
                    {
                        //if the words are the same print out the words
                        Console.ForegroundColor = ConsoleColor.White;
                        if (words[count2] == words2[count2])
                        {
                            Console.Write(words2[count2] + " ");
                        }
                        //if difffrent do this section
                        else
                        {   //if the value does not apear of has been a change write in red then write the new section in green
                            if (Array.IndexOf(words2, words[count2], count2 - 1) == -1)
                            {
                                string change = words[count2];
                                int x = 1;
                                //small section to check if any extra words are their so it is a new section
                                while (Array.IndexOf(words2, words[count2 + x], count2 - 1) == -1)
                                {
                                    change = change + words[count2 + x];
                                    x++;
                                }
                                string change2 = words2[count2];
                                x = 1;
                                while (Array.IndexOf(words, words2[count2 + x], count2 - 1) == -1)
                                {
                                    change2 = change2 + " " + words2[count2 + x];
                                    x++;
                                }
                                //add the diffrences in
                                diffrencep++;
                                diffrencet++;
                                //print the changes out
                                Console.ForegroundColor = ConsoleColor.Red;
                                sw.WriteLine("Line | " + count + " | word | " + count2 + " --" + change + " ++" + change2);
                                Console.Write("( " + change + " ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(change2 + " )");

                            }

                            //if the words is just wrong becasue if is off by a few words then simply print out white as the value was their before
                            else
                            {
                                Console.Write(words[count2] + " ");
                            }


                        }
                        count2++;
                    }
                    //if there is words not there in file two print them out in red to show they are gone
                    if (words.Length > leg2)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        int x = count2;
                        while (x < words.Length)
                        {
                            diffrencet++;
                            Console.Write(words[x] + " ");
                            x++;
                        }
                    }//if there is a words there then print out in green to show additons
                    else if (words2.Length > leg2)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        int x = count2;
                        while (x < words2.Length)
                        {

                            diffrencep++;
                            Console.Write(words2[x] + " ");
                            x++;
                        }
                    }
                    Console.WriteLine("  ");
                    count++;
                }//if there is a lines there then print out in green to show additons
                while (count < file2text.filetext.Length)
                {

                    diffrencep++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(file2text.filetext[count]);
                    count++;
                }

                //if there is line not there in file two print them out in red to show they are gone
                while (count < file1text.filetext.Length)
                {

                    diffrencet++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(file2text.filetext[count]);
                    count++;
                }
                sw.Write("changes " + (diffrencet + diffrencep) + " Addiitons " + diffrencep + " Removals " + diffrencet + "\n");
                Console.WriteLine("log is avalible here " + command.logfile);
                string files = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Logs\");
                string local = Path.Combine((Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).Substring(0, (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Length - 23)), @"Logs\");
                copy(local, files);
            }
        }


    }

}