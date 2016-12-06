//Matt Rowlandson
//Yusef Karim

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystem
{   
    //Used for quick functions (mostly parsing)
    public class Utility
    {
        //Parses the string and returns the filename within the address.
        public static String GetFileFromAddress(String address) {
            //Check if there is a start / if so, remove it
            address = address.TrimStart('/', ' ');
            //Check if there is a end / if so, remove it
            address = address.TrimEnd('/', ' ');

            if (address.IndexOf('.') == -1)
            {
                return null; //no file
            }
            String[] foo = address.Split('/');

            for (int i = 0; i < foo.Length; i++)
            {
                if (i == foo.Length - 1 && foo[i].IndexOf(".") > -1)
                {
                    //then its a file. return it
                    return foo[i];
                }
            }

            return null;
        }

        //Gets the directories of the address
        //Format ["/", "foo", "bar"];
        public static String[] getDirectoriesFromAddress(String address) {
            //Check if there is a start / if so, remove it
            address = address.TrimStart('/', ' ');
            //Check if there is a end / if so, remove it
            address = address.TrimEnd('/', ' ');

            int length = address.Split('/').Length + 1;
            if (address.IndexOf('.') > -1)
            {
                length--;
            }
            String[] tmp = new String[length];
            tmp[0] = "/";

            String[] foo = address.Split('/');

            for (int i = 0; i < foo.Length; i++)
            {
                if (i == foo.Length - 1 && foo[i].IndexOf(".") > -1)
                {
                    //then its a file. Strip it
                    continue;
                }
                tmp[i + 1] = foo[i];
            }

            return tmp;
        }//Done

        //Strips the file from the dir array
        //ex ['foo', 'bar', 'diddles', 'file1.txt']
        public static String[] removeFileFromDirArr(String[] dir) {
            String[] tmp = new String[dir.Length-1];

            int index = 0;
            foreach(string str in dir) {
                if(index == dir.Length - 1)
                {
                    break;
                }
                tmp[index++] = str;
            }

            return tmp;
        }

        //Used by front end. Parses the relative address as needed.
        public static string parseRelativeAddress(String currentAddress, String relative)
        {
            if(relative.IndexOf("/..") == -1 && relative.IndexOf("./") == -1)
                return relative;

            int levels = 0;
            String upALevel = "/..";
            String atRoot = "./";

            //special case '..'
            if (relative == upALevel)
            {
                levels++;
            }
            else if (relative.IndexOf(atRoot) > -1)
            {
                String path = relative.Split(new[] { "./" }, StringSplitOptions.None)[1];
                return path;
            }
            else
            {
                while (relative.IndexOf(upALevel) > -1)
                {
                    levels++;
                    int index = relative.IndexOf(upALevel);
                    relative = relative.Remove(index, 3);
                }
            }

            while(levels != 0)
            {
                levels--;
                int index = currentAddress.LastIndexOf('/');
                currentAddress = currentAddress.Remove(index, currentAddress.Length- index);
            }

            if (currentAddress == "")
            {
                currentAddress = "/"; //highest we can go is root
            }
            return currentAddress;
        }
    }
}
