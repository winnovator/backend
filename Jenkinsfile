pipeline {
  agent any
  stages {
    stage('Build') {
	  steps {
		withCredentials([string(credentialsId: 'sonarID', variable: 'sonarID')]) {
		  sh 'dotnet /opt/sonar-scanner-msbuild/SonarScanner.MSBuild.dll begin /k:"WInnovatorBackend" /d:sonar.host.url="https://sonar.owntournament.org" /d:sonar.login="$sonarID" /d:sonar.cs.opencover.reportsPaths="WInnovatorTest/coverage.opencover.xml" /d:sonar.scm.provider=git /d:sonar.exclusions="**/*.js,**/*.css,**.*.*html,**/DAL/Migrations/**.*"'
		  sh 'dotnet build'
		  sh 'dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
		  sh 'dotnet /opt/sonar-scanner-msbuild/SonarScanner.MSBuild.dll end /d:sonar.login="$sonarID"'
		}
	  }
    }
  }
}