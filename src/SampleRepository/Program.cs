using System.Reflection;
using System.Runtime.Serialization.Json;
using Microsoft.Extensions.Configuration;
using Octokit;
using SampleRepository;

public static class Program
{
    public static async Task Main(string[] args)
    {
        // Setup
        var settings = LoadSettings();
        var client = CreateClient(settings);


        try
        {
            // Get default branch ref and create a new branch
            var repo = await client.Repository.Get(settings.OrgName, settings.RepositoryName);
            var baseRef = await client.Git.Reference
                .Get(settings.OrgName, settings.RepositoryName, $"heads/{repo.DefaultBranch}");

            await client.Git.Reference.Create(settings.OrgName, settings.RepositoryName,
                new NewReference("refs/heads/" + settings.BranchName, baseRef.Object.Sha));

            // Delete the sample file from the branch
            var fileSha = await client.Repository.Content
                            .GetAllContentsByRef(
                                settings.OrgName, 
                                settings.RepositoryName, 
                                settings.FilePath, 
                                settings.BranchName);

            await client.Repository.Content.DeleteFile(
                                settings.OrgName,
                                settings.RepositoryName,
                                settings.FilePath,
                                new DeleteFileRequest("Test Delete", fileSha[0].Sha, settings.BranchName));
        }
        catch (Exception e)
        {
            var ex = e;
            while (ex != null)
            {
                Console.WriteLine(ex);
                ex = ex.InnerException;
            }
            throw;
        }

        Console.WriteLine("Success");
    }

    public static Settings LoadSettings()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(Assembly.GetExecutingAssembly());
        var settings = new Settings();
        config.Build().Bind(settings);
        return settings;
    }

    public static IGitHubClient CreateClient(Settings settings)
    {
        var client = new GitHubClient(new ProductHeaderValue(settings.ClientName))
        {
            Credentials = new Credentials(settings.AccessToken)
        };
        return client;
    }

    public static async Task CreateBranch(IGitHubClient client, Repository repo, Settings settings)
    {

        var baseRef = await client.Git.Reference
                                .Get(settings.OrgName, settings.RepositoryName, $"heads/{repo.DefaultBranch}");

        await client.Git.Reference.Create(settings.OrgName, settings.RepositoryName, 
                                            new NewReference("refs/heads/" + settings.BranchName, baseRef.Object.Sha));
    }
}
