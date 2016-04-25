# Smallp0wnedShell

Small modification version of PowerShell Runspace Post Exploitation Toolkit (p0wnedShell)

###p0wnedShell Author: Cn33liz and Skons

###p0wnedShell Source: https://github.com/Cn33liz/p0wnedShell

License: BSD 3-Clause

---

![Alt text](/p0wnedShell/Smallp0wnedShell.ico?raw=true "Smallp0wnedShell")


### What is Smallp0wnedShell:

p0werShell is an offensive PowerShell host application written in C# that does not rely on powershell.exe but runs powershell commands and functions within a powershell runspace environment (.NET). 

Smallp0wnedShell is just a small modification version of p0wnedShell.

### What is different:

[+] Remove all the offensive PowerShell modules and binaries of p0wnedShell


[+] Convert .NET Framework 4.0 to 2.0


[+] Reduce the size to 32kb

### How to Compile it:

####1.You need to install LINQBridge to use Linq with Microsoft .NET Framework 2.0

read this guide:

https://docs.nuget.org/consume/package-manager-console

run the following command in the Package Manager Console

`Install-Package LinqBridge`

####2.Build it by Visual Studio(My version is 2015)

### Contact:

To report an issue or request a feature, feel free to contact me at:
[@3gstudent](https://twitter.com/3gstudent)

