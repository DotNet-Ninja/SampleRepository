# SampleRepository
Sample project to demo issue with Octokit DeleteFile.

## SetUp
1. Fork Repo
2. Create Personal Access Token (Should only require repo access)
3. Clone source
4. Add UserSecret for AccessToken and set it to the token created in step 2.
```
   {
     "AccessToken": "ghp_yourpersonalaccesstoken"
   }
```
5. Configure settings in appsettings.json to point to your repository
    1. In theory you should only need to change the OrgName

## Testing
Now that you are set up you can simply run the application.  Initially it is configured as .Net 7.0 so you should see the following error in the console:

```
System.ArgumentException: The value cannot be null or empty. (Parameter 'mediaType')
   at System.Net.Http.Headers.MediaTypeHeaderValue.CheckMediaTypeFormat(String mediaType, String parameterName)
   at System.Net.Http.StringContent..ctor(String content, Encoding encoding, String mediaType)
   at Octokit.Internal.HttpClientAdapter.BuildRequestMessage(IRequest request) in /home/runner/work/octokit.net/octokit.net/Octokit/Http/HttpClientAdapter.cs:line 144
   at Octokit.Internal.HttpClientAdapter.Send(IRequest request, CancellationToken cancellationToken) in /home/runner/work/octokit.net/octokit.net/Octokit/Http/HttpClientAdapter.cs:line 47
   at Octokit.Connection.RunRequest(IRequest request, CancellationToken cancellationToken) in /home/runner/work/octokit.net/octokit.net/Octokit/Http/Connection.cs:line 679
   at Octokit.Connection.Run[T](IRequest request, CancellationToken cancellationToken) in /home/runner/work/octokit.net/octokit.net/Octokit/Http/Connection.cs:line 670
   at Octokit.Connection.Delete(Uri uri, Object data) in /home/runner/work/octokit.net/octokit.net/Octokit/Http/Connection.cs:line 563
   at Program.Main(String[] args) in C:\Code\SampleRepository\src\SampleRepository\Program.cs:line 34
```

This will have created the branch, but failed to delete the file.

---

If you change the project to .Net 6.0 it will look like this (NOTE: You will need to manually delete the Test-Branch in between each run!):

```
Success
```

You should now have a branch with the samplefile.txt deleted.