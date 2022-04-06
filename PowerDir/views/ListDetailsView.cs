using System.Text;

namespace PowerDir.views
{
    internal class ListDetailsView : AbstractView, IView
    {
        private const string _fmt_size = "{0,6}{1,1}";
        private const string _fmt_date = "s"; //"yy/MM/dd HH:mm:ss";

        private readonly string[] _suffixes = { "", "K", "M", "G", "T", "P" };
        private StringBuilder _sb;

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
            in int width,
            in int name_max_length,
            in Action<string> writeFunc,
            in Action<string, PowerDirTheme.ColorThemeItem> writeColorFunc,
            in Action<string> writeLineFunc,
            in PowerDirTheme theme,
            in EDateTimes eDateTimes
        ) : base(name_max_length, writeFunc, writeColorFunc, writeLineFunc, theme)
        {
            this.eDateTimes = eDateTimes;
            _sb = new StringBuilder(" -", width);
        }

        private string attributes(GetPowerDirInfo info)
        {
            return new StringBuilder(10)
                .Append(info.Directory ? 'd' : info.Archive ? 'a' : '-')
                .Append(info.Link ? 'l' : '-')
                //.Append(info.Archive ? 'a' : '-')
                .Append(info.Compressed ? 'c' : '-')
                .Append(info.ReadOnly ? 'r' : '-')
                .Append(info.Hidden ? 'h' : '-')
                .Append(info.System ? 's' : '-')
                .Append(info.Encrypted ? 'e' : '-')
                .Append('-')
                .ToString();
        }

        private string dateTimes(GetPowerDirInfo info)
        {
            switch (eDateTimes)
            {
                case EDateTimes.CREATION:
                    return info.CreationTime.ToLocalTime().ToString(_fmt_date);
                case EDateTimes.LAST_ACCESS:
                    return info.LastAccessTime.ToLocalTime().ToString(_fmt_date);
                case EDateTimes.LAST_WRITE:
                    return info.LastWriteTime.ToLocalTime().ToString(_fmt_date);
            }

            throw new NotImplementedException();
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

        public void displayResults(IReadOnlyCollection<GetPowerDirInfo> results)
        {
            foreach (var r in results)
            {
                _writeColor(attributes(r), _theme.GetOriginalColor());
                _write(" ");

                _writeColor(names(r), _theme.GetColor(r));
                
                _write(" ");
                _write(normalizeSize(r));
                _write(" ");
                _writeLine(dateTimes(r));
            }
        }

    }
}
