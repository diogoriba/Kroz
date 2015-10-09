using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using Assets.Server.GameObjects;
using Assets.Server;

public class Server : MonoBehaviour
{
    private Rect window;
    private LogWindow logWindow;

    public LogWindow LogWindow
    {
        get { return logWindow; }
        set { logWindow = value; }
    }
    private List<Room> map;

    public List<Room> Map
    {
        get { return map; }
        set { map = value; }
    }
    public static Server Instance { get; private set; }
    public Player ServerPlayer { get; private set; }
    public void Send(Player source, Player destination, string message, bool echo = true)
    {
        networkView.RPC("ApplyGlobalChatText", destination.NetworkPlayer, source.Name, message);
        if (echo && source != ServerPlayer)
        {
            networkView.RPC("ApplyGlobalChatText", source.NetworkPlayer, source.Name, message);
        }
    }

    public void Send(string destination, string message, Player source, bool echo = true)
    {
        Player destPlayer = GetPlayerNode(destination);
        if (destPlayer != null)
        {
            Send(source, destPlayer, message, echo);
        }
        else
        {
            source.Talk(ServerPlayer, "Não há ninguém chamado '" + destination + "' aqui");
        }
    }

    void Awake()
    {
        logWindow = GetComponent<LogWindow>();
        Instance = this;
        ServerPlayer = new Player("DM", string.Empty);
        ServerPlayer.NetworkPlayer = new NetworkPlayer();
        Room room1 = new Room("Depois de uma longa caminhada da cidade de Valen’var, você se encontra na entrada da “caverna dos esquecidos”. Uma entrada de 3 metros que se estende a té onde os olhos conseguem enxergar. O vento frio da região montanhosa sopra para dentro das profundezas da caverna. O que você pretende fazer?");
        room1.DetailedDescription = "Descrição detalhada";
        Room room2 = new Room("O caminho é úmido e escorregadio, você caminha com cautela, deixando frio gélido para traz.Depois de uma decida de alguns metros, você se encontra em um ambiente aberto, como uma pequena sala escava na pedra, ela é iluminada por duas tochas que se encontra em paredes oposta. O que pretende fazer?");
        Room room3 = new Room("Você se encontra em uma sala ampla e bem iluminada, ao centro há uma grande mesa e cadeiras. Ao norte se vê uma parede onde com um acesso para outro aposento. Um cheiro de cozido pode ser sentido no ambiente. O que pretende fazer?");
        Room room4 = new Room("Você entra em uma sala não muito ampla, com panelas e despensas espalhados pelo local. Era aqui que se preparava as refeições que eram servidas no aposento anterior. O que pretende fazer?");
        Room room5 = new Room("O corredor é apenas iluminado por um brilho sibilante que sai de uma porta do lado oposto.O que pretende fazer?");
        Room room6 = new Room("Você entra em um aposento maior que o anterior, bem iluminado e com alguns beliches. Este deveria ser o local de descanso de quem quer que estivesse aqui. As camas estão arrumadas e algumas ferramentas de maneiração estão encostados na parede ao sul.");
        Room room7 = new Room("Você caminha apelo corredor com cuidado, até alcançar o caminho que vira a leste. Ao olhar para o corredor à frente, você vê uma escadaria íngreme que leva para baixo, para as profundezas da mina. A iluminação e fraca e não é possível ver até onde ela chega.");
        Room room8 = new Room("Deixando o corredor para trás, você se encontra em uma sala ampla escavada nas sólidas pedras da montanha. O local nos está iluminado, e com o aduz da sua tocha você vê que há alguma espécie de maquinário e equipamentos diferente dos que tu já viu, provavelmente eram usados para separa os mineiros que estariam das profundezas da montanha.");
        Room room9 = new Room("Iluminando este aposento, você percebe que ele é pequeno e está cheio de equipamentos para maneiração, como casacos, picaretas, pás e lanternas.");
        Room room10 = new Room("Passando pela porta secreta, você se encontra em um corredor escuro e estreito. Ao final dele se vê ‘trés jarros contendo água em diferentes níveis/ uma porta improvisada de madeira’.");
        Room room11 = new Room("Passando pela porta quebras você tem acesso a uma pequena sala. Logo dá para ver que nela nos há mais nada, for dois baús de tamanho médio de metal que está ali o que pretende fazer?");
        Room room12 = new Room("Deixando as riquezas e aventura que explorar esta mina poderia propiciar, você decide que não vale o risco de se aventurar em um ambiente desconhecido e claustrofóbico e resolve voltar para cidade. Talvez deixando o mundo de aventurar para outros intrépetos que aceite essa riqueza e fama. E você vive uma vida pacifica e tranquila em uma fazenda ao sul.");


        room1.Neighbors.Add("norte",  room2);
        room1.Neighbors.Add("sul",    room12);
        room2.Neighbors.Add("sul",    room1);
        room2.Neighbors.Add("oeste",  room3);
        room2.Neighbors.Add("leste",  room5);
        room3.Neighbors.Add("leste",  room2);
        room3.Neighbors.Add("norte",  room4);
        room4.Neighbors.Add("sul",    room3);
        room5.Neighbors.Add("leste",  room6);
        room5.Neighbors.Add("oeste",  room2);
        room6.Neighbors.Add("norte",  room9);
        room6.Neighbors.Add("sul",    room7);
        room6.Neighbors.Add("oeste",  room5);
        room7.Neighbors.Add("leste",  room8);
        room7.Neighbors.Add("norte",  room6);
        room8.Neighbors.Add("norte",  room10);
        room8.Neighbors.Add("oeste",  room7);
        room9.Neighbors.Add("sul",    room6);
        room10.Neighbors.Add("norte", room11);
        room10.Neighbors.Add("sul",   room8);
        room11.Neighbors.Add("sul",   room10);

        map = new List<Room>();
        map.Add(room1);
        map.Add(room2);
        map.Add(room3);
        map.Add(room4);
        map.Add(room5);
        map.Add(room6);
        map.Add(room7);
        map.Add(room8);
        map.Add(room9);
        map.Add(room10);
        map.Add(room11);
        map.Add(room12);

        Item maca = new GenericItem("maçã", "teste");
        room1.Items.Add(maca);
        Key key = new Key("chave", "uma chave prateada bem simples");
        room1.Items.Add(key);
        Door door = new Door("", "");
        room1.Doors.Add("norte", door);
        room2.Doors.Add("sul", door);
        Map mapItem = new Map();
        room1.Items.Add(mapItem);
    }

