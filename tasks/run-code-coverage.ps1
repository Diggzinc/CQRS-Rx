Write-Host "Code coverage for CQRS-Rx"

Write-Host "`Step 1: Setting up coverage run"

$tools = "./tools/"

# create tools directory if needed.
if (-not (Test-Path $tools)) {
    Write-Host "Directory '$tools' missing."
    Write-Host "Creating directory '$tools'."
    New-Item -ItemType directory -Path $tools > $null
    Write-Host "Directory '$tools' created."
}
# download NuGet if needed
$nugetSource = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$nuget = "$tools/nuget.exe"

if (Test-Path $nuget) {
    Write-Host "NuGet found."
}
else {
    Write-Host "NuGet not found"
    Write-Host "Downloading NuGet."
    Invoke-WebRequest $nugetSource -OutFile $nuget
    Write-Host "NuGet download completed."
}
Set-Alias nuget $nuget -Scope Script

# download OpenCover if needed
$openCoverVersion = "4.6.519";
$opencover = "$tools/OpenCover.$openCoverVersion/tools/OpenCover.Console.exe"; 

if (Test-Path $opencover) {
    Write-Host "OpenCover $openCoverVersion found."
}
else {
    Write-Host "OpenCover not found"
    Write-Host "Downloading OpenCover $openCoverVersion"
    nuget install OpenCover -Version $openCoverVersion -OutputDirectory $tools -Verbosity quiet
    Write-Host "OpenCover download completed."   
}
Set-Alias opencover $opencover -Scope Script

# download ReportGenerator if needed
$reportGeneratorVersion = "3.1.2";
$reportgenerator = "$tools/ReportGenerator.$reportGeneratorVersion/tools/ReportGenerator.exe"; 

if (Test-Path $reportGenerator) {
    Write-Host "ReportGenerator $reportGeneratorVersion found."
}
else {
    Write-Host "ReportGenerator not found"
    Write-Host "Downloading ReportGenerator $reportGeneratorVersion"
    nuget install ReportGenerator -Version $reportGeneratorVersion -OutputDirectory $tools -Verbosity quiet
    Write-Host "ReportGenerator download completed."   
}
Set-Alias reportgenerator $reportgenerator -Scope Script

# clean build project with full debug type.
$solution = "../src/CQRSRx.sln";
Write-Host "Step 2: Building solution"
dotnet clean $solution > $null
dotnet build $solution /p:DebugType=Full > $null
Write-Host "Solution built."

# running code coverage
$tests = "../src/CQRSRx.Tests";

Write-Host "Step 3: Running code coverage"

opencover "-register:user"`
    "-target:C:/Program Files/dotnet/dotnet.exe"`
    "-targetdir:$tests"`
    "-targetargs:test --configuration Debug --no-build"`
    "-excludebyattribute:*.ExcludeFromCodeCoverage*^"`
    "-filter:+[*]* -[*.Tests.*]*"`
    "-output:artifacts/coverage.xml"`
    "-oldStyle"`
    "-mergeoutput"`
    "-log:Warn"

Write-Host "Coverage ran."

# generating report
Write-Host "Step 4: Creating report"

reportgenerator "-reports:artifacts/coverage.xml"`
    "-targetdir:./artifacts/coverage"`
    "-filters:-*.Tests*;"`
    "-historydir:./artifacts/coverage/history" > $null

Write-Host "Report created."

#open report file
.\artifacts\coverage\index.htm