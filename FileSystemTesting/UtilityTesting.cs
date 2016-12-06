using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileSystem;

namespace FileSystemTesting
{
    [TestClass]
    public class UtilityTesting
    {
        [TestMethod]
        public void ProperGetFileFromAddress()
        {
            String file = Utility.GetFileFromAddress("/foo/bar/diddles.txt");
            Assert.AreEqual("diddles.txt", file);
        }

        [TestMethod]
        public void ProperGetDirectoriesFromAddress() {
            String[] directories = Utility.getDirectoriesFromAddress("/foo/bar/diddles");
            Assert.AreEqual("foo", directories[1]);
        }

        [TestMethod]
        public void removeFileFromDirArr()
        {
            String[] dir = new String[4];
            dir[0] = "/";
            dir[1] = "foo/";
            dir[2] = "bar/";
            dir[3] = "file.text";
            String[] directories = Utility.removeFileFromDirArr(dir);
            Assert.AreEqual(directories.Length, dir.Length-1);
            Assert.AreNotEqual(directories[directories.Length-1], dir[dir.Length-1]);
        }
    }
}
