## Wyrmrest

# Prerequisites
* dotnetcore3.1
* nginx
* certbot
* mariadb

# Install NGINX
* In `/etc/nginx/sites-available/default`, replace the content with this:
```sh
server {
    listen        80;
    server_name   wyrmrest.com *.wyrmrest.com;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}
```

# Install Certbot
```sh
sudo add-apt-repository ppa:certbot/certbot
sudo apt update
sudo apt-get install python-certbot-nginx
sudo certbot --nginx -d wyrmrest.com -d www.wyrmrest.com
```
* Create a crontab using 
```sh
sudo crontab -e
```
* Add this to the bottom of the file
```sh
0 12 * * * /usr/bin/certbot renew --quiet`
```

# Web
	* **Requires 10 environment variables**
	* DatabaseHost - The server address for mariadb
    * DatabasePort - The server port for mariadb
    * DatabaseUser - The user name for mariadb
    * DatabasePass - The password for mariadb
    * SmtpHost - The server address for smtp
    * SmtpPort - The server port for smtp
    * SmtpName - The name that will appear on an email's 'from'
    * SmtpEmail - The email address that will be used to send emails
    * SmtpUser - The user name for the smtp server
    * SmtpPass - The password for the smtp server

**Setup**
1. Run the `add_services.sh` script
1. Run the `update.sh` script
