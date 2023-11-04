# Ark-Server-Creation-Tool

Ark Survival Ascended Server Creation tool - Want to host your own server rather than buy one from Nitrado?

If you have any questions join the discord https://discord.gg/RspMUPqjaJ 

This tool will automate the downloading, installation and setup of your very own ASA server so you can host your own ASA server, just punch in the info it asks for and off you go.
The tool right now is a very barebones application but is getting updated constantly to improve it

Instructions on using this tool

You will need a windows PC / Dedicated Server / VPS / VDS with a minimum of 15GB ram (preferably 20GB so you can build some bases & have a few players)
A blank ARK SA server will take 10.6GB of ram so you need some space for people to build stuff

The tool (Ark Survival Ascended Server Creation tool) will grab the ASA server files and place them into C:\ark_sa

1. Download the tool using the latest exe https://github.com/Ragonz/Ark-Server-Creation-Tool/releases
![Screenshot 2023-11-04 17 06 08](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/a6275c23-41c2-42b1-aeeb-82d7fa923940)

2. Open it on your pc (If you get a "do you want this app from an unknown publisher to make changes to your device" popup click Yes)
![Screenshot 2023-11-04 17 06 17](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/2612981c-c234-420c-b471-a48a5b0dfa07)

3. After opening the tool you will be presented with the first time config, make any changes then click save config
![Screenshot 2023-11-04 17 06 26](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/baef9b6c-11b7-44e2-9a45-3ca1c0556cc5)


The changes you can make currently are location in which to install the server "Game Folder"
Automatically creating windows firewall rules yes/no
Setting your own commandline by ticking the override launch arguments then entering in what you want in the box below (DO NOT DO THIS UNLESS YOU KNOW WHAT YOU ARE DOING)
Changing the game port (YOU WILL NEED TO DO THIS TO HOST MULTIPLE SERVERS, 1st server will be on 7777, 2nd server should be on 7787 and so on)
Setting a specific IP address to bind the server to (DO NOT TOUCH UNLESS YOU KNOW WHAT YOU ARE DOING)

4. After clicking save config you will get this window, click "Update Game Files" to download the server to the location you specified in step 3
![Screenshot 2023-11-04 17 08 11](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/03311544-9bd1-4e4f-9050-04a4307b7205)
![Screenshot 2023-11-04 17 08 20](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/42ce7d2f-35ba-4dfa-b1ed-14b1379f5be2)
Wait until the 2nd window goes away as this is what is downloading the server files

5.  After the 2nd window has gone away click "Continue" and you will get the final window where you can change the tool config "Open Configuration", start the server, update the server and open the config files for the server so you can make changes to it
![Screenshot 2023-11-04 17 18 11](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/213d5928-0e63-46fe-a284-751702c12be5)

6. Click "Open Game Config" then click "Yes" to creating the config files
![Screenshot 2023-11-04 17 22 17](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/764b2c41-240b-42d3-abaa-01dbebde376d)

7. This will open the server settings file where you can change a lot of the settings for the server, you will need to make at least 1 adjustment in here which is setting your servername, you can find it under the [ServerSettings] heading, add your servername next to SessionName= (for example SessionName=Ragonz)
![Screenshot 2023-11-04 17 24 37](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/379ca03b-9af7-422b-9b0f-bafe95bcfddb)
After you have edited this file close it & SAVE IT

8. Click "Start Server" After about a minute or so the server will be ready to play on.
![Screenshot 2023-11-04 17 29 41](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/cd67f201-6403-4cec-a28b-e18dbb035c0b)

If the tool helped you and you want to help us fund its development, consider subscribing to our Patreon
https://patreon.com/Ragonz?utm_medium=clipboard_copy&utm_source=copyLink&utm_campaign=creatorshare_creator&utm_content=join_link
