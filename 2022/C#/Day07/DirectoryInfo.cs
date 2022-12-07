using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day07
{
    internal class DirectoryInfo
    {
        public DirectoryInfo(string name, DirectoryInfo? parent)
        {
            Name = name;
            Parent = parent;
            if (Parent != null)
            {
                Parent.Directories.Add(this);
            }
        }

        public bool Contains(string name, bool isDirectory)
        {
            if (isDirectory)
            {
                var found = Directories.Find(dir => dir.Name.Equals(name));
                return found != null;
            }
            else
            {
                var found = Files.Find(file => file.Name.Equals(name));
                return found != null;
            }
            return false;
        }

        public int ComputeSize()
        {
            int count = 0;
            foreach(var file in Files)
            {
                count += file.Size;
            }
            foreach(var dir in Directories)
            {
                count += dir.ComputeSize();
            }
            return count;
        }

        public List<DirectoryInfo> Directories = new List<DirectoryInfo>();
        public List<File> Files = new List<File>();
        public DirectoryInfo? Parent = null;
        public string Name;
    }

    internal class File
    {
        public string Name;
        public int Size;
    }
}
