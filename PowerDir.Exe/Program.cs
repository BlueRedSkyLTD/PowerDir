// See https://aka.ms/new-console-template for more information

using PowerDir;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

Console.WriteLine("Hello, World!");

////  get-item *.*  | select Name
//var ps = PowerShell.Create().AddCommand("Select").AddParameter(null, "Length")

//    .AddStatement().AddCommand("Get-Item").AddParameter(null, "*.*");

// dir | select Name
Runspace runspace = RunspaceFactory.CreateRunspace();
runspace.Open();
Pipeline pipeline = runspace.CreatePipeline();
Command dir = new Command("dir");
pipeline.Commands.Add(dir);
Command select = new Command("select");
select.Parameters.Add(null, "Name");
pipeline.Commands.Add(select);
var out1 = pipeline.Invoke();

runspace.Close();

//PowerShell ps = PowerShell.Create();
//ps.AddCommand(new CmdletInfo("Get-PowerDir", typeof(GetPowerDir)))
//    .AddParameter("d", "ld")
//    .AddParameter("Debug");
//var out1 = ps.Invoke();


//ps.Commands.Clear();

foreach (object o in out1)
{
    Console.WriteLine(o);
}

