using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir
{
    internal class GetPowerDirInfo : IEquatable<GetPowerDirInfo>
    {
        public string Name { get; }
        public string Extension { get; }
        public long size { get; }
        public bool Link { get; }
        internal FileAttributes Attributes { get; }
        public bool Directory { get; }
        public bool Hidden { get; }
        public bool System { get; }
        public bool ReadOnly { get; }
        public bool ReparsePoint { get; }
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
            ReadOnly = info.Attributes.HasFlag(FileAttributes.ReparsePoint);

            size = Directory? 0 : ((FileInfo) info).Length; // not available for directory

            //dirInfo.CreationTime
            //dirInfo.GetAccessControl
            //dirInfo.LastAccessTime
            //dirInfo.LastWriteTime
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

        public string normalizeSize()
        {
            if (Directory)
                return "-";

            string[] suffixes = { "", "K", "M", "G", "T", "P" };
            int exp = 0;
            decimal _size = (decimal) size; // double could not contain a long (int64)
            while (_size >= 1024)
            {
                _size /= 1024;
                exp++;
            }
            exp %= suffixes.Length; // just to avoid errors
            string fmt = exp == 0 ? "0" : "0.00";
            return String.Format("{0}{1}", _size.ToString(fmt), suffixes[exp]);
        }
    }
}
