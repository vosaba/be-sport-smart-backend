param(
    [string]$MigrationName,
    [string]$Context,
    [string]$OutputDir
)

if (-not $MigrationName) {
    Write-Host "Migration name is required."
    exit 1
}

$rootPath = (Get-Item -Path ".\").FullName -replace '\\tools\\migration_creator$', ''
$projectPath = "$rootPath\src\Bss.Dal.Migrations"
$configFile = "$rootPath\src\Bss.Dal\Bss.Dal.config.json"

cd $projectPath

dotnet ef migrations add $MigrationName --context $Context --output-dir $OutputDir -- $configFile

if ($LASTEXITCODE -eq 0) {
    Write-Host "Migration for $Context created successfully."
} else {
    Write-Host "Failed to create migration for $Context."
}
