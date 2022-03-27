using System.Text;

namespace PowerDir
{
    // TODO: using this class colors are lost unless use it to write too.
    internal class ListDetailsView
    {
        private readonly string _fmt_name;
        private const string _fmt_size = "{0,6}{1,1}";
        private const string _fmt_date = "s";//"yy/MM/dd HH:mm:ss";
        public int NameMaxLength { get; }

        private readonly string[] _suffixes = { "", "K", "M", "G", "T", "P" };

        public ListDetailsView(int name_max_length)
        {
            NameMaxLength = name_max_length;
            _fmt_name = "{0," + -NameMaxLength + "}";
        }

        
        private void attributes(GetPowerDirInfo info, StringBuilder sb)
        {
            sb
                .Append(info.Directory ? 'd' : info.Archive ? 'a' : '-')
                .Append(info.Link ? 'l' : '-')
                //.Append(info.Archive ? 'a' : '-')
                .Append(info.Compressed ? 'c' : '-')
                .Append(info.ReadOnly ? 'r' : '-')
                .Append(info.Hidden ? 'h' : '-')
                .Append(info.System ? 's' : '-')
                .Append(info.Encrypted ? 'e' : '-')
                .Append('-');
        }

        private void names(GetPowerDirInfo info, StringBuilder sb)
        {
            if (info.Name.Length > NameMaxLength)
            {
                sb.Append(info.Name.Substring(0, NameMaxLength - 3));
                sb.Append("...");
            }
            else
                sb.Append(String.Format(_fmt_name, info.Name));
        }

        private void dateTimes(GetPowerDirInfo info, StringBuilder sb)
        {
            //sb.Append(info.CreationTime.ToLocalTime().ToString(_fmt_date));
            //sb.Append(' ');
            sb.Append(info.LastAccessTime.ToLocalTime().ToString(_fmt_date));
            //sb.Append(' ');
            //sb.Append(info.LastWriteTime.ToLocalTime().ToString(_fmt_date));
        }

        private string normalizeSize(GetPowerDirInfo info)
        {
            if (info.Directory)
                return String.Format(_fmt_size,"-","");

            int exp = 0;
            decimal _size = (decimal)info.size; // double could not contain a long (int64)
            while (_size >= 1024)
            {
                _size /= 1024;
                exp++;
            }
            exp %= _suffixes.Length; // just to avoid errors
            string fmt = exp == 0 ? "0" : "0.00";

            return String.Format(_fmt_size, _size.ToString(fmt), _suffixes[exp]);
        }
        public string getRow(GetPowerDirInfo info)
        {
            // TODO 120 to be replaced with UI.Width and use it as a fallback
            StringBuilder sb = new StringBuilder(" -", 120);
            attributes(info, sb);
            //File Name
            sb.Append(' ');
            names(info, sb);
            // File Size
            sb.Append(' ');
            sb.Append(normalizeSize(info));

            // Date Times
            sb.Append(' ');
            dateTimes(info, sb);

            return sb.ToString();
        }
    }
}
