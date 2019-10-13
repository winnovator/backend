pipeline {
  agent any
  stages {
    stage('Build') {
      steps {
        sh '''dotnet /opt/sonar-scanner-msbuild/SonarScanner.MSBuild.dll begin /k:"WInnovatorBackend" /d:sonar.host.url="https://sonar.owntournament.org" /d:sonar.login="e8e8e2d4f4962d4ad2d5d59dad2009258b2630f8" /d:sonar.cs.opencover.reportsPaths="WInnovatorTest/coverage.opencover.xml" /d:sonar.scm.provider=git /d:sonar.exclusions="**/*.js,**/*.css,**.*.html,**/DAL/Migrations/**.*"
dotnet build
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
dotnet /opt/sonar-scanner-msbuild/SonarScanner.MSBuild.dll end /d:sonar.login="e8e8e2d4f4962d4ad2d5d59dad2009258b2630f8"'''
      }
    }
  }
}