#!/bin/bash
home=$1
devopsServer=$2
devopsPAT=$3
user=$4

echo "DevOps server information passed in. Downloading devops agent and setting up on  $devopsServer"
curl -L https://vstsagentpackage.azureedge.net/agent/2.186.1/vsts-agent-linux-x64-2.186.1.tar.gz -o $home/downloads/vsts-agent-linux-x64.tar.gz
mkdir -p $home/myagent && cd $home/myagent

tar zxvf ../downloads/vsts-agent-linux-x64.tar.gz

echo "Configuring devops agent."
export AGENT_ALLOW_RUNASROOT="1" #http://www.azuredevopsguide.com/must-not-run-with-sudo-issue-on-azuredevops-agent-in-linux-machines/
$home/myagent/config.sh  --unattended --url $devopsServer --auth pat --token $devopsPAT --pool Default --agent $devopsAgentName --replace --work $home/myagent/_work --acceptTeeEula 

echo "Configured devops agent. Installing service"
$home/myagent/svc.sh install $user

echo "Starting devops agent"
$home/myagent/svc.sh start
echo "Installed and started devops agent service"

