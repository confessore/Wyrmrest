#!/bin/sh

sudo systemctl stop wyrmrest.web.service
sudo systemctl stop wyrmrest.discord.service

sudo systemctl disable wyrmrest.web.service
sudo systemctl disable wyrmrest.discord.service
