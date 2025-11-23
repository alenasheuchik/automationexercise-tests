pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Restore') {
            steps {
                // nuget.exe is in repo root next to .sln
                bat 'nuget.exe restore AutomationExercise.Tests.sln -PackagesDirectory .\\packages'
            }
        }

        stage('Build') {
            steps {
                // explicit path to MSBuild.exe
                bat '''"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" AutomationExercise.Tests.sln /p:Configuration=Debug'''
            }
        }

        stage('Test') {
            steps {
                bat '''
packages\\NUnit.ConsoleRunner.3.17.0\\tools\\nunit3-console.exe ^
  AutomationExercise.Tests\\bin\\Debug\\AutomationExercise.Tests.dll ^
  --result=TestResult.xml;format=nunit3
'''
            }
        }

        stage('Allure') {
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