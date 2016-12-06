//Matt Rowlandson
//Yusef Karim

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    public class FileSystem
    {
		//Top of the filesystem, root directory
        public Node root { get; }

		//To keep track of number of total files
		private int count;
		public int Count
		{
			get
			{
				return this.count;
			}
			set
			{
				count = value;
			}
		}

        // Creates a file system with a root directory
        public FileSystem() {
            //top most directory
            root = new Node("/");
			count = 0;
        }

        // Adds a directory at the given address (also used for checking if directory exists
        //Rather then returning true or false. We return the node we were last on.
        //The ignoreAdd paramater is used for when checking if a directory exists or not. It stops the adding of a node
        //NOTE TO GRADER: Although this returns a NODE rather then a BOOL. A node would equal to true, where as returning null would equal false. 
        //                Therefore it can be used like it returns a bool if needed...
        public Node AddDirectory(string address, bool ignoreAdd = false)
        {
            //ex /foo/bar/diddles/

            //Split up the address into its directories
            String[] directories = Utility.getDirectoriesFromAddress(address);

            Node current = root;
			//Depth of leftMostChild
            int index = 0;
            String currentDir = directories[index];
            int dirCount = directories.Length;
            Node beforeNode = null;

            while (current != null) {
                //Go through the directory list as deep as we can go.
                //Each time we come across a directory that does not exist. 
                //Create it and remove it from the directories array
                //Until we get to the last directory

                //If the current directory is the directory 
                if (current.directory == currentDir)
                {
                    if (index + 1 >= directories.Length) {
                        //We have reached the bottom most directory todo, it is finished
                        if(ignoreAdd && current.directory != currentDir)
                            return null;
                        break;
                    }

                    currentDir = directories[++index];

                    //Directory already exists, go into it
                    if (currentDir != null){

                        if (current.leftMostChild == null) {
                            if(ignoreAdd)
                                return null; //Dont add
 
                            //We need create that node
                            current.leftMostChild = new Node(currentDir);
                            beforeNode = current;
                            current = current.leftMostChild;
                            continue;
                        }
                        else
                        {
                            beforeNode = current;
                            current = current.leftMostChild;
                            continue;
                        }
                    }
                    else
                    {
                        //we are at the directory we need to create
                        //Already exists. Get out
                        break;
                    }
                }
                else
                {
                    //Go to the next right most child
                    Node sibling = current.rightSibling;
                    if (sibling == null)
                    {

                        if (ignoreAdd)
                            return null; //Dont add.

                        //Create the new directory as a sibling
						current.rightSibling = new Node (currentDir);
						beforeNode = current;
						current = current.rightSibling;
						continue;
                    }
                    else
                    {
                        Node before = sibling;

                        //Go through all the siblings till we get to the last or we found the sibling
                        bool exists = false;
                        while (sibling != null)
                        {
                            //No sibling
                            if (sibling == null)
                            {
                                break;
                            }
                            //The sibling is equal to the current directory we are trying to currently add
							else if (sibling.directory == currentDir)
                            {
                                //Exists
								exists = true;
								current = sibling;
                                if(index == directories.Length-1)
                                {
                                    return current;//Final directory already exists. Done.
                                }
                                else
                                {
                                    //Go deeper in the outer while loop.
                                    break;
                                }
                            }

                            before = sibling;
                            sibling = sibling.rightSibling;
                        }

						if (exists == false) {
							if (ignoreAdd)
								return null; //Dont add, doesnt exist

							//Create the  new node at specified position, moves current to be this node
							before.rightSibling = new Node (currentDir);
							beforeNode = current;
							current = before.rightSibling;
							continue;
						} 
                    }
                }
            }

			//if ignoreAdd=true, returns the node before the file we want to add, else we are not adding a file, returns current directory added
            if (ignoreAdd)
                return beforeNode;
            else
                return current;
            //Done
        }

        // Adds a file at the given address 
        //(if the directory doesn't exist. We choose to create it. It's more useful that way)
        public bool AddFile(string address) {
            //ex /foo/bar/ugly.txt
            String file = Utility.GetFileFromAddress(address);
            if (file == null)
                return false;
            String[] dir = Utility.getDirectoriesFromAddress(address);
            //Turn the directory arry into string
            String directory = "";
            foreach(string str in dir)
            {
                directory = directory + "/" + str;
            }


           //Find the directory at the given address 
           //If it doesn't exist. Create it. returns the node
           Node dirNode = AddDirectory(directory);

            //Add the file
            bool added = dirNode.AddFile(file);
			//Increment count for number of files
			if (added == true) {
				this.count++;
			}

            // Returns false if file already exists. True otherwise.
            return added;
        }

        // Removes the file at the given address
        // Returns false if the file is not found or the path is undefined; true otherwise
        public bool RemoveFile(string address) {
            //ex /foo/bar/ugly.txt
            String file = Utility.GetFileFromAddress(address);
            if (file == null)
                return false;
            String[] dir = Utility.getDirectoriesFromAddress(address);
            //Turn the directory arr into string
            String directory = "";
            foreach (string str in dir)
            {
                directory = directory + "/" + str;
            }


            //Find the directory at the given address 
            Node dirNode = AddDirectory(directory, false); //Don't add the directory as we do by default
            if (dirNode == null)
                return false;

            //Remove the file
            bool removed = dirNode.RemoveFile(file);
			//Removes file from total count of files
			if (removed == true) {
				this.count--;
			}

            // Returns false if file doesn't exists. True otherwise.
            return removed;
        }

        //Go through and verify the directory we are looking for exists
        public bool DirectoryExists(String address) {
            //We will try to add that directory. Without actually adding it. If we try to add it. We know it doesn't exist.
            Node node = AddDirectory(address, true);
            if (node == null)
                return false;

            return true;
        }

        // Removes the directory (and its subdirectories) at the given address
        // Returns false if the directory is not found or the path is undefined; true otherwise
        public bool RemoveDirectory(string address) {
            String[] directories = Utility.getDirectoriesFromAddress(address);

            Node current = root;
            int index = 0;
            String currentDir = directories[index];
            int dirCount = directories.Length;
            Node before = null;
            while (current != null)
            {
                //Go through the directory list as deep as we can go.
                //Each time we come across a directory that does not exist. 
                //Create it and remove it from the directories array
                //Until we get to the last directory

                //If the current directory is the directory 
                if (current.directory == currentDir)
                {
                    if (index + 1 >= directories.Length)
                    {
                        //We have reached the bottom most directory todo, it is finished
                        //Snip and reconnect siblings and parents as needed
                        if(current.rightSibling != null)
                        {
                            current = current.rightSibling;
                            before.leftMostChild = current;
                        }else
                        {
                            before.leftMostChild = null;
                        }

                        //Deleted
                        return true;
                    }

                    currentDir = directories[++index];

                    //Directory already exists, go into it
                    if (currentDir != null)
                    {

                        if (current.leftMostChild == null)
                        {
                            //As we are down, we shoulnd't hit a null. Otherwise path undefined
                            return false;
                        }
                        else
                        {
                            before = current;
                            current = current.leftMostChild;
                            continue;
                        }
                    }
                }
                else
                {
                    //Go to the next right most child
                    Node sibling = current.rightSibling;
                    if (sibling == null)
                    {
                        //we can't keep traversing. path undefined.
                        return false;
                    }
                    else
                    {
                        //Go through all the siblings till we get to the last

                        while (sibling != null)
                        {
                            sibling = sibling.rightSibling;

                            //No sibling
                            if (sibling == null)
                            {
                                break;
                            }

                            if (sibling.directory == currentDir)
                            {
                                //Delete this node, via a snip
                                current.rightSibling = sibling.rightSibling;
                                return true;
                            }
                        }

                        //Didn't delete and we got to the end of the siblings
                        return false;
                    }
                }
            }
            return false;
        }

        // Returns the number of files in the file system
        public int NumberFiles() {
            //everytime you add a file. Increment a count. return the count here.
            return this.count;
        }

        // Prints the directories in a pre-order fashion along with their files
        public void PrintFileSystem() {
            //Traverse and print:
            root.PrintStructure("");
        }

    }
}
