using System.Management.Automation;
using System.Management.Automation.Host;


namespace PowerDir
{
    [Cmdlet(VerbsCommon.Get,"PowerDir")]
    public class GetPowerDir : PSCmdlet
    {
        [Parameter(Position = 0)]
        public Object? InputObject { get; set; }
        
        private ConsoleColor fg;
        private ConsoleColor bg;

        private IEnumerable<string>? dirs;
        private IEnumerable<string>? files;

        private bool supportColor = true;

        private void setFgColor(ConsoleColor fg)
        {
            if (!supportColor) return;
            Host.UI.RawUI.ForegroundColor = fg;
        }
        protected override void BeginProcessing()
        {
            this.WriteDebug($"Host.Name = {Host.Name}");
            this.WriteObject("here");
            try
            {
                this.fg = Host.UI.RawUI.ForegroundColor;
                this.bg = Host.UI.RawUI.BackgroundColor;
            } catch (HostException ex)
            {
                this.supportColor = false;
                this.WriteError(ex.ErrorRecord);
            }

            this.dirs = Directory.EnumerateDirectories("./");
            this.files = Directory.EnumerateFiles("./");
         
            base.BeginProcessing();
        }

        protected override void ProcessRecord()
        {
            setFgColor(ConsoleColor.Blue);
            this.WriteObject(dirs, true);

            setFgColor(ConsoleColor.Gray);
            this.WriteObject(files, true);

            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            
            
            base.EndProcessing();
        }
    }
}