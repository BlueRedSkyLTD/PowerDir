using System.Management.Automation;
using System.Management.Automation.Host;
using System.Text;
using PowerDir.views;
using System.IO;


namespace PowerDir
{
    /// <summary>
    /// <para type="synopsis">Get-PowerDir an alternate Get-ChildItem.</para>
    /// <para type="description">Get-PowerDir is used to display files and directories</para>
    /// <para type="description">search for them in a user-friendly way, alsosupporting colors.</para>
    /// </summary>
    /// <para type="link" uri="(http://tempuri.org)">[My Web Page]</para>
    /// <para type="link">about_PowerDir</para>
    [Cmdlet(VerbsCommon.Get, "PowerDir")]
    [OutputType(typeof(GetPowerDirInfo))]
    [Alias("d")]
    public class GetPowerDir : PSCmdlet
    {
        const int MAX_NAME_LENGTH = 50;

        /// <summary>
        /// convert Hex color format to RGB
        /// </summary>
        /// <param name="hex"></param>
        /// <returns>(r,g,b)</returns>
        private (byte,byte,byte) hexToRgb(int hex)
        {
            return (
                (byte)((hex >> 16) & 0xFF),
                (byte)((hex >> 8) & 0xFF),
                (byte)((hex) & 0xFF)
            );
        }

        #region Parameters

        /// <summary>
        /// <para type="description">Globbing path search (default: *).</para>
        /// <para type="inputDescription">string supporting some wildcards (Globbing not yet implemented).</para>
        /// </summary>
        [Parameter(
            Position = 0,
            HelpMessage = "Path to search. Accepting wildcards (default: *)"
        )]
        [SupportsWildcards()]
        public string Path { get; set; } = "*";

        //private bool pagination;
        /// <summary>
        /// <para type="description"> blah blah blah</para>
        /// <para type="inputType">Input description for parameter.</para>
        /// </summary>
        //[Parameter]
        //public SwitchParameter Pagination { get { return pagination; } set { pagination = value; } }

        private bool _recursive;
        /// <summary>
        /// <para type="description">Search Recursively (default: No)</para>
        /// </summary>
        [Parameter(
            HelpMessage ="Search Recursively (default: No)",
            ParameterSetName = "Recursion"
            )]
        [Alias("r")]
        public SwitchParameter Recursive {
            get { return _recursive; }
            set { _recursive = value; }
        }

        /// <summary>
        /// <para type="description">Max Recursion Depth (default: int.MaxValue)</para>
        /// <para type="inputType">int</para>
        /// </summary>
        [Parameter(
            HelpMessage = "Max Recursion Depth (default: int.MaxValue)",
            ParameterSetName = "Recursion"
        )]
        [Alias("l")]
        public int Level { get; set; } = int.MaxValue;

        /// <summary>
        /// <para type="description"></para>
        /// </summary>
        public enum DisplayOptions
        {
            /// <summary>
            ///
            /// </summary>
            Object = 0,
            /// <summary>
            /// 
            /// </summary>
            List = 1,
            /// <summary>
            /// 
            /// </summary>
            ListDetails = 2,
            /// <summary>
            /// 
            /// </summary>
            Wide = 3,
            // aliases
            /// <summary>
            /// 
            /// </summary>
            o = 0,
            /// <summary>
            /// 
            /// </summary>
            l = 1,
            /// <summary>
            /// 
            /// </summary>
            ld = 2,
            /// <summary>
            /// 
            /// </summary>
            w = 3
        }

        /// <summary>
        /// <para type="description">Display type (default: Object)</para>
        /// <para type="inputType">DisplayOptions</para>
        /// </summary>
        [Parameter(
            HelpMessage = "Display type (default: Object)"
        )]
        [Alias("d")]
        public DisplayOptions Display { get; set; } = DisplayOptions.Object;
        #endregion Parameters

        private ConsoleColor fg;
        private ConsoleColor bg;

        private List<string> dirs = new List<string>();
        private List<string> files = new List<string>();

        // TODO: review this HashSet, a concurrent bag maybe btter
        private HashSet<GetPowerDirInfo> results = new HashSet<GetPowerDirInfo>();

