using System.Reflection;


[assembly: AssemblyTitle("ServiceHost")]
[assembly: AssemblyCompany("Jaricardodev")]
[assembly: AssemblyProduct("Paginator")]
[assembly: AssemblyCopyright("Copyright ©  2021")]

[assembly: AssemblyVersion(ThisAssembly.Git.BaseVersion.Major + "." + ThisAssembly.Git.BaseVersion.Minor + "." + ThisAssembly.Git.BaseVersion.Patch)]

[assembly: AssemblyFileVersion(ThisAssembly.Git.BaseVersion.Major + "." + ThisAssembly.Git.BaseVersion.Minor + "." + ThisAssembly.Git.BaseVersion.Patch)]

[assembly: AssemblyInformationalVersion(
    ThisAssembly.Git.BaseVersion.Major + "." +
    ThisAssembly.Git.BaseVersion.Minor + "." +
    ThisAssembly.Git.BaseVersion.Patch + "+" +
    ThisAssembly.Git.Branch + "+" +
    ThisAssembly.Git.Commit)]
