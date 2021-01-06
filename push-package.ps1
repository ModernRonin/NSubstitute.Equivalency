param ([parameter(mandatory)]$version)

if (-not $nugetApiKey) {Write-Host "You need to set the variable nugetApiKey"}

dotnet pack --configuration Release
dotnet nuget push .\NSubstituteEquivalency\nupkg\NSubstituteEquivalency.$version.nupkg -s https://api.nuget.org/v3/index.json --api-key $nugetApiKey