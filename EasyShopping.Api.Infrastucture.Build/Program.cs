using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

internal class Program
{
    private static void Main(string[] args)
    {
        var githubPipeline = new GithubPipeline
        {
            Name = "Watch Build Pipeline",

            OnEvents = new Events
            {
                Push = new PushEvent
                {
                    Branches = new string[] { "master" }
                },
                PullRequest = new PullRequestEvent
                {
                    Branches = new string[] { "master" }
                },
            },
            Jobs = new Dictionary<string, Job>
            {
                {
                    "build",
                    new Job
                    {
                        RunsOn = BuildMachines.Windows2022,

                        Steps = new List<GithubTask>
                        {
                            new CheckoutTaskV2
                            {
                                Name = "Checking out Code"
                            },
                            new SetupDotNetTaskV1
                            {
                                Name = "Setting Up .Net",
                                TargetDotNetVersion = new TargetDotNetVersion
                                {
                                    DotNetVersion = "8.0.204"
                                }
                            },

                            new RestoreTask
                            {
                                Name = "Restoring Nuget Packages"
                            },
                            new DotNetBuildTask
                            {
                                Name = "Building Project"
                            },
                            new TestTask
                            {
                                Name = "Running Tests"
                            }
                        }
                    }
                }
            }
        };

        string buildScriptPath = "../../../../.github/workflows/dotnet.yml";
        string directoryPath = Path.GetDirectoryName(buildScriptPath);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        ADotNetClient aDotNetClient = new ADotNetClient();
        aDotNetClient.SerializeAndWriteToFile(githubPipeline, path: buildScriptPath);
    }
}
