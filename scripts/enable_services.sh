#!/bin/sh

sudo systemctl enable wyrmrest.web.service
sudo systemctl enable wyrmrest.discord.service

sudo systemctl start wyrmrest.web.service
sudo systemctl start wyrmrest.discord.service
