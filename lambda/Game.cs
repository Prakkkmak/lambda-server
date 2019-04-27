using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Items;
using Lambda.Commands;
using Lambda.Database;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Organizations;
using Lambda.Utils;
using Player = Lambda.Entity.Player;
using Vehicle = Lambda.Entity.Vehicle;

namespace Lambda
{
    public class Game
    {
        private List<Player> players;
        private List<Vehicle> vehicles;
        private List<Area> areas;
        private List<Spawn> spawns;
        private List<Command> commands;
        //private List<ComponentLink> componentLinks;
        private List<Organization> organizations;
        public List<Link> links;

        private Interior[] interiors;
        private Skin[] skins;
        private BaseItem[] baseitems;
        private DBConnect dbConnect;
        private Events events;
        public Chat Chat;
        public IVoiceChannel VoiceChannel;
        //public Links Links { get; set; }



        public DbVehicle DbVehicle { get; }
        public DbBaseItem DbBaseItem { get; }
        public DbArea DbArea { get; }
        public DbPlayer DbPlayer { get; }
        public DbAccount DbAccount { get; }
        public DbSkin DbSkin { get; }
        public DbOrganization DbOrganization { get; }
        public DbRank DbRank { get; }
        public DbLink DbLink { get; }
        //public DbLink DbComponentLink { get; }
        public DbInterior DbInterior { get; }



        public Game()
        {
            players = new List<Player>();
            vehicles = new List<Vehicle>();
            areas = new List<Area>();
            spawns = new List<Spawn>();
            commands = new List<Command>();
            //componentLinks = new List<ComponentLink>();
            organizations = new List<Organization>();
            links = new List<Link>();
            baseitems = new BaseItem[0];
            dbConnect = new DBConnect();
            events = new Events(this);
            skins = new Skin[0];
            interiors = new Interior[0];
            VoiceChannel = Alt.CreateVoiceChannel(true, 10);

            Chat = new Chat();

            DbVehicle = new DbVehicle(this, dbConnect, "t_vehicle_veh", "veh");
            DbBaseItem = new DbBaseItem(this, dbConnect, "t_itemdata_itd", "itd");
            DbArea = new DbArea(this, dbConnect, "t_area_are", "are");
            DbPlayer = new DbPlayer(this, dbConnect, "t_character_cha", "cha");
            DbAccount = new DbAccount(this, dbConnect, "t_account_acc", "acc");
            //DbComponentLink = new DbLink(this, dbConnect, "t_link_lin", "lin");
            DbSkin = new DbSkin(this, dbConnect, "t_skin_ski", "ski");
            DbInterior = new DbInterior(this, dbConnect, "t_interior_int", "int");
            DbOrganization = new DbOrganization(this, dbConnect, "t_organization_org", "org");
            DbRank = new DbRank(this, dbConnect, "t_rank_ran", "ran");
            DbLink = new DbLink(this, dbConnect, "t_link_lin", "lin");

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
            AddSpawn(new Spawn(new Position(-167.8418f, 921.5604f, 235.6395f)));
            Alt.Log("=== Spawns added... ===");
            Alt.Log("=== Register Commands... ===");
            AddAllCommands();
            Alt.Log("=== Commands are registered ===");
            Alt.Log("=== Load in database... ===");
            AddAllInteriors();
            Alt.Log(">All interiors loaded");
            AddAllBaseItems();
            Alt.Log(">Base items created");
            AddAllVehicles();
            Alt.Log(">Vehicles spawned");
            AddAllAreas();
            Alt.Log(">All areas loaded");
            AddAllOrganizations();
            Alt.Log(">All organizations loaded");
            AddAllLinks();

        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
            VoiceChannel.AddPlayer(player.AltPlayer);
            Alt.EmitAllClients("chatmessage", null, $"{player.AltPlayer.Name} c'est connecté!");
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
            VoiceChannel.RemovePlayer(player.AltPlayer);
            Alt.EmitAllClients("chatmessage", $"{player.AltPlayer.Name} c'est déconnecté!");
        }

        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
            vehicle.Game = this;
            //vehicle.Spawn();
        }

