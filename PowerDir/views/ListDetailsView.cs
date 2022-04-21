using System.Text;

namespace PowerDir.views
{
    internal class ListDetailsView : AbstractView
    {
        private const string _fmt_date = "s"; //"yy/MM/dd HH:mm:ss";
        private readonly StringBuilder _sb;

        // TODO: as flags instead, so not mutual exclusive but 
        // adding them as extra columns?
        public enum EDateTimes
        {
            CREATION = 0,
            LAST_ACCESS,
            LAST_WRITE
        }
        private readonly EDateTimes eDateTimes;

        /// <summary>
        ///  TODO: remove the constructor
        /// </summary>
        /// <param name="width"></param>
        /// <param name="name_max_length"></param>
        /// <param name="writeFunc"></param>
        /// <param name="writeColorFunc"></param>
        /// <param name="writeLineFunc"></param>
        /// <param name="theme"></param>
        /// <param name="eDateTimes"></param>
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

#pragma warning disable S1172 // Unused method parameters should be removed
        private string dateTimes(GetPowerDirInfo info)
#pragma warning restore S1172 // Unused method parameters should be removed
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

        public override void displayResult(GetPowerDirInfo result)
        {
            _writeColor(result.Attr, _theme.GetOriginalColor());
            _write(" ");

            _writeColor(names(result), _theme.GetColor(result));

            _write(" ");
            _write(result.NormalizedSize);
            _write(" ");
            _writeLine(dateTimes(result));
        }
    }
}
