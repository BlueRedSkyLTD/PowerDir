using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDir
{
    internal class GetPowerDirInfo
    {
        public string Name { get; }
        public string Extension { get; }
        public bool Directory { get; }
        public bool Link { get; }
        public bool Hidden { get; }
        public bool System { get; }

        public GetPowerDirInfo(FileSystemInfo info)
        {
            Name = info.Name;
            Extension = info.Extension;
            Directory = info.Attributes.HasFlag(FileAttributes.Directory);
            Link = info.LinkTarget != null;
            Hidden = info.Attributes.HasFlag(FileAttributes.Hidden);
            System = info.Attributes.HasFlag(FileAttributes.System);
        }
    }
}
