using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Utils;

namespace Lambda
{
    public class Game
    {
        private List<Player> players;
        private List<Vehicle> vehicles;
        private List<Area> areas;
        private List<Spawn> spawns;
        private List<Command> commands;
        private BaseItem[] baseitems;
        private DBConnect dbConnect;
        private Events events;

        public Chat Chat;
        public DbVehicle DbVehicle { get; }
        public DbBaseItem DbBaseItem { get; }
        public DbArea DbArea { get; }
        public DbPlayer DbPlayer { get; }
        public DbAccount DbAccount { get; }

        public Game()
        {
            players = new List<Player>();
            vehicles = new List<Vehicle>();
            areas = new List<Area>();
            spawns = new List<Spawn>();
            commands = new List<Command>();
            baseitems = new BaseItem[0];
            dbConnect = new DBConnect();
            events = new Events(this);


            Chat = new Chat();
            DbVehicle = new DbVehicle(this, dbConnect, "t_vehicle_veh", "veh");
            DbBaseItem = new DbBaseItem(this, dbConnect, "t_itemdata_itd", "itd");
            DbArea = new DbArea(this, dbConnect, "t_area_are", "are");
            DbPlayer = new DbPlayer(this, dbConnect, "t_character_cha", "cha");
            DbAccount = new DbAccount(this, dbConnect, "t_account_acc", "acc");

        }

        public void Init()
        {
            Alt.Log("=== Register events... ===");
            events.RegisterEvents();
            Alt.Log(">Server events registered");
            Chat.RegisterEvents();
            Alt.Log(">Chat registered");
            Alt.Log("=== Events are registered ===");
            Alt.Log("=== Register Spawns... ===");
            AddSpawn(new Spawn(new Position(-1023.89f, -487.7407f, 36.96338f)));
            Alt.Log("=== Spawns added... ===");
            Alt.Log("=== Register Commands... ===");
            AddAllCommands();
            Alt.Log("=== Commands are registered ===");
            Alt.Log("=== Load in database... ===");
            Alt.Log(DbVehicle.ToString());
            AddAllVehicles();
            Alt.Log(">Vehicles spawned");
            AddAllBaseItems();
            Alt.Log(">Base items created");

        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public Player[] GetPlayers()
        {
            return players.ToArray();
        }

        public Player[] GetPlayers(string nameOrId)
        {
            List<Player> players = new List<Player>();
            foreach (Player player in this.players)
            {
                if (player.ServerId.ToString().Equals(nameOrId)) players.Add(player);
                else if (player.Name.StartsWith(nameOrId)) players.Add(player);
            }
            return players.ToArray();
        }

        public void RemovePlayer(Player player)
        {
            players.Remove(player);
        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
            vehicle.Game = this;
            vehicle.Spawn();
        }

        public void AddAllVehicles()
        {
            vehicles = DbVehicle.GetAll().ToList();
            vehicles.ForEach((veh) =>
            {
                veh.Spawn();
            });
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            vehicles.Remove(vehicle);
            DbVehicle.Delete(vehicle);
            vehicle.AltVehicle.Remove();
        }

        public void AddAllBaseItems()
        {
            baseitems = DbBaseItem.GetAll();
        }

        public BaseItem GetBaseItem(uint id)
        {
            return baseitems.FirstOrDefault(baseitem => baseitem.Id == id);
        }

        public Area GetArea(Position position)
        {
            foreach (Area area in areas)
            {
                if (area.Position.Distance(position) < area.Radius)
                {
                    return area;
                }
            }

            return null;
        }
        public Area GetArea(Position position, Area.AreaType type)
        {
            foreach (Area area in areas)
            {
                if (area.Type != type) continue;
                if (area.Position.Distance(position) < area.Radius)
                {
                    return area;
                }
            }

            return null;
        }

        public void AddArea(Area area)
        {
            areas.Add(area);
        }

        public void AddAllAreas()
        {
            areas = DbArea.GetAll().ToList();
        }

        public void RemoveArea(Area area)
        {
            areas.Remove(area);
            DbArea.Delete(area);
        }

        public void AddSpawn(Spawn spawn)
        {
            spawns.Add(spawn);
        }

        public Spawn GetSpawn(int id)
        {
            if (id < spawns.Count)
            {
                return spawns[id];
            }
            else
            {
                return null;
            }
        }

        public void RemoveSpawn(Spawn spawn)
        {
            spawns.Remove(spawn);
        }

        public void AddCommand(Command command)
        {
            commands.Add(command);
            Alt.Log($"[COMMAND]{command.Name} registered");
        }

        public void AddAllCommands(string namespaceName = "", Assembly assembly = null)
        {

            if (namespaceName.Length == 0) namespaceName = typeof(Command).Namespace; //If no namespace is specified
            if (assembly == null) assembly = Assembly.GetExecutingAssembly();
            Type[] types = GetTypesInNamespace(assembly, namespaceName); // Get all classes in this namespace
            Alt.Log($"Nombre de types dans le namespace {namespaceName}: " + types.Length);
            foreach (Type type in types)
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.GetCustomAttributes(typeof(CommandAttribute), false).Length <= 0) continue;
                    if (method.GetParameters()[0].ParameterType == typeof(Player) && method.GetParameters()[1].ParameterType == typeof(string[]))
                    {
                        Command.CommandFunc function = (Command.CommandFunc)Delegate.CreateDelegate(typeof(Command.CommandFunc), method, false); // Create the delegate of my méthod
                        if (function == null) Alt.Log("FUNCTION NULL" + method.Name);
                        CommandAttribute attribute = (CommandAttribute)method.GetCustomAttributes(typeof(CommandAttribute), false)[0];
                        Command command = new Command(method.Name.ToLower(), function, attribute.Type, attribute.Syntax);
                        AddCommand(command); // Register the command
                    }
                }
            }
        }

        public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }

        public Command[] GetCommands()
        {
            return commands.ToArray();
        }

        public Command[] GetCommands(string[] parameters)
        {
            List<Command> cmds = new List<Command>(commands);
            int i = 0;
            while (cmds.Count > 1 && i < parameters.Length) // While good commands is too high
            {
                for (int j = 0; j < cmds.Count; j++)
                {
                    Command command = cmds[j];
                    string[] commandText = command.Name.Split("_");// vehicule_park => vehicule park
                    if (commandText.Length > i) // If command have good size for test
                    {


                        if (!commandText[i].StartsWith(parameters[i]))
                        {
                            j--;
                            cmds.Remove(command);
                        }
                    }
                }

                if (cmds.Count <= 1) break;
                bool sameLength = true;
                for (int j = 1; j < cmds.Count; j++)
                {
                    Command command = cmds[j];
                    Command command2 = cmds[j - 1];
                    string[] commandText = command.Name.Split("_");// vehicule_park => vehicule park
                    string[] commandText2 = command2.Name.Split("_");// vehicule_park => vehicule park
                    if (commandText2.Length != commandText.Length)
                    {
                        sameLength = false;
                        break;
                    }
                }

                if (sameLength && i >= cmds[0].Name.Split("_").Length) break;
                for (int j = 0; j < cmds.Count; j++)
                {
                    Command command = cmds[j];
                    string[] commandText = command.Name.Split("_");// vehicule_park => vehicule park
                    if (commandText.Length <= i)
                    {
                        j--;
                        cmds.Remove(command);
                    }
                }
                i++;
            }

            return cmds.ToArray();
        }


    }
}
