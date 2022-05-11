using System.Text;
using PowerDir.themes;

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

        internal ListDetailsView(
            in int width,
            in int name_max_length,
            in Action<object> writeObject,
            in EDateTimes eDateTimes
        ) : base(name_max_length, writeObject)
        {
            this.eDateTimes = eDateTimes;
            _sb = new StringBuilder(width);
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

        public override void displayResult(GetPowerDirInfo result, IPowerDirTheme theme)
        {
            _sb.Clear();
            _sb.Append(result.Attr)
                .Append(" ")
                .Append(theme.colorizeProperty(result, names(result.RelativeName)))
                .Append(" ")
                .Append(result.NormalizedSize)
                .Append(" ")
                .Append(dateTimes(result))
            ;

            _writeObject(_sb.ToString());
        }
    }
}
