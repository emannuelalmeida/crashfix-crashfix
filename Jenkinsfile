

pipeline {
	agent {
		node {
			label 'unity-build'
		}
	}
	stages {
		stage('Build') {
			steps {
				script {
					checkout scm
					sh """\
$UNITY_HOME/Unity -batchmode -nographics -executeMethod Editor.Builds.PerformWebGLBuild -projectPath '${pwd()}' -quit -logFile '${pwd()}/build.log'
"""
                    sh 'cat build.log'
				}
			}
		}
		stage('Deploy') {
			steps {
				script {
					echo 'To Do'
				}
			}
		}
	}
}