

pipeline {
    agent {
        node {
            label 'unity-build'
        }
    }
    stages {
        stage('Build') {
            when {
                not { changeset 'docs/**'  }
            }
            steps {
                script {
                    sh """\
$UNITY_HOME/Unity -batchmode -nographics -executeMethod Editor.Builds.PerformWebGLBuild -projectPath '${pwd()}' -quit -logFile '${pwd()}/build.log'
"""
                    sh 'cat build.log'
                }
            }
        }
        stage('Deploy') {
            when {
                allOf {
                    branch 'master'
                    not { changeset 'docs/**'  }
                }
            }
            steps {
                script {
                    withCredentials([sshUserPrivateKey(credentialsId: 'hwvenancio', keyFileVariable: 'SSH_KEY', usernameVariable: 'SSH_USER')]) {
                        withEnv([ "GIT_SSH_COMMAND=ssh -o StrictHostKeyChecking=no -o User=${SSH_USER} -i ${SSH_KEY}" ] ) {
                            sh '''
                                git remote add originssh git@github.com:emannuelalmeida/crashfix-crashfix.git || true
                                git checkout master
                                git rm -r docs
                                mv Build/crashfix-crashfix-web docs
                                git add docs
                                git commit 'updating github pages'
                                git push originssh master
                            '''
                        }
                    }                    
                }
            }
        }        
    }
}