#!/bin/sh

sudo systemctl stop wyrmrest.web.service
sudo systemctl stop wyrmrest.discord.service

sudo systemctl disable wyrmrest.web.service
sudo systemctl disable wyrmrest.discord.service

read -p "Database Host: " databaseHost
read -p "Database Port: " databasePort
read -p "Database User: " databaseUser
read -p "Database Pass: " databasePass
read -p "Smtp Host: " smtpHost
read -p "Smtp Port: " smtpPort
read -p "Smtp Name: " smtpName
read -p "Smtp Email: " smtpEmail
read -p "Smtp User: " smtpUser
read -p "Smtp Pass: " smtpPass

sudo cp ./services/wyrmrest.web.service ./services/wyrmrest.web.service.backup
sudo cp ./services/wyrmrest.discord.service ./services/wyrmrest.discord.service.backup

sudo sed -i '/DatabaseHost=/s/$/'"$databaseHost"'/' ./services/wyrmrest.web.service.backup
sudo sed -i '/DatabasePort=/s/$/'"$databasePort"'/' ./services/wyrmrest.web.service.backup
sudo sed -i '/DatabaseUser=/s/$/'"$databaseUser"'/' ./services/wyrmrest.web.service.backup
sudo sed -i '/DatabasePass=/s/$/'"$databasePass"'/' ./services/wyrmrest.web.service.backup
sudo sed -i '/SmtpHost=/s/$/'"$smtpHost"'/' ./services/wyrmrest.web.service.backup
sudo sed -i '/SmtpPort=/s/$/'"$smtpPort"'/' ./services/wyrmrest.web.service.backup
sudo sed -i '/SmtpName=/s/$/'"$smtpName"'/' ./services/wyrmrest.web.service.backup
sudo sed -i '/SmtpEmail=/s/$/'"$smtpEmail"'/' ./services/wyrmrest.web.service.backup
sudo sed -i '/SmtpUser=/s/$/'"$smtpUser"'/' ./services/wyrmrest.web.service.backup
sudo sed -i '/SmtpPass=/s/$/'"$smtpPass"'/' ./services/wyrmrest.web.service.backup

sudo mv ./services/wyrmrest.web.service.backup /etc/systemd/system/wyrmrest.web.service
sudo mv ./services/wyrmrest.discord.service.backup /etc/systemd/system/wyrmrest.discord.service

sudo systemctl enable wyrmrest.web.service
sudo systemctl enable wyrmrest.discord.service

sudo systemctl start wyrmrest.web.service
sudo systemctl start wyrmrest.discord.service
