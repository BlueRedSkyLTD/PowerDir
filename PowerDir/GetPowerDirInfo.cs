using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir
{
    /// <summary>
    /// 
    /// </summary>
    public class GetPowerDirInfo : IEquatable<GetPowerDirInfo>
    {
        // TODO: evaluate to further elabore on link attribute
        // ref: http://www.flexhex.com/docs/articles/hard-links.phtml#hardlinks
        // (eg hard links are not detected, because not supported by .NET, but where is the power of this tool then?)
        // ref: https://stackoverflow.com/questions/4193309/list-hard-links-of-a-file-in-c

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        public string Extension { get; }
        /// <summary>
        /// 
        /// </summary>
        public long size { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool Link { get; }
        /// <summary>
        /// 
        /// </summary>
        internal FileAttributes Attributes { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool Directory { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool Hidden { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool System { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool ReadOnly { get; }
        //public bool ReparsePoint { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool Archive { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool Compressed { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool Encrypted { get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreationTime { get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastAccessTime { get; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime LastWriteTime { get; }

        //public FileSystemSecurity SecurityInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (Name, Extension, Directory, Link, Attributes).GetHashCode();
        }
    }
}
