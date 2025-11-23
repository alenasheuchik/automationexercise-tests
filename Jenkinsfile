pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
			
                checkout scm
            }
        }

        stage('Restore packages') {
            steps {

                bat 'nuget restore AutomationExercise.Tests.sln'
            }
        }

        stage('Build') {
            steps {

                bat 'msbuild AutomationExercise.Tests.sln /p:Configuration=Debug'
            }
        }

        stage('Run tests') {
            steps {

                bat '''
packages\\NUnit.ConsoleRunner.3.17.0\\tools\\nunit3-console.exe ^
  AutomationExercise.Tests\\bin\\Debug\\AutomationExercise.Tests.dll ^
  --result=TestResult.xml;format=nunit3
'''
            }
        }

        stage('Allure report') {
            steps {

                allure([
                    includeProperties: false,
                    jdk: '',
                    results: [[path: 'allure-results']],
                    reportBuildPolicy: 'ALWAYS'
                ])
            }
        }
    }

    post {
        always {

            archiveArtifacts artifacts: 'TestResult.xml', fingerprint: true
        }
    }
}