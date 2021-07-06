#!/bin/bash

echo "Configuring Jumpbox"

echo "Updating packages ..."
apt update
apt upgrade -y

echo "Installing Azure CLI"
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash


