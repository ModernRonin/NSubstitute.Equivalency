param ($version)

if (-not $nugetApiKey) {$nugetApiKey= $Env:NUGETAPIKEY}
if (-not $nugetApiKey) 
{
	Write-Host "You need to either set the powershell variable nugetApiKey or the environment variable NUGETAPIKEY before executing this script" 
	exit
}

if (-not $version) 
{
	[xml]$xml= Get-Content .\NSubstituteEquivalency\release.history
	$version= $xml.Project.PropertyGroup.Version.ToString()
}
if (-not $version) 
{
	Write-Host "Something went wrong, could not read version from release.history"
	exit 
}

dotnet pack --configuration Release
dotnet nuget push .\NSubstituteEquivalency\nupkg\NSubstituteEquivalency.$version.nupkg -s https://api.nuget.org/v3/index.json --api-key $nugetApiKey