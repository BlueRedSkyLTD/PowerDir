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

        public enum DisplayOptions
        {
            Object = 0,
            List = 1,
            Wide = 2,
            // aliases
            o = 0,
            l = 1,
            w = 2
        }

        [Parameter(
            HelpMessage = "Display type (default: Object)"
        )]
        [Alias("d")]
        public DisplayOptions Display { get; set; } = DisplayOptions.Object;

        private ConsoleColor fg;
        private ConsoleColor bg;

        private List<string> dirs = new List<string>();
        private List<string> files = new List<string>();

        private HashSet<GetPowerDirInfo> results = new HashSet<GetPowerDirInfo>();

        private bool _supportColor = true;

        private string basePath = "./";
        private EnumerationOptions enumerationOptions = new EnumerationOptions();

        // TODO: display attributes

        // TODO: pagination

        // TODO: get-power-dir | format-wide

        // TODO: get-power-dir attributes, datetime, size, etc..

        private void setFgColor(ConsoleColor fg)
        {
            if (!_supportColor) return;
            Host.UI.RawUI.ForegroundColor = fg;
        }

        protected override void BeginProcessing()
        {
            basePath = this.SessionState.Path.CurrentFileSystemLocation.Path;
            WriteDebug($"basePath = {basePath}");
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
            //enumerationOptions.ReturnSpecialDirectories = true;
            enumerationOptions.IgnoreInaccessible = false;
            enumerationOptions.AttributesToSkip = 0;
            
            dirs = Directory.EnumerateDirectories(basePath, Path, enumerationOptions).ToList();
            files = Directory.EnumerateFiles(basePath, Path, enumerationOptions).ToList();
            if (pagination)
            {
                WriteDebug("paginated results");
                WriteDebug(PagingParameters.ToString());
            }

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

            base.BeginProcessing();
        }

        private void displayObject()
        {
            WriteObject(results, true);
        }

        private void displayList()
        {
            foreach (var r in results)
            {
                setFgColor(PowerDirTheme.getFgColor(r));
                WriteObject(r.Name);
            }
        }

        private void displayWide()
        {
            int width = Host.UI.RawUI.WindowSize.Width;
            
            int num_columns = 4;
            int col_size = width / num_columns;
            // col_size = 40;
            // num_columns = width / col_size;

            WriteDebug($"width = {width} --- col_size = {col_size} --- num_columns = {num_columns}");

            int c = 0;
            foreach (var r in results)
            {
                int w = c * col_size;
                int cc = w + col_size;
                setFgColor(PowerDirTheme.getFgColor(r));
                // TODO: if c == 3 and r.Name > col_size, should start a new line.
                Host.UI.Write(r.Name);
                w += r.Name.Length;

                if (w < cc)
                    Host.UI.Write(new string(' ', (cc - w)));
                else
                {
                    // TODO if it wrote in over 2 columns?
                    int rest = w - cc;
                    while(rest > col_size)
                    {
                        //Coordinates coord = Host.UI.RawUI.CursorPosition;
                        //coord.X += col_size;
                        //Host.UI.RawUI.CursorPosition = coord;

                        Host.UI.Write(new string(' ', col_size));
                        rest -= col_size;
                        c++;
                    }

                    // w>=cc => w-cc >= 0 (number of chars over column end)
                    Host.UI.Write(new string(' ', col_size - rest));
                    // skip 1 column;
                    c++;
                }

                c++;
                if (c >= num_columns)
                {
                    Host.UI.WriteLine();
                    c = 0;
                }
            }
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
                case DisplayOptions.Wide:
                    displayWide();
                    break;
            }

            base.EndProcessing();
        }
    }
}
