using System.Management.Automation;
using System.Management.Automation.Host;
using System.Text;
using PowerDir.views;


namespace PowerDir
{
    /// <summary>
    /// <para type="synopsis">This is the cmdlet synopsis.</para>
    /// <para type="description">This is part of the longer cmdlet description.</para>
    /// <para type="description">Also part of the longer cmdlet description.</para>
    /// </summary>
    /// <para type="link" uri="(http://tempuri.org)">[My Web Page]</para>
    /// <para type="link">about_PowerDir</para>
    /// 
    /// <example>
    ///   <para>This is part of the first example's introduction.</para>
    ///   <para>This is also part of the first example's introduction.</para>
    ///   <code>New-Thingy | Write-Host</code>
    ///   <para>This is part of the first example's remarks.</para>
    ///   <para>This is also part of the first example's remarks.</para>
    /// </example>
    [Cmdlet(VerbsCommon.Get, "PowerDir")]
    [OutputType(typeof(GetPowerDirInfo))]
    public class GetPowerDir : PSCmdlet
    {
        const int MAX_NAME_LENGTH = 50;

        #region Parameters

        /// <summary>
        /// <para type="description">Globbing path search (default: *).</para>
        /// </summary>
        [Parameter(
            Position = 0,
            HelpMessage = "Path to search. Accepting wildcards (default: *)"
        )]
        [SupportsWildcards()]
        public string Path { get; set; } = "*";

        private bool pagination;
        /// <summary>
        /// <para type="description"> blah blah blah</para>
        /// <para type="inputType">Input description for parameter.</para>
        /// </summary>
        [Parameter]
        public SwitchParameter Pagination { get { return pagination; } set { pagination = value; } }

        private bool _recursive;
        [Parameter(
            HelpMessage ="Search Recursively (default: No)",
            ParameterSetName = "Recursion"
            )]
        [Alias("r")]
        public SwitchParameter Recursive {
            get { return _recursive; }
            set { _recursive = value; }
        }

        [Parameter(
            HelpMessage = "Max Recursion Depth (default: int.MaxValue)",
            ParameterSetName = "Recursion"
        )]
        [Alias("l")]
        public int Level { get; set; } = int.MaxValue;

        public enum DisplayOptions
        {
            Object = 0,
            List = 1,
            ListDetails = 2,
            Wide = 3,
            // aliases
            o = 0,
            l = 1,
            ld = 2,
            w = 3
        }

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

        private HashSet<GetPowerDirInfo> results = new HashSet<GetPowerDirInfo>();

        private bool _supportColor = true;
        int _width = 120;
        bool _useUIWrite = true;
        StringBuilder _sb = new StringBuilder();

        private string basePath = "./";
        private EnumerationOptions enumerationOptions = new EnumerationOptions();

        // TODO: pagination

        // TODO: get-power-dir | format-wide

        // TODO: get-power-dir attributes, datetime, size, etc..

        private PowerDirTheme theme = new PowerDirTheme(ConsoleColor.Gray, ConsoleColor.Black);

        private void setColor(PowerDirTheme.ColorThemeItem color)
        {
            if (!_supportColor) return;
            Host.UI.RawUI.ForegroundColor = color.Fg;
            Host.UI.RawUI.BackgroundColor = color.Bg;
        }

        private void write(string msg)
        {
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

        protected override void BeginProcessing()
        {
            basePath = this.SessionState.Path.CurrentFileSystemLocation.Path;
            WriteDebug($"basePath = {basePath}");
            WriteDebug($"Host.Name = {Host.Name}");

            checkColorSupport();
            checkWidthSupport();

            WriteDebug($"Color = {_supportColor}");
            WriteDebug($"Width = {_width} --- useUIWrite(default)={_useUIWrite}");
            WriteDebug($"Recursive = {_recursive}");

            collectResults();
            
            // TODO
            if (pagination)
            {
                WriteDebug("paginated results");
                WriteDebug(PagingParameters.ToString());
            }

            base.BeginProcessing();
        }

        private void displayObject()
        {
            WriteObject(results, true);
        }

        private void displayList()
        {
            var l = new ListView(write, writeLine, setColor, theme);
            l.displayResults(results);
        }
        private void displayListDetails()
        {
            // TODO
            //      permissions?
            //      etc
            
            // TODO switch parameter for dateTime type
            ListDetailsView ldv = new ListDetailsView(MAX_NAME_LENGTH, write, writeLine, setColor, theme, ListDetailsView.EDateTimes.CREATION);
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

            WideView view = new WideView(_width, num_columns, write, writeLine, setColor, theme);
            view.displayResults(results);
        }
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
    }
}
