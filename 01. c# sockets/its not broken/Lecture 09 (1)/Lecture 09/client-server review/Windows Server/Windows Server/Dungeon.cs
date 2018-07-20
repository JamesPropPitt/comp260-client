using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;


using System.Data.SQLite;


namespace SUD
{
    public class Dungeon
    {        
        public Dictionary<String, Room> roomMap;
        public Dictionary<Socket, Room> socketToRoomLookup;

        SQLiteConnection conn = null;

        string databaseName = "data.database";
        
        void AddRoomToDungeon(String roomName, String description, String north, String south, String east, String west)
        {
            
            try
            {
                var sql = "insert into " + "table_rooms" + " (name, desc) values ";
                sql += "('" + roomName + "'";
                sql += ",";
                sql += "'" + description + "'";
                sql += ")";

                var command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to add: " + roomName + " : " + description + " to DB " + ex);
            }
        }

        public void Init()
        {

            try
            {
                SQLiteConnection.CreateFile(databaseName);

                conn = new SQLiteConnection("Data Source=" + databaseName + ";Version=3;FailIfMissing=True");

                SQLiteCommand command;

                conn.Open();

                command = new SQLiteCommand("create table table_rooms (name varchar(20), desc varchar(20))", conn);
                command.ExecuteNonQuery();

                //command = new SQLiteCommand("drop table table_phonenumbers", conn);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Create DB failed: " + ex);
            }

            AddRoomToDungeon("room 0", "room 0 desc","room 1","","","");
            AddRoomToDungeon("room 1", "room 1 desc","","room 0","","");

            try
            {
                Console.WriteLine("");
                SQLiteCommand command = new SQLiteCommand("select * from " + "table_rooms" + " order by name asc", conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("Name: " + reader["name"] + ": " + reader["desc"]);
                }

                reader.Close();
                Console.WriteLine("");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to display DB");
            }


            socketToRoomLookup = new Dictionary<Socket, Room>();
#if false
            roomMap = new Dictionary<string, Room>();

            {
                var room = new Room("Room 0", "You are standing in the entrance hall\nAll adventures start here");
                room.north = "Room 1";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 1", "You are in room 1");
                room.south = "Room 0";
                room.west = "Room 3";
                room.east = "Room 2";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 2", "You are in room 2");
                room.north = "Room 4";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 3", "You are in room 3");
                room.east = "Room 1";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 4", "You are in room 4");
                room.south = "Room 2";
                room.west = "Room 5";
                roomMap.Add(room.name, room);
            }

            {
                var room = new Room("Room 5", "You are in room 5");
                room.south = "Room 1";
                room.east = "Room 4";
                roomMap.Add(room.name, room);
            }  
#endif
        }
        
        public void SetClientInRoom(Socket client, String room)
        {
            if (socketToRoomLookup.ContainsKey(client) == false)
            {
                //socketToRoomLookup[client] = roomMap[room];
            }
        }

        public void RemoveClient(Socket client)
        {
            if (socketToRoomLookup.ContainsKey(client) == true)
            {
                socketToRoomLookup.Remove(client);
            }
        }

        public String RoomDescription(Socket client)
        {
            String desc = "";

#if false

            desc += socketToRoomLookup[client].desc + "\n";
            desc += "Exits" + "\n";
            for (var i = 0; i < socketToRoomLookup[client].exits.Length; i++)
            {
                if (socketToRoomLookup[client].exits[i] != null)
                {
                    desc += Room.exitNames[i] + " ";
                }
            }

            var players = 0;

            foreach(var kvp in socketToRoomLookup)
            {
                if( (kvp.Key != client)
                  &&(kvp.Value == socketToRoomLookup[client])
                  )
                {
                    players++;
                }
            }

            if(players > 0)
            {
                desc += "\n";
                desc += "There are " + players + " other dungeoneers in this room";
            }

            desc += "\n";

            return desc;
#endif

            return "";
        }
    }
}
