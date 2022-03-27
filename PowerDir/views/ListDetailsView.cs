using System.Text;

namespace PowerDir.views
{
    // TODO: using this class colors are lost unless use it to write too.
    internal class ListDetailsView : AbstractView, IView
    {
        private const string _fmt_size = "{0,6}{1,1}";
        private const string _fmt_date = "s"; //"yy/MM/dd HH:mm:ss";

        private readonly string[] _suffixes = { "", "K", "M", "G", "T", "P" };

        // TODO: as flags instead, so not mutual exclusive but 
        // adding them as extra columns?
        public enum EDateTimes
        {
            CREATION = 0,
            LAST_ACCESS,
            LAST_WRITE
        }
        private EDateTimes eDateTimes = EDateTimes.CREATION;

        internal ListDetailsView(
            in int name_max_length,
            in Action<string> writeFunc,
            in Action<string> writeLineFunc,
            in Action<PowerDirTheme.ColorThemeItem> setColorFunc,
            in PowerDirTheme theme,
            in EDateTimes eDateTimes
        ) : base(name_max_length, writeFunc, writeLineFunc, setColorFunc, theme)
        {
            this.eDateTimes = eDateTimes;
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

        private void dateTimes(GetPowerDirInfo info, StringBuilder sb)
        {
            switch (eDateTimes)
            {
                case EDateTimes.CREATION:
                    sb.Append(info.CreationTime.ToLocalTime().ToString(_fmt_date));
                    break;
                case EDateTimes.LAST_ACCESS:
                    sb.Append(info.LastAccessTime.ToLocalTime().ToString(_fmt_date));
                    break;
                case EDateTimes.LAST_WRITE:
                    sb.Append(info.LastWriteTime.ToLocalTime().ToString(_fmt_date));
                    break;
            }
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

            return string.Format(_fmt_size, _size.ToString(fmt), _suffixes[exp]);
        }
        private string getRow(GetPowerDirInfo info)
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

        public void displayResults(IReadOnlyCollection<GetPowerDirInfo> results)
        {
            foreach (var r in results)
            {
                // TODO: just highlight the name, instead of the whole row
                _setColor(_theme.getColor(r));
                _writeLine(getRow(r));
            }
        }

    }
}
