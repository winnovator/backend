@echo off
dotnet sonarscanner begin /k:"WInnovatorBackend" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="73015ed16666f27b23acb0679ea9851a4b163ac3" /d:sonar.cs.opencover.reportsPaths="WInnovatorTest\coverage.opencover.xml" /d:sonar.coverage.exclusions="WInnovator/Data/Migrations/**/*.*,WInnovator/Pages/**/*.*,WInnovator/Startup.cs,WInnovator/Program.cs" -d:sonar.scm.provider=git
dotnet build
REM dotnet test --collect "Code coverage"
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
dotnet sonarscanner end /d:sonar.login="73015ed16666f27b23acb0679ea9851a4b163ac3"