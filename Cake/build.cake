#tool "nuget:?package=vswhere"
#tool "nuget:?package=Cake.Bakery"
#tool "nuget:?package=Squirrel.Windows"

#addin Cake.Incubator
#addin Cake.Squirrel
#addin nuget:?package=SharpZipLib
#addin nuget:?package=Cake.Compression
#addin nuget:?package=Cake.OctoDeploy

//////////////////////////////////////////////////////////////////////
// Constants
//////////////////////////////////////////////////////////////////////

const string SolutionPath ="..\\Maple.sln";
const string Platform = "anyCPU";
const string Configuration = "Release";
const string PackagePath = ".\\Package";
const string InstallerPath = ".\\Releases";
const string ArchivePath = ".\\Archive";

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// Enviroment information and other local variables
//////////////////////////////////////////////////////////////////////

var testsDirectories = new List<string>();
var assemblyInfoParseResult = default(AssemblyInfoParseResult);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("VSWhereAll")
    .Does(()=>
    {
        foreach(var item in VSWhereAll())
            Information(item.FullPath);
    });

Task("Gather-BuildRequirements")
    .IsDependentOn("VSWhereAll")
    .Does(() =>
    {
        var tools = new List<string>()
        {
            @".\Common7\IDE\Extensions\TestPlatform\vstest.console.exe",
            @".\Common7\IDE\MSTest.exe",
            @".\MSBuild\15.0\Bin\MSBuild.exe",
            @".\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe",
        };

        // source https://docs.microsoft.com/en-us/visualstudio/install/workload-and-component-ids
        var products = new List<string>
        {
            "Microsoft.VisualStudio.Product.Enterprise",
            "Microsoft.VisualStudio.Product.Professional",
            "Microsoft.VisualStudio.Product.Community",
            "Microsoft.VisualStudio.Product.TeamExplorer",
            "Microsoft.VisualStudio.Product.BuildTools",
            "Microsoft.VisualStudio.Product.TestAgent",
            "Microsoft.VisualStudio.Product.TestController",
            "Microsoft.VisualStudio.Product.TestProfessional",
            "Microsoft.VisualStudio.Product.FeedbackClient",
        };

        var foundMSBuild = false;
        var foundVSTest = false;

        foreach(var product in products)
        {
            foreach(var directory in VSWhereProducts(product))
            {
                foreach(var tool in tools)
                {
                    var toolPath = directory.CombineWithFilePath(tool);
                    if(FileExists(toolPath))
                    {
                        Context.Tools.RegisterFile(toolPath);
                        if(tool.Contains("MSBuild.exe"))
                            foundMSBuild = true;
                        if(tool.Contains("vstest.console.exe"))
                            foundVSTest = true;
                    }
                }
            }
        }

        if(!foundMSBuild)
            Warning("MSBuild not found");

        if(!foundVSTest)
            Warning("VSTest not found");

        if(foundMSBuild && foundVSTest)
            Information("Required tools have been found.");
    });

Task("Clean-Solution")
    .IsDependentOn("Gather-BuildRequirements")
    .Does(() =>
    {
        var solution = ParseSolution(SolutionPath);

        foreach(var project in solution.Projects)
        {
            // check solution items and exclude solution folders, since they are virtual
            if(project.Name == "Solution Items")
                continue;

            if(project.Name == "Cake")
                continue;

            var customProject = ParseProject(project.Path, configuration: Configuration, platform: Platform);

            CleanDirectory(customProject.OutputPath.FullPath);

            if(customProject.OutputType != "Library") // WinExe
                continue;

            if(!project.Name.Contains("Test")) // we only care about test assemblies
                continue;

            testsDirectories.Add(customProject.OutputPath.FullPath);
        }
    });

