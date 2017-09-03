#tool "nuget:?package=vswhere"

#addin Cake.Incubator
//////////////////////////////////////////////////////////////////////
// Constants
//////////////////////////////////////////////////////////////////////

var solutionPath ="../Maple.sln";
var platform = "anyCPU";
var testSettingsPath = "../.runsettings";
var testResultsPath ="../testResults.trx";

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// Enviroment information
//////////////////////////////////////////////////////////////////////

var testsDirectories = new List<string>();

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

Task("Clean")
    .IsDependentOn("Gather-BuildRequirements")
    .Does(() =>
    {
        var solution = ParseSolution(solutionPath);

        foreach(var project in solution.Projects)
        {
            // check solution items and exclude solution folders, since they are virtual
            if(project.Name == "Solution Items")
                continue;

            if(project.Name == "Cake")
                continue;

            var customProject = ParseProject(project.Path, configuration: configuration, platform: platform);

            CleanDirectory(customProject.OutputPath.FullPath);

            if(customProject.OutputType != "Library") // WinExe
                continue;

            if(!project.Name.Contains("Test")) // we only care about test assemblies
                continue;

            testsDirectories.Add(customProject.OutputPath.FullPath);
        }
    });

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        var settings = new NuGetRestoreSettings()
        {
            DisableParallelProcessing = false,
            Verbosity = NuGetVerbosity.Quiet, //Detailed, Normal, Quiet
            NoCache = true,
        };

        NuGetRestore(solutionPath, settings);
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

        settings.SetConfiguration(configuration)
                .SetDetailedSummary(false)
                .SetMaxCpuCount(0)
                .SetMSBuildPlatform(MSBuildPlatform.Automatic);

        MSBuild(solutionPath, settings);
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

        VSTest(testAssemblies,settings);
    });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
