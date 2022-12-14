pipeline {
    agent any
    options {
        timeout(time: 30, unit: 'MINUTES')
        buildDiscarder(logRotator(numToKeepStr: '5', artifactNumToKeepStr: '5'))
    }
    stages {
        stage('setting env') {
            agent any
            options {
                skipDefaultCheckout(true)
            }
            steps {
                //配置构建人变量
                wrap([$class: 'BuildUser']) {
                    script {
                        BUILD_USER = "陈昇亨"
                        BUILD_USER_ID ="陈昇亨"
                    }
                }
                script {
                    env.BUILD_USERNAME = "${BUILD_USER}"
                    env.BUILD_USERNAMEID = "${BUILD_USER_ID}"
                }
            }
        }
    }

post {
        success {
            script {
                sh 'export TYPE=success;export JOB_NAME="${JOB_BASE_NAME}";export BUILD_NUM="$BUILD_NUMBER";export BUILD_TIME="$BUILD_TIMESTAMP";export BUILD_USER="${BUILD_USERNAME}"; export URL_JOB="${BUILD_URL}";export URL_LOG="${BUILD_URL}console";export JOB_TIPS1="${BUILD_USERNAMEID}" ;sh send_message-export.sh'
            }
        }
        failure {
            script {
                sh 'export TYPE=failure;export JOB_NAME="${JOB_BASE_NAME}";export BUILD_NUM="$BUILD_NUMBER";export BUILD_TIME="$BUILD_TIMESTAMP"; export BUILD_USER="${BUILD_USERNAME}"; export URL_JOB="${BUILD_URL}";export URL_LOG="${BUILD_URL}console";export JOB_TIPS1="${BUILD_USERNAMEID}" ;sh send_message-export.sh'
            }
        }
}
}


