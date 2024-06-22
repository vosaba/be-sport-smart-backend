param(
    [string]$MigrationName
)

$context = "IdentityDbContext"
$outputDir = "Migrations/Identity"

.\add_migration.ps1 -MigrationName $MigrationName -Context $context -OutputDir $outputDir
