using System.Management.Automation;
using System.Management.Automation.Host;


namespace PowerDir
{
    [Cmdlet(VerbsCommon.Get,"PowerDir", SupportsPaging = true)]
    public class GetPowerDir : PSCmdlet
    {
        [Parameter(
            Position = 0,
            HelpMessage = "Path to search. Accepting wildcards"
        )]
        [SupportsWildcards()]

        public string path { get; set; } = "*";
        [Parameter]
        public SwitchParameter Pagination { get { return pagination; } set { pagination = value; } }
        bool pagination;
        
        private ConsoleColor fg;
        private ConsoleColor bg;

        private List<string> dirs = new List<string>();
        private List<string> files = new List<string>();

        private bool supportColor = true;

        private const string basePath = "./";
        private EnumerationOptions enumerationOptions = new EnumerationOptions();

        private void setFgColor(ConsoleColor fg)
        {
            if (!supportColor) return;
            Host.UI.RawUI.ForegroundColor = fg;
        }

        private void populateDirsAndFiles()
        {
            
        }
        protected override void BeginProcessing()
        {
            WriteDebug($"Host.Name = {Host.Name}");
            try
            {
                fg = Host.UI.RawUI.ForegroundColor;
                bg = Host.UI.RawUI.BackgroundColor;
            } catch (HostException ex)
            {
                supportColor = false;
                WriteError(ex.ErrorRecord);
            }

            enumerationOptions.ReturnSpecialDirectories = true;
            dirs = Directory.EnumerateDirectories(basePath, path, enumerationOptions).ToList();
            files = Directory.EnumerateFiles(basePath, path, enumerationOptions).ToList();

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

            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}