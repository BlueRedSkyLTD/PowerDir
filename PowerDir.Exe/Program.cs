// See https://aka.ms/new-console-template for more information

using PowerDir;
using System.Management.Automation;

Console.WriteLine("Hello, World!");

PowerShell ps = PowerShell.Create();
ps.AddCommand(new CmdletInfo("Get-PowerDir", typeof(GetPowerDir)))
    .AddParameter("Debug");
var out1 = ps.Invoke();

ps.Commands.Clear();

foreach (object o in out1)
{
    Console.WriteLine(o);
}