        private bool _supportColor = true;
        int _width = 120;
        // TODO: consider to use just writeObject generating a string instead as it can support color with ESC[ sequence
        bool _useUIWrite = true;
        StringBuilder _sb = new StringBuilder();

        private string basePath = "./";
        private EnumerationOptions enumerationOptions = new EnumerationOptions();

        // TODO: pagination

        // TODO: get-power-dir | format-wide

        // TODO: get-power-dir attributes, datetime, size, etc..

        // TODO to be upgraded to 24 bits
        private PowerDirTheme theme = new PowerDirTheme(ConsoleColor.Gray, ConsoleColor.Black);

        #region WriteOps
        private void write(string msg)
        {
            // TODO consider to use just writeObject generating a string instead.
            if (_useUIWrite)
                Host.UI.Write(msg);
            else
                _sb.Append(msg);
        }

        private void writeLine(string msg = "")
        {
            if (_useUIWrite)
            {
                Host.UI.Write(msg);
                Host.UI.WriteLine();
            }
            else
            {
                _sb.Append(msg);
                WriteObject(_sb.ToString());
                _sb.Clear();
            }
        }
        #endregion

        #region Colors
        private void resetColor()
        {
            if (!_supportColor) return;
            Host.UI.RawUI.ForegroundColor = fg;
            Host.UI.RawUI.ForegroundColor = bg;
        }

        //private void resetColor24Bits()
        //{
        //    write("\x1B[0m");
        //}

        private void setColor(PowerDirTheme.ColorThemeItem color)
        {
            if (!_supportColor) return;
            Host.UI.RawUI.ForegroundColor = color.Fg;
            Host.UI.RawUI.BackgroundColor = color.Bg;
        }

        //private void setColor(int fg_col, int bg_col)
        //{
        //    if (!_supportColor) return;
        //    var (fr, fg, fb) = hexToRgb(fg_col);
        //    var (br, bg, bb) = hexToRgb(bg_col);
        //    write($"\x1B[38;2;{fr};{fg};{fb}m\x1B[48;2;{br};{bg};{bb}m");
        //}

        #endregion


        #region Colored WriteOps
        //private void write(string msg, int fg, int bg)
        //{
        //    setColor(fg, bg);
        //    write(msg);
        //    resetColor24Bits();
        //}

        //private void writeLine(string msg, int fg, int bg)
        //{
        //    write(msg, fg, bg);
        //    writeLine();
        //}
        /// <summary>
        /// write a message using the given color.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="col"></param>
        private void write(string msg, PowerDirTheme.ColorThemeItem col)
        {
            setColor(col);
            write(msg);
            setColor(theme.GetOriginalColor());
        }
        
        #endregion

        private void checkColorSupport()
        {
            try
            {
                fg = Host.UI.RawUI.ForegroundColor;
                bg = Host.UI.RawUI.BackgroundColor;
                // Loading Color Theme (only default one at the moment)
                // TODO: load color theme from env variable or setting file
                theme = new PowerDirTheme(fg, bg);
            }
            catch (HostException ex)
            {
                _supportColor = false;
                WriteError(ex.ErrorRecord);
            }
        }

        // TODO: this can be merged in checkColorSupport method
        //       as if there is no color there won't be no with neither i guess... 
        private void checkWidthSupport()
        {
            try
            {
                _width = Host.UI.RawUI.WindowSize.Width;
            }
            catch (HostException e)
            {
                _useUIWrite = false;
                WriteError(e.ErrorRecord);
            }
        }

