using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir
{
    internal class GetPowerDirInfo : IEquatable<GetPowerDirInfo>
    {
        // TODO: evaluate to further elabore on link attribute
        // ref: http://www.flexhex.com/docs/articles/hard-links.phtml#hardlinks
        // (eg hard links are not detected, because not supported by .NET, but where is the power of this tool then?)
        // ref: https://stackoverflow.com/questions/4193309/list-hard-links-of-a-file-in-c

        public string Name { get; }
        public string Extension { get; }
        public long size { get; }
        public bool Link { get; }
        internal FileAttributes Attributes { get; }
        public bool Directory { get; }
        public bool Hidden { get; }
        public bool System { get; }
        public bool ReadOnly { get; }
        //public bool ReparsePoint { get; }
        public bool Archive { get; }
        public bool Compressed { get; }
        public bool Encrypted { get; }
        public DateTime CreationTime { get; }
        public DateTime LastAccessTime { get; }
        public DateTime LastWriteTime { get; }

        //public FileSystemSecurity SecurityInfo { get; }

        public GetPowerDirInfo(FileSystemInfo info)
        {
            Name = info.Name;
            Extension = info.Extension;
            Link = info.LinkTarget != null;
            Attributes = info.Attributes;
            Directory = info.Attributes.HasFlag(FileAttributes.Directory);
            Hidden = info.Attributes.HasFlag(FileAttributes.Hidden);
            System = info.Attributes.HasFlag(FileAttributes.System);
            ReadOnly = info.Attributes.HasFlag(FileAttributes.ReadOnly);
            //ReparsePoint = info.Attributes.HasFlag(FileAttributes.ReparsePoint);
            Archive = info.Attributes.HasFlag(FileAttributes.Archive);
            Compressed = info.Attributes.HasFlag(FileAttributes.Compressed);
            Encrypted = info.Attributes.HasFlag(FileAttributes.Encrypted);

            size = Directory? 0 : ((FileInfo) info).Length; // not available for directory

            CreationTime = info.CreationTime;
            LastAccessTime = info.LastAccessTime;
            LastWriteTime = info.LastWriteTime;
            
            // This throws
            //SecurityInfo = Directory ?
            //    ((DirectoryInfo)info).GetAccessControl() :
            //    ((FileInfo)info).GetAccessControl();
        }

        public bool Equals(GetPowerDirInfo? other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return Name == other.Name 
                && Extension == other.Extension
                && Directory == other.Directory
                && Link == other.Link
                && Attributes == other.Attributes;
        }

        public override int GetHashCode()
        {
            return (Name, Extension, Directory, Link, Attributes).GetHashCode();
        }
    }
}