    public Item Find(Player player, string itemName)
    {
        if (!string.IsNullOrEmpty(itemName))
        {
            // Player
            Player targetPlayer = GetPlayerNode(itemName);
            if (targetPlayer != null)
            {
                return targetPlayer;
            }

            // Room
            if (itemName.Equals("sala"))
            {
                return player.Room;
            }

            // Room items
            Item roomItem = player.Room.Find(itemName);
            if (roomItem != null)
            {
                return roomItem;
            }

            // Inventory
            Item playerItem = player.Find(itemName);
            if (playerItem != null)
            {
                return playerItem;
            }
        }

        return null;
    }

    [RPC]
    void ApplyGlobalChatText(string name, string command)
    {
        if (Network.connections.Length > 0)
        {
            Player author = GetPlayerNode(name);
            if (author != null)
            {
                Command cmd = Parse(command, author);
                switch (cmd.Verb)
                {
                    case "inventário":
                    case "inventario":
                        author.Inventory(author);
                        break;
                    case "ir":
                        author.Room.Parse(cmd, author);
                        break;
                    case "pegar":
                    case "largar":
                    case "examinar":
                    case "usar":
                    case "falar":
                        if (cmd.Target != null)
                        {
                            cmd.Target.Parse(cmd, author);
                        }
                        else
                        {
                            author.Talk(global::Server.Instance.ServerPlayer, string.Format("Do que você está falando? Não há nada chamado {0}", cmd.Tail.FirstOrDefault()));
                        }
                        break;
                    default:
                        author.Talk(global::Server.Instance.ServerPlayer, "Comando inválido");
                        break;
                }
            }
        }
    }

    private Command Parse(string command, Player author)
    {
        command = command.ToLower();
        string[] parsedCommand = command.Split(' ');
        string verb = parsedCommand[0];
        string target = parsedCommand.ElementAtOrDefault(1) ?? "";
        Item targetItem = Find(author, target);
        string[] tail;
        if ((targetItem != null) && (!verb.Equals("ir")))
        {
            tail = parsedCommand.Skip(2).ToArray();
        }
        else
        {
            tail = parsedCommand.Skip(1).ToArray();
        }

        return new Command(verb, targetItem, tail);
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
            if (entry.Name.ToLower() == playerName.ToLower())
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
        Player newEntry = new Player(name);
        newEntry.NetworkPlayer = info.sender;
        playerList.Add(newEntry);
        newEntry.Room = map.First();
        newEntry.Room.Describe(newEntry); // initial description

        logWindow.Log(name + " joined the chat");
    }
    #endregion
}