        private void processPath()
        {
            WriteDebug($"[START] Path = {Path} --- basePath = {basePath}");

            Path = Path.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            // TODO this not work due to drive letters that needs absolute path
            if (Path.StartsWith("$HOME"))
                Path = Path.Replace("$HOME", "~");
            if (Path.StartsWith("~"))
            {
                Path = SessionState.Path.NormalizeRelativePath(Path, basePath);
                WriteDebug($"Normalized Relative Path = {Path}");
            }
            Path = System.IO.Path.GetFullPath(Path);
            WriteDebug($"Normalized Absolute Path = {Path}");
            basePath = System.IO.Path.GetFullPath(Path);
            var p = System.IO.Path.GetDirectoryName(Path);
            // if p == null is root dir
            if (p!=null && Path != p)
            {
                basePath = System.IO.Path.Combine(basePath, p);
                var split = Path.Split(p);
                WriteDebug($"Path split = [{String.Join(',', split)}]");
                foreach (var s in split)
                {
                    if (s.Length == 0) continue;
                    if (Path.Contains(s))
                    {
                        int index = s.StartsWith(System.IO.Path.DirectorySeparatorChar) ? 1 : 0;
                        Path = s.Substring(index);
                        break;
                    }
                }
            }

            if(Directory.Exists(Path) || Directory.Exists(System.IO.Path.Combine(basePath, Path)))
            {
                basePath = System.IO.Path.Combine(basePath, Path);
                Path = "*";
            }

            if (string.IsNullOrEmpty(Path)) Path = "*";

            WriteDebug($"[END] Path = {Path} --- basePath = {basePath}");

        }
        private void collectResults()
        {
            enumerationOptions.RecurseSubdirectories = _recursive;
            enumerationOptions.MaxRecursionDepth = Level;
            //enumerationOptions.ReturnSpecialDirectories = true;
            enumerationOptions.IgnoreInaccessible = false;
            enumerationOptions.AttributesToSkip = 0;

            dirs = Directory.EnumerateDirectories(basePath, Path, enumerationOptions).ToList();
            files = Directory.EnumerateFiles(basePath, Path, enumerationOptions).ToList();

            // TODO: consider to process this while displaying instead.
            foreach (string dir in dirs)
            {
                var dirInfo = new DirectoryInfo(dir);
                results.Add(new GetPowerDirInfo(dirInfo));
            }

            foreach (string file in files)
            {
                var fileInfo = new FileInfo(file);
                results.Add(new GetPowerDirInfo(fileInfo));
            }

            dirs.Clear();
            files.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void BeginProcessing()
        {
            //write("Power", 0xFF0000, 0x00FFFF);
            //write("Dir", 0x00FF00, 0xFF00FF);
            //write("terminal color test", 0x0000FF, 0xFFFF00);
            //writeLine();

            WriteDebug($"Host Name = {Host.Name}");
            basePath = SessionState.Path.CurrentFileSystemLocation.Path;
            WriteDebug($"basePath = {basePath} --- Path = {Path}");

            checkColorSupport();
            checkWidthSupport();

            WriteDebug($"Color = {_supportColor}");
            WriteDebug($"Width = {_width} --- useUIWrite={_useUIWrite}");
            WriteDebug($"Recursive = {_recursive}");

            processPath();
            collectResults();
            
            // TODO
            //if (pagination)
            //{
            //    WriteDebug("paginated results");
            //    WriteDebug(PagingParameters.ToString());
            //}

            base.BeginProcessing();
        }

        private void displayObject()
        {
            WriteObject(results, true);
        }

        private void displayList()
        {
            var l = new ListView(write, write, writeLine, theme);
            l.displayResults(results);
        }
        private void displayListDetails()
        {
            // TODO
            //      permissions?
            //      etc
            
            // TODO switch parameter for dateTime type
            ListDetailsView ldv = new ListDetailsView(_width, MAX_NAME_LENGTH, write, write, writeLine, theme, ListDetailsView.EDateTimes.CREATION);
            ldv.displayResults(results);
        }

        private void displayWide()
        {
            // TODO as a dynamic parameter when using displayWide
            int num_columns = 4;
            
            int col_size = _width / num_columns;
            // col_size = 40;
            // num_columns = width / col_size;

            WriteDebug($"width = {_width} --- col_size = {col_size} --- num_columns = {num_columns}");

            WideView view = new WideView(_width, num_columns, write, write, writeLine, theme);
            view.displayResults(results);
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void EndProcessing()
        {
            switch (Display)
            {
                case DisplayOptions.Object:
                    displayObject();
                    break;
                case DisplayOptions.List:
                    displayList();
                    break;
                case DisplayOptions.ListDetails:
                    displayListDetails();
                    break;
                case DisplayOptions.Wide:
                    displayWide();
                    break;
            }

            base.EndProcessing();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void StopProcessing()
        {
            base.StopProcessing();
        }
    }
}
