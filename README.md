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

2. Open it on your pc (If you get a "Windows Protected your PC" click "Run Anyway" it will then be followed by a "do you want this app from an unknown publisher to make changes to your device" popup click Yes to this.)
![Screenshot 2023-11-04 17 06 17](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/2612981c-c234-420c-b471-a48a5b0dfa07)
![Screenshot 2023-11-04 17 47 57](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/20845629/792b9f79-e098-41b5-8bd7-7cda521d34ff)

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

# Creating a cluster

The current version of ASCT doesn't directly aid in the creation of clusters. However, it is still possible to manually configure this. This guide demonstates how to create a 2 server cluster.

Step 1: Download ASCT to 2 different folders on the same machine
![Screenshot 2024-04-02 013313](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/9b2ebb17-7b68-4a1e-824b-d36510c394d8)

Step 2: Configure each server, ensuring that you give them unique IP addresses. We'll need to override the launch arguments as cluster launch arguments are not currently handled by the tool. 

You need to add a Cluster ID setting, which is what the game uses to group servers, and thje ClusterDirOverride setting, which tells the game which folder to use to share files between the servers. Both settings must be identical between the 2 servers for the cluster to work properly. 
The cluster ID argument is `-clusterid=`
the cluster dir override argument is `-ClusterDirOverride=`

![Screenshot 2024-04-02 014433](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/c378ad1c-8485-4122-a3c9-9169568fe3e4)

Step 3: Allow the updater to download the files for each server. 
![Screenshot 2024-04-02 013829](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/ee332a0f-8396-4f18-baff-3cdd63a0bae1)

Step 4: Start both servers. No further steps are required for clustering. You can configure each service as you would normally. 
![Screenshot 2024-04-02 014604](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/b2a09fd5-17fa-42a2-8e0a-514fd1f03d44)

If the tool helped you and you want to help us fund its development, consider subscribing to our Patreon
https://www.patreon.com/Ragonz
