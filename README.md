# Ark-Server-Creation-Tool

Ark Survival Ascended Server Creation tool - Want to host your own server rather than buy one from Nitrado?

If you have any questions join the discord https://discord.gg/RspMUPqjaJ 

This tool will automate the downloading, installation and setup of your very own ASA server so you can host your own ASA server, just punch in the info it asks for and off you go.
The tool right now is a very barebones application but is getting updated constantly to improve it

Requirements:
 - .NET 9

# Instructions on using this tool

You will need a windows PC / Dedicated Server / VPS / VDS with a minimum of 15GB ram (preferably 20GB so you can build some bases & have a few players)
A blank ARK SA server will take 10.6GB of ram so you need some space for people to build stuff

The tool (Ark Survival Ascended Server Creation tool) will grab the ASA server files and place them into C:\ark_sa

1. Download the tool using the latest zip https://github.com/Ragonz/Ark-Server-Creation-Tool/releases/latest
2. Extract the tool to a desired location. It's recommended that you keep this in its own folder for simplicity. If you have an existing installation of ASCT, you can replace that version with the new one.
   ![Screenshot 2024-04-26 190818](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/f38390b8-1051-4c26-946f-65dea0431d85)
3. Launch `ARK Server Creation Tool.exe` to open the tool.
4. On first launch, you'll be presented with a settings window. If you need to run ASA on a different drive from the tool, you can change the default paths now. If you're happy with everything on the same drive, the default paths should work fine for you. It's recommended you leave the port settings as their defaults.
   ![Screenshot 2024-04-26 190900](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/2382cda6-68d7-411a-aaff-cc5769036eb5)

   Press Save to continue.
5. The server list page will then open. If this is a fresh install, no servers will be present. If you upgraded an existing ASCT install, the existing server should be detected and displayed.
   ![Screenshot 2024-04-26 190917](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/0dfb2d6b-8653-40f2-8e96-c88f38de1f0b)

6. To create a server, press the + button.
7. The server configuration window will open. On this window, you can give your server a name and tell the tool where to install it. The name is only used within the tool for identification purposes.
   ![Screenshot 2024-04-26 190927](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/6ed24fc2-64ad-4953-af48-d4fe7242a153)
   
   Click Save to create the new server.
8. The server updater will now open. This is how the game files are downladed. Ensure the newly created server is selected in the list and then clickt he "Update Game Files" button.
    ![Screenshot 2024-04-26 190942](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/75ee4c1c-efe9-4259-9ba3-7366ba9cdc2b)
9. If this is a new installation of the tool and no DepotDownloader is detected, the tool will ask if you'd like to download it. Click Yes.
    ![Screenshot 2024-04-26 190948](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/cb28f042-fd90-4848-ae7a-c91d2d99797f)
10. Once DepotDownloader has been downloaded and extracted, the tool will launch it to download the files to the correct location
    ![Screenshot 2024-04-26 191010](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/6f7891ad-9d8b-4cd3-b432-cdfb5711c4cc)
11. The Depotdownloader console will close and the tool will confirm the update is complete. You can now close the updater window.
    ![Screenshot 2024-04-26 191717](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/f6105e27-d40d-4833-b3e9-166737209ada)
12. The server will now display in the server list window. Select it in the list and click "View Server"
    ![Screenshot 2024-04-26 191723](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/30ebd363-f5ac-4274-bf55-51e8dea4dff3)
13. You will now be presented with the server view window. This gives you the option to launch the server, access the config within ASCT or access the main game config files. You can press "Start Server" to launch the game server.
    ![Screenshot 2024-04-26 192051](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/4c62cd1d-a3b7-40ac-a21b-ecbcd0c41e9d)
14. After clicking Start Server, the tool will run the game server and if required, create a firewall rule for you. Once the game has fully started, you should be able to join it in game.
    ![Screenshot 2024-04-26 192103](https://github.com/Ragonz/Ark-Server-Creation-Tool/assets/12957193/191f77e9-786d-41b0-ac7d-8edc64caae14)

If the tool helped you and you want to help us fund its development, consider subscribing to our Patreon
https://www.patreon.com/Ragonz

People keep asking us if port forwarding is required, yes unless you want to only play on your server within your own home.
If you cannot be bothered to port forward (or don't know how (there are many guides out there)) rent a vps as then you don't have to. No we don't have any reccomendations on what to use, however we have previously used https://pingperfect.com/ and it worked fine.
If you do go the route of a VPS make sure it has a decent cpu speed and enough ram (approx 10GB per map)
