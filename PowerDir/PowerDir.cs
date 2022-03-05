using System.Management.Automation;

namespace PowerDir
{
    [Cmdlet(VerbsCommon.Get,"PowerDir")]
    public class PowerDir : PSCmdlet
    {
        [Parameter(Position = 0)]
        public Object InputObject { get; set; }

        protected override void EndProcessing()
        {
            string[] files = System.IO.Directory.GetFiles("./");
            this.WriteObject(files, true);
            base.EndProcessing();
        }
    }
}