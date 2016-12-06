//Matt Rowlandson
//Yusef Karim

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    class Program
    {
        private static FileSystem fs;
        private static String current = "/"; //Current directory level

        private static String[] commands = new String[] {"help", "current", "cd", "ls", "mkdir", "rm", "touch", "exit" };

        static void Main(string[] args)
        {
            SetupFileStructure();
            DisplayCommands();

            /*
			fs.AddDirectory ("/dude");
			fs.AddDirectory ("/dude/tralal");
			fs.AddDirectory ("/guy/");
			fs.AddDirectory ("/guy/work");
			fs.AddDirectory ("/guy/workplease");
			fs.AddDirectory ("/guy/work/thing");
			fs.AddFile ("/dude/1.txt");
			fs.AddFile ("/dude/2.txt");
			fs.AddDirectory ("/foo");
			fs.AddFile ("/foo/3.txt");
			fs.AddFile ("/foo/4.txt");
			fs.AddDirectory ("/foo/bar");
			fs.AddDirectory ("/foo/dudeman");
			fs.AddDirectory ("/foo/bar/stuff");
			fs.AddFile ("/foo/bar/5.txt");
			fs.AddDirectory ("/foo/car");

			fs.PrintFileSystem ();
			*/

            //user input loop (infinite loop)
            while (true) {
				Console.Write ("[User@localmachine]$ ");
				getUserInput(Console.ReadLine());

            }
          
        }

        //Intializes the file system
        private static void SetupFileStructure() {
            fs = new FileSystem();
        }

        private static void DisplayCommands() {
            Console.WriteLine("--------- COMMANDS -----------");
            Console.WriteLine("help            : list commands");
            Console.WriteLine("current         : print current address");
            Console.WriteLine("cd <address>    : change directory to the specified directory");
            Console.WriteLine("cd ..           : moves up one directory from the current directory.");
            Console.WriteLine("ls              : Prints all directories and files");
            Console.WriteLine("mkdir <address> : Creates directory from the current address");
            Console.WriteLine("rm <address>    : Removes the specified directory or file");
            Console.WriteLine("touch <file>    : Adds file to the current address");
            Console.WriteLine("exit            : exits the application");
            Console.WriteLine("------------------------------");
            Console.WriteLine("Note: This GUI keeps track of where you are in the current directory");
            Console.WriteLine("      If you want, to add from root. put a './' infront of your address");
            Console.WriteLine("");
        }

        private static string getRequiredParam(String input)
        {
            String[] strArr = input.Split(' ');
            if (strArr.Length == 2)
            {
                return strArr[1];
            }
            else
            {
                Console.WriteLine("Invalid Command: "+strArr[0]+" requires an additional param");
                return null;
            }
        }

        private static string getOptionalParam(String input)
        {
            String[] strArr = input.Split(' ');
            if (strArr.Length == 2)
            {
                return strArr[1];
            }
            else
            {
                return null;
            }
        }

        private static void getUserInput(String input) {
            //Verify commands
            int found = -1;
            int index = 0;
            foreach(String cmd in commands)
            {
                if (input.IndexOf(cmd) > -1)
                {
                    found = index;
                    break;
                }
                index++;
            }

            if (found != -1)
            {
                string param;
                switch (commands[found])
                {
                    case ("help"):
                        DisplayCommands();
                        break;
                    case ("current"):
                        Console.WriteLine(current);
                        break;
                    case ("cd"):
                        //Takes extra param: address (required)
                        param = getRequiredParam(input);
                        if(param != null)
                        {
                            String fullpath = current + param;
                            try
                            {
                                if(param == current)
                                {
                                    //Already at that directory. return
                                    break;
                                }

                                if(param.IndexOf('/') == -1 && current.LastIndexOf("/") != current.Length-1)
                                {
                                    param = "/"+param;
                                }

                                //Check for relative link
                                try
                                {
                                    //Special case (back to root)
                                    if (param == "./")
                                    {
                                        current = "/";
                                        return;
                                    }
                 
                                    param = Utility.parseRelativeAddress(current, param);
                                }
                                catch (Exception e) { }

                                String slash = "";
                                if (current.LastIndexOf('/') != current.Length-1 && param.IndexOf('/') != 0)
                                {
                                    slash =  "/";
                                }
                                fullpath = current + slash + param;
                                bool exists = fs.DirectoryExists(fullpath);
                                if (exists)
                                {
                                    current = fullpath;
                                }
                                else
                                    Console.WriteLine("Directory doesnt exist: " + fullpath);
                            }
                            catch (Exception e) {
                                Console.WriteLine("Invalid directory address: " + fullpath);
                            }
                        }
                        break;
                    case ("ls"):
                        fs.PrintFileSystem();
                        break;
                    case ("mkdir"):
                        //Takes extra param: address (required)
                        param = getRequiredParam(input);

                        if(param != null)
                        {

                            //Check for relative link
                            try
                            {
                                param = Utility.parseRelativeAddress(current, param);
                            }
                            catch (Exception e) { }
                            String slash = "";
                            if(param.IndexOf('/') != 0 && current.LastIndexOf('/') != current.Length-1)
                            {
                                slash = "/";
                            }
                            param = current + slash + param;
                            bool exists = fs.DirectoryExists(param);

                            if (exists)
                            {
                                Console.WriteLine("That directory already exists");
                            }
                            else
                            {
                                Node node = fs.AddDirectory(param);
                                if(node == null)
                                {
                                    Console.WriteLine("Could not make the directory");
                                }
                            }
                        }

                        break;
                    case ("rm"):
                        //Takes extra param: address (required)
                        param = getRequiredParam(input);
                        if (param != null)
                        {
                            //Check for relative link
                            try
                            {
                                param = Utility.parseRelativeAddress(current, param);
                            }
                            catch (Exception e) { }

                            bool removed;
                            if (param.IndexOf('.') > -1)
                            {
                                removed = fs.RemoveFile(param);
                            }
                            else
                            {
                                removed = fs.RemoveDirectory(param);
                            }

                            if (!removed)
                            {
                                Console.WriteLine("Could not remove, does not exist");
                            }
                        }
                        break;
                    case ("touch"):
                        //Takes extra param: file (required)
                        param = getRequiredParam(input);
                        if(param != null)
                        {
                            //Check for relative link
                            try
                            {
                                param = Utility.parseRelativeAddress(current, param);
                            }
                            catch (Exception e) { }

                            //Make sure file has an extension
                            if (param.IndexOf('.') > -1)
                            {
                               bool added =  fs.AddFile(current + "/" + param);
                                if (!added)
                                {
                                    Console.WriteLine("Could not add the file. Already exists");
                                }
                            }else
                            {
                                Console.WriteLine("Could not create file. Needs an extension. ex file.txt");
                            }
                        }
                        break;

                    case ("exit"):
                        System.Environment.Exit(1);
                        break;
                    default:
                        Console.WriteLine("Command not implemented...");
                        break;
                }
            }else
            {
                Console.WriteLine("Invalid Command Given: " + input);
            }
        }
    }
}