Task("Clean-Folders")
    .Does(() =>
    {
        var folders = new[]
        {
            new DirectoryPath(PackagePath),
            new DirectoryPath(InstallerPath),
            new DirectoryPath(ArchivePath),
        };

        foreach(var folder in folders)
        {
            EnsureDirectoryExists(folder);
            CleanDirectory(folder);
        }
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean-Solution")
    .IsDependentOn("Clean-Folders")
    .Does(() =>
    {
        var settings = new NuGetRestoreSettings()
        {
            DisableParallelProcessing = false,
            Verbosity = NuGetVerbosity.Quiet,
            NoCache = true,
        };

        NuGetRestore(SolutionPath, settings);
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
    {
        var msBuildPath = Context.Tools.Resolve("msbuild.exe");

        var settings = new MSBuildSettings();

        if(msBuildPath != null)
            settings.ToolPath = msBuildPath;
        else
            settings.ToolVersion = MSBuildToolVersion.VS2017;

        settings.SetConfiguration(Configuration)
                .SetDetailedSummary(false)
                .SetMaxCpuCount(0)
                .SetMSBuildPlatform(MSBuildPlatform.Automatic);

        MSBuild(SolutionPath, settings);
    });

 Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
    {
        // http://cakebuild.net/blog/2017/03/vswhere-and-visual-studio-2017-support
        var testAssemblies = new List<FilePath>();
        var vsTest = Context.Tools.Resolve("vstest.console.exe");

        var settings = new VSTestSettings()
        {
            Parallel = true,
            InIsolation = true,
        };

        if(vsTest!= null)
            settings.ToolPath = Context.Tools.Resolve("vstest.console.exe");

        Information("Gathering Test-Assemblies");
        foreach(var path in testsDirectories)
        {
            var files = GetFiles(path + "/*.Test.dll");

            foreach(var file in files)
            {
                testAssemblies.Add(file);
                Information(file);
            }
        }

        VSTest(testAssemblies, settings);
    });

Task("Parse-AssemblyInfo")
    .Does(() =>
    {
        assemblyInfoParseResult = ParseAssemblyInfo("..\\src\\Resources\\SharedAssemblyInfo.cs");
    });

Task("Pack-Application")
    .WithCriteria(()=> assemblyInfoParseResult != null)
    .IsDependentOn("Parse-AssemblyInfo")
    .IsDependentOn("Run-Unit-Tests")
    .Does(()=>
    {
        Information(new FilePath("..\\src\\Resources\\Images\\logo.ico").MakeAbsolute(Context.Environment));
        var settings = new NuGetPackSettings()
        {
            Id                          = "Maple",
            Version                     = $"{assemblyInfoParseResult.AssemblyVersion}",
            Authors                     = new[] {"Insire"},
            Owners                      = new[] {"Insire"},
            Description                 = $"Maple v{assemblyInfoParseResult.AssemblyVersion}",
            Summary                     = "Maple is a windows desktop application designed to support semi and non professional streamers in playing back local audio files and streaming content from the internet to their favorite playback device",
            ProjectUrl                  = new Uri(@"https://github.com/Insire/Maple/"),
            IconUrl                      = new Uri(new FilePath("..\\src\\Resources\\Images\\logo.ico").MakeAbsolute(Context.Environment).FullPath, UriKind.Absolute),
            LicenseUrl                  = new Uri(@"https://github.com/Insire/Maple/blob/master/license.md"),
            Copyright                   = $"© {DateTime.Today.Year} Insire",
            ReleaseNotes                = new[]{""},
            Tags                        = new[]{"Maple", "Materia Player", "MediaPlayer", "Material", "WPF", "Windows", "C#", "Csharp", "Material Design"},
            RequireLicenseAcceptance    = true,
            Symbols                     = false,
            NoPackageAnalysis           = false,
            Files                       = new[]
            {
                new NuSpecContent{ Source="*",Target="lib\\net471", Exclude="*.pdb"},
                new NuSpecContent{ Source=".\\Resources\\",Target="lib\\net471\\Resources\\", Exclude="*.pdb"},
                new NuSpecContent{ Source=".\\x64\\*",Target="lib\\net471\\x64\\", Exclude="*.pdb"},
                new NuSpecContent{ Source=".\\x86\\*",Target="lib\\net471\\x86\\", Exclude="*.pdb"},
            },
            BasePath                    = new DirectoryPath("..\\src\\Maple\\bin\\Release\\"),
            OutputDirectory             = new DirectoryPath(PackagePath),
            KeepTemporaryNuSpecFile     = false,
        };

        NuGetPack(settings);
    });

Task("Create-Installer")
    .WithCriteria(()=> assemblyInfoParseResult != null)
    .IsDependentOn("Pack-Application")
	.Does(() =>
    {
		var settings = new SquirrelSettings()
        {
            NoMsi = true,
            Silent = true,
            ReleaseDirectory = new DirectoryPath(InstallerPath), // breaks Squirrel if not set to "Releases", maybe? idk anymore
            Icon = new FilePath("..\\src\\Resources\\Images\\logo.ico"),
            SetupIcon =  new FilePath("..\\src\\Resources\\Images\\logo.ico"),
            ShortCutLocations = "Desktop,StartMenu",
        };

        // nupkg file cant be in the Releases folder
        var nupkg = new DirectoryPath(PackagePath).CombineWithFilePath(new FilePath($".\\Maple.{assemblyInfoParseResult.AssemblyVersion}.nupkg"));

		 Squirrel(nupkg, settings);
	});

Task("Rename-Installer")
    .WithCriteria(()=> assemblyInfoParseResult != null)
    .IsDependentOn("Create-Installer")
    .Does(()=>
    {
        var root = new DirectoryPath(InstallerPath);
        var source = root.CombineWithFilePath(new FilePath("Setup.exe"));
        var target = root.CombineWithFilePath(new FilePath($".\\MapleSetup-v{assemblyInfoParseResult.AssemblyVersion}.exe"));

        MoveFile(source, target);
    });

Task("Compress-Installer") // this task might be obsolete for squirrel - might break the auto updater?
    .WithCriteria(()=> assemblyInfoParseResult != null)
    .IsDependentOn("Rename-Installer")
    .Does(()=>
    {
        var installer = new DirectoryPath(InstallerPath);
        var archive = new DirectoryPath(ArchivePath);
        var source = installer.CombineWithFilePath(new FilePath($".\\MapleSetup-v{assemblyInfoParseResult.AssemblyVersion}.exe"));

        ZipCompress(installer, archive.CombineWithFilePath(new FilePath(".\\MapleSetup.zip")), new[]{source});
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Compress-Installer");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
