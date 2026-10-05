# Settings: coverage.config (no XML comments - the coverage extension rejects them).
# Collects code coverage with Microsoft.Testing.Platform (Microsoft.Testing.Extensions.CodeCoverage,
# bundled with MSTest.Sdk) and renders an HTML/lcov report with ReportGenerator.
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$rootDir = "$scriptDir\.."
$resultsDir = "$rootDir\TestResults"
$coverageFiles = "$resultsDir\**\*.cobertura.xml"
$coverageReportDir = "$rootDir\coverage"
$coverageSettings = "$rootDir\coverage.config"
$generatorTests = "$rootDir\CapnpC.CSharp.Generator.Tests\CapnpC.CSharp.Generator.Tests.csproj"
$runtimeTests = "$rootDir\Capnp.Net.Runtime.Tests\Capnp.Net.Runtime.Tests.csproj"

If(test-path $resultsDir) {
  Remove-Item -Recurse -Force $resultsDir
}

If(!(test-path $coverageReportDir)) {
  New-Item -ItemType Directory -Force -Path $coverageReportDir
}

foreach ($project in @($generatorTests, $runtimeTests)) {
  & dotnet test --project $project `
    --filter TestCategory=Coverage `
    --configuration Release `
    --coverage `
    --coverage-output-format cobertura `
    --coverage-settings $coverageSettings `
    --results-directory $resultsDir
}

ReportGenerator.exe -reports:"$coverageFiles" -targetdir:"$coverageReportDir" -reportTypes:"Html;lcov"
