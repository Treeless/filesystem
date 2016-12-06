//Matt Rowlandson
//Yusef Karim

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{
    public class Node
    {
        private List<string> files = new List<String>();

        public string directory { get; set; }
        public string fullPath { get; }
        public Node leftMostChild { get; set; }
        public Node rightSibling { get; set; }

        //Construct(directory)
        public Node(String directory) {
            this.directory = directory;
        }

        //Constructor(directory, leftMostChild)
        public Node(String directory, Node leftMostChild)
        {
            this.directory = directory;
            this.leftMostChild = leftMostChild;
        }

        //Adds a file to the files list
        public bool AddFile(String file) {
          for (int i = 0; i < files.Count; i++) {
                if (files[i] == file)
                {
                    return false;
                }
            }
            files.Add(file);
            return true;
        }

        //Removes a file from the files list
        public bool RemoveFile(String file) {
            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] == file)
                {
                    files.Remove(file);
                    return true;
                }
            }
            return false;
        }

        //Getter for files list
        public List<String> Files()
        {
            return files;
        }

		//Print entire Filesystem, goes through and prints each directory along with corresponding files
        public void PrintStructure(String spacing = "", bool ignoreSiblings = false) {
            string parentSpacing = spacing;
		
            //Print self and its files
			Console.WriteLine (spacing+directory);
			PrintFiles(spacing+"--");

            //Print underlying children's structure
            if(leftMostChild != null)
            {
                leftMostChild.PrintStructure(spacing+"--");
            }

            if (!ignoreSiblings)
            {
                //After printing my own structure. Go through the next right sibling and print their structure 
                Node sibling = rightSibling;

                if (sibling != null)
                {
                    sibling.PrintStructure(parentSpacing);
                }
            }
        }

		//Print out files in this node(directory)
        public void PrintFiles(String spacing = "") {
            foreach(String file in files){
                Console.WriteLine(spacing+file);
            }
        }
    }
}
