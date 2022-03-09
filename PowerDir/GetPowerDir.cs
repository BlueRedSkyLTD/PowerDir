using System.Management.Automation;
using System.Management.Automation.Host;


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

        private ConsoleColor fg;
        private ConsoleColor bg;

        private List<string> dirs = new List<string>();
        private List<string> files = new List<string>();

        private List<GetPowerDirInfo> results = new List<GetPowerDirInfo>();

        private bool _supportColor = true;

        private const string basePath = "./";
        private EnumerationOptions enumerationOptions = new EnumerationOptions();

        // TODO: Mapping colors
        // directory blue
        // file gray
        // link cyan
        // exe gree
        // system dark gray
        enum ColorTheme
        {
            DIRECTORY = ConsoleColor.Blue,
            FILE = ConsoleColor.Gray,
            EXE = ConsoleColor.Green,
            LINK = ConsoleColor.Cyan,
            HIDDEN_DIR = ConsoleColor.DarkBlue,
            HIDDEN_FILE = ConsoleColor.DarkGray,
            SYSTEM_DIR = ConsoleColor.DarkRed,
            SYSTEM_FILE = ConsoleColor.Red
        }

        private void setFgColor(GetPowerDirInfo info)
        {
            if(info.Link)
            {
                setFgColor(ColorTheme.LINK);
            }
            else if(info.Hidden)
            {
                setFgColor(info.Directory ? ColorTheme.HIDDEN_DIR : ColorTheme.HIDDEN_FILE);
            }
            else if (info.System)
            {
                setFgColor(info.Directory ? ColorTheme.SYSTEM_DIR : ColorTheme.SYSTEM_FILE);
            }
            else if(info.Directory)
            {
                setFgColor(ColorTheme.DIRECTORY);
            }
            else if(info.Extension.ToUpper().EndsWith(".EXE"))
            {
                // EXE
                setFgColor(ColorTheme.EXE);
            }
            else
            {
                // FILE
                setFgColor(ColorTheme.FILE);
            }
        }
        private void setFgColor(DirectoryInfo dirInfo)
        {
            // LINK
            if (dirInfo.LinkTarget != null)
            {
                setFgColor(ColorTheme.LINK);
            }
            else if (dirInfo.Attributes.HasFlag(FileAttributes.Hidden))
            {
                // HIDDEN
                setFgColor(ColorTheme.HIDDEN_DIR);
            }
            // DIRECTORY
            else
            {
                setFgColor(ColorTheme.DIRECTORY);
            }
        }

        // TODO: display attributes

        // TODO: pagination

        // TODO: get-power-dir | format-wide

        // TODO: get-power-dir attributes, datetime, size, etc..

        private void setFgColor(ColorTheme ct)
        {
            setFgColor((ConsoleColor)ct);
        }
        private void setFgColor(ConsoleColor fg)
        {
            if (!_supportColor) return;
            Host.UI.RawUI.ForegroundColor = fg;
        }

        protected override void BeginProcessing()
        {
            WriteDebug($"Host.Name = {Host.Name}");
            try
            {
                fg = Host.UI.RawUI.ForegroundColor;
                bg = Host.UI.RawUI.BackgroundColor;
            }
            catch (HostException ex)
            {
                _supportColor = false;
                WriteWarning(ex.Message);
            }

            WriteDebug($"Color = {_supportColor}");
            WriteDebug($"Recursive = {_recursive}");

            enumerationOptions.RecurseSubdirectories = _recursive;
            enumerationOptions.MaxRecursionDepth = Level;
            enumerationOptions.ReturnSpecialDirectories = true;
            enumerationOptions.IgnoreInaccessible = false;
            enumerationOptions.AttributesToSkip = 0;
            
            dirs = Directory.EnumerateDirectories(basePath, Path, enumerationOptions).ToList();
            files = Directory.EnumerateFiles(basePath, Path, enumerationOptions).ToList();
            if (pagination)
            {
                WriteDebug("paginated results");
                WriteDebug(PagingParameters.ToString());
            }

            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            setFgColor(ConsoleColor.Blue);
            WriteObject(dirs, true);

            setFgColor(ConsoleColor.Gray);
            WriteObject(files, true);

            WriteObject(" ------- ");
            setFgColor(ConsoleColor.White);
            foreach(string dir in dirs)
            {
                var dirInfo = new DirectoryInfo(dir);

                //dirInfo.Attributes.
                //dirInfo.CreationTime
                //dirInfo.GetAccessControl
                //dirInfo.LastAccessTime
                //dirInfo.LastWriteTime

                //setFgColor(dirInfo);
                //WriteObject(dirInfo.Name);

                results.Add(new GetPowerDirInfo(dirInfo));
            }

            foreach(string file in files)
            {
                var fileInfo = new FileInfo(file);
                results.Add(new GetPowerDirInfo(fileInfo));
            }

            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            //WriteObject(results, true);

            foreach(var r in results)
            {
                setFgColor(r);
                WriteObject(r.Name);
            }

            base.EndProcessing();
        }
    }
}
