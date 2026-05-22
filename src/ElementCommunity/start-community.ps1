$ErrorActionPreference = "Stop"

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$project = Join-Path $root "ElementCommunity.App\ElementCommunity.App.csproj"
$url = "http://localhost:5096"

Write-Host "Starting ElementCommunity at $url"
dotnet run --project $project --urls $url
