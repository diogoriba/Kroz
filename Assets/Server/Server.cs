using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Assets.Server.GameObjects;

public class Server : MonoBehaviour
{
    private Rect window;
    private LogWindow logWindow;
    private List<Room> map;
    public static NetworkView NetworkView { get; set; }

    void Awake()
    {
        logWindow = GetComponent<LogWindow>();
        Server.NetworkView = networkView;

        Room room1 = new Room("sala inicial");
        Room room2 = new Room("cooredor");
        Room room3 = new Room("sala esquerda");
        Room room4 = new Room("sala direita");
        room1.neighbors.Add("norte", room2);
        room2.neighbors.Add("sul", room1);
        room2.neighbors.Add("oeste", room3);
        room3.neighbors.Add("leste", room2);
        room2.neighbors.Add("leste", room4);
        room4.neighbors.Add("oeste", room2);

        map = new List<Room>();
        map.Add(room1);
        map.Add(room2);
        map.Add(room3);
        map.Add(room4);
    }

    [RPC]
    void ApplyGlobalChatText(string name, string command)
    {
        if (Network.connections.Length > 0)
        {
            command = command.ToLower();
            string[] parsedCommand = command.Split(' ');
            string verb = parsedCommand[0];
            string target = parsedCommand[1];
            string[] tail = parsedCommand.Skip(2).ToArray();
            Player author = GetPlayerNode(name);
            switch (verb)
            {
                case "talk":
                case "falar":
                    string message = String.Join(" ", tail);
                    logWindow.Log("[talk] " + name + " -> " + target + ": " + message);
                    Player targetPlayer = GetPlayerNode(target);
                    if (target == null)
                    {
                        author.Send("SERVER", "Não há ninguém chamado '" + target + "' aqui");
                    }
                    else
                    {
                        targetPlayer.Send(name, message);
                        author.Send(name, message); // echo
                    }
                    break;
                case "go":
                case "ir":
                    author.Room.Go(target, author);
                    break;
                default:
                    author.Send("SERVER", "Comando inválido");
                    break;
            }
        }
    }

    #region server
    //Server-only playerlist
    private List<Player> playerList = new List<Player>();

    //A handy wrapper function to get the PlayerNode by networkplayer
    Player GetPlayerNode(NetworkPlayer networkPlayer)
    {
        foreach (Player entry in playerList)
        {
            if (entry.NetworkPlayer == networkPlayer)
            {
                return entry;
            }
        }
        Debug.LogError("GetPlayerNode: Requested a playernode of non-existing player!");
        return null;
    }

    Player GetPlayerNode(string playerName)
    {
        foreach (Player entry in playerList)
        {
            if (entry.PlayerName.ToLower() == playerName.ToLower())
            {
                return entry;
            }
        }
        return null;
    }

    //Server function
    void OnPlayerDisconnected(NetworkPlayer player)
    {
        logWindow.Log("Player disconnected from: " + player.ipAddress + ":" + player.port);

        //Remove player from the server list
        playerList.Remove(GetPlayerNode(player));
    }

    //Server function
    void OnPlayerConnected(NetworkPlayer player)
    {
        logWindow.Log("Player connected from: " + player.ipAddress + ":" + player.port);
    }

    [RPC]
    //Sent by newly connected clients, recieved by server
    void TellServerOurName(string name, NetworkMessageInfo info)
    {
        Player newEntry = new Player();
        newEntry.PlayerName = name;
        newEntry.NetworkPlayer = info.sender;
        newEntry.Room = map.First();
        playerList.Add(newEntry);

        logWindow.Log(name + " joined the chat");
    }
    #endregion
}