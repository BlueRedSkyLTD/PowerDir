// See https://aka.ms/new-console-template for more information

using PowerDir;
using System;
using System.Management.Automation;

Console.WriteLine("Hello, World!");


PowerShell ps = PowerShell.Create();
//ps.AddCommand("Import-Module").AddParameter("Assembly", System.Reflection.Assembly.GetExecutingAssembly());
//ps.Invoke();
//ps.Commands.Clear();

ps.AddCommand(new CmdletInfo("Get-PowerDir", typeof(GetPowerDir)));
var out1 = ps.Invoke();

//GetPowerDir pd = new GetPowerDir();



//var cmd1 = pd.InvokeCommand.GetCmdlet("Get-PowerDir");
//var out1 = pd.InvokeCommand.InvokeScript("Get-PowerDir");

foreach (object o in out1)
{
    Console.WriteLine(o);
}

