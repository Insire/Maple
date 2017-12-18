#tool "nuget:?package=vswhere"
#tool "nuget:?package=Cake.Bakery"
#tool "nuget:?package=Squirrel.Windows"

#addin "Cake.Incubator"
#addin "Cake.Squirrel"
#addin "Cake.FileSet"
#addin "nuget:?package=SharpZipLib"
#addin "nuget:?package=Cake.Compression"
#addin "nuget:?package=Microsoft.Extensions.FileSystemGlobbing&version=1.1.1"

//////////////////////////////////////////////////////////////////////
// Constants
//////////////////////////////////////////////////////////////////////

const string VsWhereTask = "VSWhereAll";
const string GatherBuildRequirementTask = "Gather-BuildRequirements";
const string CleanSolutionTask = "Clean-Solution";
const string CleanFoldersTask = "Clean-Folders";
const string RestoreNugetTask = "Restore-NuGet-Packages";
const string BuildTask = "Build";
const string UnitTestTask = "Run-Unit-Tests";
const string AssemblyInfoTask = "Parse-And-Update-AssemblyInfo";
const string PackTask = "Pack-Application";
const string InstallerCreationTask = "Create-Installer";
const string InstallerRenameTask = "Rename-Installer";
const string InstallerCompressionTask = "Compress-Installer";
const string BuildServerTask  ="Update-BuildServerEnvironment";

const string SolutionPath ="..\\Maple.sln";
const string AssemblyInfoPath ="..\\src\\Resources\\SharedAssemblyInfo.cs";
const string Platform = "anyCPU";
const string Configuration = "Release";
const string PackagePath = ".\\Package";
const string InstallerPath = ".\\Releases";
const string ArchivePath = ".\\Archive";
const string TestResultsPath = ".\\TestResults";

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

Task(VsWhereTask)
    .Does(()=>
    {
        foreach(var item in VSWhereAll())
            Information(item.FullPath);
    });

Task(GatherBuildRequirementTask)
    .IsDependentOn(VsWhereTask)
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

Task(CleanSolutionTask)
    .IsDependentOn(GatherBuildRequirementTask)
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

Task(CleanFoldersTask)
    .Does(() =>
    {
        var folders = new[]
        {
            new DirectoryPath(PackagePath),
            new DirectoryPath(InstallerPath),
            new DirectoryPath(ArchivePath),
            new DirectoryPath(TestResultsPath),
        };

        foreach(var folder in folders)
        {
            EnsureDirectoryExists(folder);
            CleanDirectory(folder);
        }
    });

Task(RestoreNugetTask)
    .IsDependentOn(CleanSolutionTask)
    .IsDependentOn(CleanFoldersTask)
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

Task(AssemblyInfoTask)
    .Does(() =>
    {
        assemblyInfoParseResult = ParseAssemblyInfo(AssemblyInfoPath);

            var settings = new AssemblyInfoSettings()
            {
                Version                 = assemblyInfoParseResult.AssemblyVersion,
                FileVersion             = assemblyInfoParseResult.AssemblyFileVersion,
                InformationalVersion    = assemblyInfoParseResult.AssemblyInformationalVersion,

                Product                 = assemblyInfoParseResult.Product,
                Company                 = assemblyInfoParseResult.Company,
                Trademark               = assemblyInfoParseResult.Trademark,
                Copyright               = string.Format("© {0} Insire", DateTime.Now.Year),

                ComVisible              = assemblyInfoParseResult.ComVisible,
                InternalsVisibleTo      = assemblyInfoParseResult.InternalsVisibleTo,

                // invalid entries:
                // Configuration           = assemblyInfoParseResult.Configuration,
                // Description             = assemblyInfoParseResult.Description,
                // Guid                    = assemblyInfoParseResult.Guid,
                // Title                   = assemblyInfoParseResult.Title,

                // posssible missing entries
                // CustomAttributes     = assemblyInfoParseResult.CustomAttributes,
                // CLSCompliant         = assemblyInfoParseResult.CLSCompliant,
            };

        if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        {
            var version = EnvironmentVariable("APPVEYOR_BUILD_VERSION") ?? "no version found from AppVeyorEnvironment";

            Information($"Version: {version}");

            settings.Version                 = EnvironmentVariable(version) ?? assemblyInfoParseResult.AssemblyVersion;
            settings.FileVersion             = EnvironmentVariable(version) ?? assemblyInfoParseResult.AssemblyFileVersion;
            settings.InformationalVersion    = EnvironmentVariable(version) ?? assemblyInfoParseResult.AssemblyInformationalVersion;
        }

        CreateAssemblyInfo(new FilePath(AssemblyInfoPath), settings);

        assemblyInfoParseResult = ParseAssemblyInfo(AssemblyInfoPath);
    });

Task(BuildTask)
    .IsDependentOn(RestoreNugetTask)
    .IsDependentOn(AssemblyInfoTask)
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

Task(UnitTestTask)
    .IsDependentOn(BuildTask)
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

        settings.Logger = "trx";

        if(vsTest != null)
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

Task(PackTask)
    .WithCriteria(()=> assemblyInfoParseResult != null)
    .IsDependentOn(AssemblyInfoTask)
    .IsDependentOn(UnitTestTask)
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

        Information($"Files found in {PackagePath}:");
        foreach(var file in GetFiles(PackagePath))
            Information(file.FullPath);
    });

Task(InstallerCreationTask)
    .WithCriteria(()=> assemblyInfoParseResult != null)
    .IsDependentOn(PackTask)
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

Task(InstallerRenameTask)
    .WithCriteria(()=> assemblyInfoParseResult != null)
    .IsDependentOn(InstallerCreationTask)
    .Does(()=>
    {
        var root = new DirectoryPath(InstallerPath);
        var source = root.CombineWithFilePath(new FilePath("Setup.exe"));
        var target = root.CombineWithFilePath(new FilePath($".\\MapleSetup-v{assemblyInfoParseResult.AssemblyVersion}.exe"));

        MoveFile(source, target);
    });

Task(InstallerCompressionTask) // this task might be obsolete for squirrel - might break the auto updater?
    .WithCriteria(()=> assemblyInfoParseResult != null)
    .IsDependentOn(InstallerRenameTask)
    .Does(()=>
    {
        var installer = new DirectoryPath(InstallerPath);
        var archive = new DirectoryPath(ArchivePath);
        var source = installer.CombineWithFilePath(new FilePath($".\\MapleSetup-v{assemblyInfoParseResult.AssemblyVersion}.exe"));

        ZipCompress(installer, archive.CombineWithFilePath(new FilePath(".\\MapleSetup.zip")), new[]{source});
    });

Task(BuildServerTask)
    .WithCriteria(()=> BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    .IsDependentOn(AssemblyInfoTask)
    .Does(()=>
    {
        var settings = new FileSetSettings()
        {
            BasePath=".\\TestResults",
            Includes = new string[]{"*.trx"},
            Excludes = new string[]{},
            CaseSensitive = false,
        };
        var files = GetFileSet(settings);

        foreach(var file in files)
        {
            Information($"Testfile {file.FullPath} found and uploading....");
            BuildSystem.AppVeyor.UploadTestResults(file, AppVeyorTestResultsType.MSTest);
        }
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn(InstallerCompressionTask)
    .IsDependentOn(BuildServerTask);

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
