# Ark-Server-Creation-Tool

Ark Survival Ascended Server Creation tool - Want to host your own server rather than buy one from Nitrado?

This tool will automate the downloading, installation and setup of your very own ASA server so you can host your own ASA server, just punch in the info it asks for and off you go.
The tool right now is a simple batch script which prompts for information, in the future it will be an application that will manage your server for you and be simple to use.

Instructions on using this tool

You will need a windows PC / Dedicated Server / VPS / VDS with a minimum of 22GB ram (preferably 25GB so you can build some bases & have a few players)

The tool (Ark Survival Ascended Server Creation tool) will grab the ASA server files and place them into C:\ark_sa along with a start_server.bat & an update_server.bat
The tool (Ark Survival Ascended Server Creation tool) will ask you for a few bits of information like servername (what you want the server to show up as in game), server password (password players will need to enter in order to join the server) and so on to create the server start file then go on to start the server for you.

How to use the tool (Ark Survival Ascended Server Creation tool)

1. Download the tool to the machine you want to host the server on, does not matter where you download it to or run it from
2. Right click the Ark_Server_Creation_Tool file and select run as administrator follow the prompts
3. If you are using a home pc you will need to open the ports 7777, 7778, 27015 on your router (If you do not know how to do this you will need to google "How to port forward on [insert router brand and model here]"
4. We open the required ports as part of the installer in your firewall if you are using windows firewall, if you are not using windows firewall you will need to do this manually
5. enjoy

This is version 1.0 of this tool, each new updated of this tool, will come with improvements & soon a ui to control the server with.

Server settings are located in C:\ark_sa\ShooterGame\Saved\Config\WindowsServer\GameUserSettings.ini on the machine you used this tool on.
