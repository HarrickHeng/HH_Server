#!/bin/sh
#替换成自己的群机器人key值
CHAT_WEBHOOK_KEY="b36f73bb-91e0-4180-9fae-a19741fdec74"
CHAT_CONTENT_TYPE='Content-Type: application/json'
#-o代表或的意思，成功或失败 CHAT_WEBHOOK_URL都是一样的WEBHOOK url
if [ _"${TYPE}" = _"success" -o  _"${TYPE}" = _"failure" ]; then
CHAT_WEBHOOK_URL='https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key'
fi

if [ _"${CHAT_WEBHOOK_KEY}" = _"" ]; then
echo "please make sure CHAT_WEBHOOK_KEY has been exported as environment variable"

fi
echo "## send message for : ${TYPE}"
if [ _"${TYPE}" = _"success" ]; then
curl "${CHAT_WEBHOOK_URL}=${CHAT_WEBHOOK_KEY}" \
-H "${CHAT_CONTENT_TYPE}" \
-d '
   {
        "msgtype": "markdown",
        "markdown": {
         "content": "<font color=\"warning\">**Jenkins任务通知**</font> \n
         >任务名称：<font color=\"comment\">'"${JOB_NAME}"'</font>
         >构建时间：<font color=\"comment\">'"${BUILD_TIME}"'</font>
         >任务地址：<font color=\"comment\">[点击查看]('"${URL_JOB}"')</font>
         >构建日志：<font color=\"comment\">[点击查看]('"${URL_LOG}"')</font>
         >构建状态：<font color=\"info\">**Success**</font> \n
         >任务已构建完成请确认：<@所有人>"
         
        }
   }
'
elif [ _"${TYPE}" = _"failure" ]; then
curl "${CHAT_WEBHOOK_URL}=${CHAT_WEBHOOK_KEY}" \
-H "${CHAT_CONTENT_TYPE}" \
-d '
   {
        "msgtype": "markdown",
        "markdown": {
         "content": "<font color=\"warning\">**Jenkins任务通知**</font> \n
         >任务名称：<font color=\"comment\">'"${JOB_NAME}"'</font>
         >构建时间：<font color=\"comment\">'"${BUILD_TIME}"'</font>
         >任务地址：<font color=\"comment\">[点击查看]('"${URL_JOB}"')</font>
         >构建日志：<font color=\"comment\">[点击查看]('"${URL_LOG}"')</font>
         >构建状态：<font color=\"red\">**Failure**</font>
         >任务已构建完成请确认：<@所有人>"
         
        }
   }
'
fi