        public void AddAllVehicles()
        {
            vehicles = DbVehicle.GetAll().ToList();
            vehicles.ForEach((veh) => { veh.Game = this; });
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            vehicles.Remove(vehicle);
            if (vehicle.Id != 0) DbVehicle.Delete(vehicle);
            vehicle.AltVehicle.Remove();
            //
        }

        public void AddAllBaseItems()
        {
            baseitems = DbBaseItem.GetAll();
        }

        public BaseItem GetBaseItem(uint id)
        {
            return baseitems.FirstOrDefault(baseitem => baseitem.Id == id);
        }

        public BaseItem[] GetBaseItems()
        {
            return baseitems.ToArray();
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
        public Location GetDestination(Position position, short dimension)
        {
            foreach (Area area in areas)
            {
                if (area.InteriorLocation.Position.Distance(position) < area.Radius && area.Id == dimension)
                {
                    return area.ExteriorLocation;
                }
                if (area.ExteriorLocation.Position.Distance(position) < area.Radius && area.Dimension == dimension)
                {
                    return area.InteriorLocation;
                }
            }

            return default;
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
            areas.ForEach((area) =>
            {
                area.Game = this;
                //area.Spawn(); //TODO
            });
        }

        public void RemoveArea(Area area)
        {
            areas.Remove(area);
            DbArea.Delete(area);
            area.AltCheckpoint.Remove();
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
                        PermissionAttribute[] permissionAttributes = (PermissionAttribute[])method.GetCustomAttributes(typeof(PermissionAttribute), false);
                        string permission = "";
                        if (permissionAttributes.Length > 0) permission = permissionAttributes[0].Permission;
                        Command command = new Command(method.Name.ToLower(), function, attribute.Type, attribute.Syntax, permission);
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




        public Skin[] GetSkins()
        {
            return skins.ToArray();
        }


        public void AddAllInteriors()
        {
            interiors = DbInterior.GetAll();
        }
        public Interior GetInterior(uint id)
        {
            foreach (Interior interior in interiors)
            {
                if (interior.Id == id) return interior;
            }
            return null;
        }
        public Interior[] GetInteriors()
        {
            return interiors;
        }

        public void AddOrganization(Organization org)
        {
            organizations.Add(org);
        }

        public void AddAllOrganizations()
        {
            organizations = DbOrganization.GetAll().ToList();
            foreach (Organization organization in organizations)
            {
                Rank[] ranks = DbRank.GetAll(organization.Id);
                foreach (Rank rank in ranks)
                {
                    organization.AddRank(rank);
                }
            }
        }
        public Organization GetOrganization(uint id)
        {
            foreach (Organization organization in organizations)
            {
                if (organization.Id == id) return organization;
            }

            return null;
        }
        public Organization[] GetOrganizations()
        {
            return organizations.ToArray();
        }
        public void RemoveOrganization(Organization organization)
        {
            organizations.Remove(organization);
            DbOrganization.Delete(organization);
            //if (organization.Id != 0) DbVehicle.Delete(organization);

        }


        public Link AddLink(Link link)
        {
            Link sLink = GetLink(link);
            if (sLink == null)
            {
                links.Add(link);
                return link;
            }
            else
            {
                sLink.Type = link.Type;
                return sLink;
            }
        }
        public void AddAllLinks()
        {
            links = DbLink.GetAll().ToList();
        }

        public Link GetLink(byte from, byte to, ushort drawableFrom, ushort drawableTo)
        {
            foreach (Link link in links)
            {
                if (link.Component1.Item1 == from &&
                    link.Component1.Item2 == drawableFrom &&
                    link.Component2.Item1 == to &&
                    link.Component2.Item2 == drawableTo)
                {
                    return link;
                }
            }

            return null;

        }
        public Link GetLink(Link link)
        {
            return GetLink(link.Component1.Item1, link.Component2.Item1, link.Component1.Item2, link.Component2.Item2);
        }
        public Link[] GetLinks()
        {
            return links.ToArray();
        }

        public static Game BaseGame = new Game();

    }
}
