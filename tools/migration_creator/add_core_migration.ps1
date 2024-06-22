param(
    [string]$MigrationName
)

$context = "CoreDbContext"
$outputDir = "Migrations/Core"

.\add_migration.ps1 -MigrationName $MigrationName -Context $context -OutputDir $outputDir