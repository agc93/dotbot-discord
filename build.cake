#load "./scripts/version.cake"

var target = Argument<string>("target", "Default");
var config = Argument<string>("configuration", "Release");

var version = new BuildVersion("0.1.0", "local");

var solutionPath = File("./src/Dotbot.Discord.sln");
var solution = ParseSolution(solutionPath);
var projects = solution.Projects.Where(p => p.Type != "{2150E333-8FDC-42A3-9474-1A3956D46DE8}");
var projectPaths = projects.Select(p => p.Path.GetDirectory());

var isRunningOnAppVeyor = BuildSystem.AppVeyor.IsRunningOnAppVeyor;
var isPullRequest = BuildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;

Setup(context => 
{
    // Calculate semantic version.
    version = GetVersion(context);

    // Output some information.
    Information("Version: {0}", version.GetSemanticVersion());
    Information("Pull Request: {0}", isPullRequest);
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory("./.artifacts");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore(solutionPath, new DotNetCoreRestoreSettings
    {
        Verbose = false,
        Sources = new [] { "https://api.nuget.org/v3/index.json" }
    });
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() =>
{
    // Build
    DotNetCoreBuild(solutionPath, new DotNetCoreBuildSettings 
    {
        Configuration = config,
        ArgumentCustomization = args => args.Append("/p:Version={0}", version.GetSemanticVersion())
    });
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    // Publish
    foreach(var project in projects.Select(p => p.Name))
    {
        Information("\nPacking {0}...", project.FullPath);
        DotNetCorePack(project.FullPath, new DotNetCorePackSettings 
        {
            Configuration = config,
            OutputDirectory = "./.artifacts",
            VersionSuffix = version.Suffix,
            NoBuild = true,
            Verbose = false,
            ArgumentCustomization = args => args
                .Append("/p:Version={0}", version.GetSemanticVersion())
                .Append("--include-symbols --include-source")
        });
    }
});

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);