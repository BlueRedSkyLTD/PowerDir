using System.Management.Automation;
using System.Management.Automation.Host;
using System;
using System.Text;
using PowerDir.views;
using PowerDir.themes;

namespace PowerDir
{
    /// <summary>
    /// TODO: Pipeline inputs!
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
        private bool _stop = false;

        #region Parameters

        /// <summary>
        /// 
        /// </summary>
        [Parameter(HelpMessage = "About GetPowerDir information")]
        public SwitchParameter About {get; set;}

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

        /// <summary>
        /// <para type="description">Search Recursively (default: No)</para>
        /// </summary>
        [Parameter(
            HelpMessage ="Search Recursively (default: No)",
            ParameterSetName = "Recursion"
            )]
        [Alias("r")]
        public SwitchParameter Recursive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Parameter(HelpMessage = "Disable colors (default: no)")]
        [Alias("n")]
        public SwitchParameter NoColor { get; set; }

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

        int _width = 120;
        // TODO: consider to use just writeObject generating a string instead as it can support color with ESC[ sequence

        private string basePath = "./";
        private readonly EnumerationOptions enumerationOptions = new EnumerationOptions();

        // TODO: pagination

        // TODO: get-power-dir attributes, datetime, size, etc..

        private IPowerDirTheme? _theme;
        private IView? view;

        private void checkWidthSupport()
        {
            try
            {
                _width = Host.UI.RawUI.WindowSize.Width;
            }
            catch (HostException e)
            {
                WriteError(e.ErrorRecord);
            }
        }

        private bool supportEscapeCodes()
        {
            try
            {
                string expectedResponse = AbstractEscapeCodesTheme.ResponseDevice();
                int i = 0;
                WriteObject(AbstractEscapeCodesTheme.QueryDevice());
                
                while (Host.UI.RawUI.KeyAvailable)
                {
                    var key = Host.UI.RawUI.ReadKey();
                    if (expectedResponse[i] != key.Character)
                        break;

                    i++;
                    if (i >= expectedResponse.Length)
                        return true;
                }
            } catch(HostException e)
            {
                WriteError(e.ErrorRecord);
            }

            return false;
        }

        private void processPath()
        {
            WriteDebug($"[START] Path = {Path} --- basePath = {basePath}");
            Path = Path.Replace(System.IO.Path.AltDirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar);
            
            if (Path.StartsWith("$HOME"))
                Path = Path.Replace("$HOME", "~");
            if (Path.StartsWith("~"))
            {
                basePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                Path = Path.Substring(Path.Length >= 2 ? 2 : 1); // strip out starting of '~'
            }

            Path = System.IO.Path.Combine(basePath, Path);
            WriteDebug($"Normalized Absolute Path = {Path}");
            basePath = System.IO.Path.GetFullPath(Path);
            var p = System.IO.Path.GetDirectoryName(Path);
            // if p == null is root dir
            if (p != null && Path != p)
            {
                basePath = System.IO.Path.Combine(basePath, p);
                var split = Path.Split(p);
                WriteDebug($"Path split = [{String.Join(',', split)}]");
                int index = split[1].StartsWith(System.IO.Path.DirectorySeparatorChar) ? 1 : 0;
                // Sanity check
                if(split.Length != 2 || split[0].Length > 0)
                    throw new NotImplementedException("Path.Split(p) unexpected result");
                Path = split[1][index..];
            }

            if (Directory.Exists(Path) || Directory.Exists(System.IO.Path.Combine(basePath, Path)))
            {
                basePath = System.IO.Path.Combine(basePath, Path);
                Path = "*";
            }

            if (string.IsNullOrEmpty(Path)) Path = "*";

            WriteDebug($"[END] Path = {Path} --- basePath = {basePath}");
        }

        private void aboutInfo()
        {
            WriteObject(GetPowerDirAbout.line1);
            WriteObject(GetPowerDirAbout.line2);
            WriteObject(GetPowerDirAbout.line3);
            WriteObject(GetPowerDirAbout.line4);
            if (NoColor) 
                WriteObject(GetPowerDirAbout.showTheme(new NoColorTheme()));
            else
                WriteObject(GetPowerDirAbout.showTheme(new EscapeCodesTheme16()));
        }

        /// <summary>
        ///
        /// </summary>
        protected override void BeginProcessing()
        {
            WriteDebug($"Host Name = {Host.Name}");
            basePath = SessionState.Path.CurrentFileSystemLocation.Path;
            WriteDebug($"basePath = {basePath} --- Path = {Path}");

            checkWidthSupport();
            bool supportEscCode = supportEscapeCodes();
            WriteDebug($"Escape codes support: {(supportEscCode ? "y" : "n")}");

            // no escape codes support, no color support
            if (!supportEscCode)
                NoColor = true;

            if (NoColor)
                _theme = new NoColorTheme();
            else
                //_theme = new EscapeCodesTheme256();
                //_theme = new EscapeCodesThemeRGB();
                _theme = new EscapeCodesTheme16();

            WriteDebug($"Width = {_width}");
            WriteDebug($"Recursive = {Recursive}");

            if (About)
            {
                aboutInfo();
                StopProcessing();
                return;
            }

            // TODO:
            //WriteDebug($"Extensions = {String.Join(',', _theme._extensions)}");
            processPath();

            enumerationOptions.RecurseSubdirectories = Recursive;
            enumerationOptions.MaxRecursionDepth = Level;
            enumerationOptions.IgnoreInaccessible = true;
            enumerationOptions.MatchCasing = MatchCasing.PlatformDefault;
            enumerationOptions.AttributesToSkip = 0;

            // UNICODE Example:
            //var rune = new Rune(0x1F4BE);
            //WriteObject(rune.ToString());

            // TODO
            //if (pagination)
            //{
            //    WriteDebug("paginated results");
            //    WriteDebug(PagingParameters.ToString());
            //}

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
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessRecord()
        {
            if (_stop) return;
            // TODO: not sure if it is nice this branch, but don't know how to visualize the directory first,
            //       unless i am going to implement my recursive method to discover direcories and files with in it.
            //       at the moment this is the quickest way. I don't want yet to implement my own search recursive method.
            if (Recursive)
            {
                foreach (string fileSys in Directory.EnumerateFileSystemEntries(basePath, Path, enumerationOptions))
                {
                    FileSystemInfo info = Directory.Exists(fileSys) ?
                        new DirectoryInfo(fileSys) :
                        new FileInfo(fileSys);

                    view?.displayResult(new GetPowerDirInfo(info, basePath), _theme!);
                    if (_stop)
                        return;
                }
            }
            else
            {
                foreach (string dir in Directory.EnumerateDirectories(basePath, Path, enumerationOptions))
                {
                    var dirInfo = new DirectoryInfo(dir);
                    view?.displayResult(new GetPowerDirInfo(dirInfo, basePath), _theme!);
                    if (_stop)
                        return;
                }

                foreach (string file in Directory.EnumerateFiles(basePath, Path, enumerationOptions))
                {
                    var fileInfo = new FileInfo(file);
                    view?.displayResult(new GetPowerDirInfo(fileInfo, basePath), _theme!);
                    if (_stop)
                        return;
                }
            }
        }

        private void displayObject()
        {
            view = new DefaultView(WriteObject);
        }

        private void displayList()
        {
            view = new ListView(WriteObject);
        }
        private void displayListDetails()
        {
            // TODO
            //      permissions?
            //      etc
            // TODO switch parameter for dateTime type
            view = new ListDetailsView(_width, MAX_NAME_LENGTH,
                WriteObject, ListDetailsView.EDateTimes.CREATION);
        }

        private void displayWide()
        {
            // TODO as a dynamic parameter when using displayWide
            int num_columns = 4;
            
            int col_size = _width / num_columns;
            // col_size = 40;
            // num_columns = width / col_size;

            WriteDebug($"width = {_width} --- col_size = {col_size} --- num_columns = {num_columns}");

            view = new WideView(_width, num_columns, WriteObject);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void EndProcessing()
        {
            view?.endDisplay();
        }
        
        /// <summary>
        /// 
        /// </summary>
        protected override void StopProcessing()
        {
            _stop = true;
            view?.endDisplay();
        }
    }
}
