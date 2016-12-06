//Matt Rowlandson
//Yusef Karim

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSystemTesting
{
    [TestClass]
    public class FileSystemTesting
    {
        
        [TestMethod]
        public void ProperAddDirectory()
        {
            FileSystem.FileSystem f = new FileSystem.FileSystem();
            f.AddDirectory("/foo/bar/diddles/");
            f.AddDirectory("/foo/bar/funtimes/");
            f.AddDirectory("/foo/bar/other/");
            f.AddDirectory("/foo/bar/funtimes/foo/1/2/3/4");

            bool exists = f.DirectoryExists("/foo/bar/funtimes/foo/1/2/3/4");
            Assert.AreEqual(true, exists);
        }

        [TestMethod]
        public void IgnoresAddDirectory()
        {
            FileSystem.FileSystem f = new FileSystem.FileSystem();
            f.AddDirectory("/foo/bar/diddles/");
            f.AddDirectory("/foo/bar/diddles/");

            Assert.AreEqual(true, f.DirectoryExists("/foo/bar/diddles/"));
        }

        [TestMethod]
        public void GoodAddFile() {
            FileSystem.FileSystem f = new FileSystem.FileSystem();
            bool added = f.AddFile("/foo/bar/diddles/file1.txt");
            Assert.AreEqual(true, added);
        }

        [TestMethod]
        public void BadAddFile()
        {
            FileSystem.FileSystem f = new FileSystem.FileSystem();
            bool added = f.AddFile("/foo/bar/diddles/file1.txt");
            added = f.AddFile("/foo/bar/diddles/file1.txt");
            Assert.AreEqual(false , added);
        }

        [TestMethod]
        public void GoodRemoveFile()
        {
            FileSystem.FileSystem f = new FileSystem.FileSystem();
            f.AddFile("/foo/bar/diddles/file1.txt");
            bool removed = f.RemoveFile("/foo/bar/diddles/file1.txt");

            FileSystem.Node node = f.AddDirectory("/foo/bar/diddles/");

            Assert.AreEqual(true, removed);
            Assert.AreEqual(0, node.Files().Count);
        }

        [TestMethod]
        public void BadRemoveFile()
        {
            FileSystem.FileSystem f = new FileSystem.FileSystem();
            f.AddFile("/foo/bar/diddles/file1.txt");
            bool removed = f.RemoveFile("/foo/bar/diddles/file2.txt");

            FileSystem.Node node = f.AddDirectory("/foo/bar/diddles/");

            Assert.AreEqual(false, removed);
            Assert.AreEqual(1, node.Files().Count);
        }

        [TestMethod]
        public void GoodRemoveDirectory()
        {
            FileSystem.FileSystem f = new FileSystem.FileSystem();
            f.AddDirectory("/foo/bar/diddles0");
            f.AddDirectory("/foo/bar/diddles1");
            f.AddDirectory("/foo/bar/diddles2");
            f.AddDirectory("/foo/bar/diddles3");

            bool removed = f.RemoveDirectory("/foo/bar/diddles0");

            Assert.AreEqual(true, removed);
            Assert.AreEqual(false, f.DirectoryExists("/foo/bar/diddles0"));
        }

        [TestMethod]
        public void BadRemoveDirectory()
        {
            FileSystem.FileSystem f = new FileSystem.FileSystem();
            f.AddDirectory("/foo/bar/diddles0");
            f.AddDirectory("/foo/bar/diddles1");
            f.AddDirectory("/foo/bar/diddles2");
            f.AddDirectory("/foo/bar/diddles3");

            bool removed = f.RemoveDirectory("/foo/bar/diddles7");

            Assert.AreEqual(false, removed);
            Assert.AreEqual(false, f.DirectoryExists("/foo/bar/diddles7"));

        }
		[TestMethod]
		public void NumberFiles()
		{
			FileSystem.FileSystem f = new FileSystem.FileSystem();
			f.AddDirectory("/foo/bar");
			f.AddFile("/foo/bar/file1.txt");
			f.AddFile("/foo/bar/file2.txt");
			f.AddFile("/foo/bar/file3.txt");

			Assert.AreEqual(3, f.Count);

			f.RemoveFile("/foo/bar/randomFile.txt");
			f.RemoveFile("/foo/bar/file3.txt");

			Assert.AreEqual(2, f.Count);
		}

    }
}
