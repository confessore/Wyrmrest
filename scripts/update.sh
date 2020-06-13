#!/bin/sh

sudo service wyrmrest.web stop
sudo service wyrmrest.discord stop

cd /home/$USER/wyrmrest
sudo git pull origin master

cd /home/$USER/wyrmrest/src/Wyrmrest.Web
sudo dotnet publish -c Release -o /var/dotnetcore/Wyrmrest.Web

cd /home/$USER/wyrmrest/src/Wyrmrest.Discord
sudo dotnet publish -c Release -o /var/dotnetcore/Wyrmrest.Discord

sudo service wyrmrest.web start
sudo service wyrmrest.discord start
