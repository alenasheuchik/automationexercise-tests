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
                dir('AutomationExercise.Tests') {
                    bat 'nuget.exe is in repo rootnest to .sln bat 'nuget.exe restore AutomationExercise.Tests.sln -PackagesDirectory.\\packages'
                }
            }
        }

        stage('Build') {
            steps {
                dir('AutomationExercise.Tests') {
                    bat 'dotnet build --configuration Debug'
                }
            }
        }

        stage('Test') {
            steps {
                dir('AutomationExercise.Tests') {
                    bat 'dotnet test AutomationExercise.Tests.csproj --logger:"trx;LogFileName=test-result.trx"'
                }
            }
        }
    }

    post {
        always {

            script {
                allure([
                    includeProperties: false,
                    jdk: '',
                    results: [[path: 'AutomationExercise.Tests\\bin\\Debug\\allure-results']],
                    reportBuildPolicy: 'ALWAYS'
                ])
            }


            archiveArtifacts artifacts: 'AutomationExercise.Tests\\**\\*.trx', allowEmptyArchive: true
        }
    }
}
