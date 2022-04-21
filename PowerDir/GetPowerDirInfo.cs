﻿using System;
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
#pragma warning disable S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
    sealed public class GetPowerDirInfo : IEquatable<GetPowerDirInfo>
#pragma warning restore S1206 // "Equals(Object)" and "GetHashCode()" should be overridden in pairs
    {
        private const string _fmt_size = "{0,6}{1,1}";
        private readonly string[] _suffixes = { "", "K", "M", "G", "T", "P" };


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
        public string FullName { get; }
        /// <summary>
        /// 
        /// </summary>
        public string RelativeName { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Attr { get; }
        /// <summary>
        /// 
        /// </summary>
        public string NormalizedSize { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="basePath"></param>
        public GetPowerDirInfo(FileSystemInfo info, in string basePath)
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

            FullName = info.FullName;

            // TODO consider to compute this value when display the names instead.
            //      it won't be available in the PSObject
            RelativeName = Path.GetRelativePath(basePath, FullName);
            Attr = attributes();
            NormalizedSize = normalizeSize();
        }

        private string attributes()
        {
            return new StringBuilder(10)
                .Append(Directory ? 'd' : Archive ? 'a' : '-')
                .Append(Link ? 'l' : '-')
                //.Append(info.Archive ? 'a' : '-')
                .Append(Compressed ? 'c' : '-')
                .Append(ReadOnly ? 'r' : '-')
                .Append(Hidden ? 'h' : '-')
                .Append(System ? 's' : '-')
                .Append(Encrypted ? 'e' : '-')
                .Append('-')
                .ToString();
        }

        private string normalizeSize()
        {
            if (Directory)
                return String.Format(_fmt_size, "-", "");

            int exp = 0;
            decimal _size = (decimal)size; // double could not contain a long (int64)
            while (_size >= 1024)
            {
                _size /= 1024;
                exp++;
            }
            exp %= _suffixes.Length; // just to avoid errors
            string fmt = exp == 0 ? "0" : "0.00";

            return string.Format(_fmt_size, _size.ToString(fmt), _suffixes[exp]);
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

            return FullName == other.FullName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (FullName).GetHashCode();
        }

        
    }
}
