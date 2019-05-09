using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.XPath;
using AltV.Net;
using Lambda.Entity;
using Lambda.Items;
using Lambda.Organizations;
using Lambda.Utils;
using MoreLinq;


namespace Lambda.Commands
{
    public class Command
    {
        public enum CommandStatus
        {
            DEFAULT,
            NEW,
            TOTEST
        }
        public enum CommandType
        {
            DEFAULT,
            CHAT,
            INVENTORY,
            VEHICLE,
            ORGANIZATION,
            AREA,
            SHOP,
            HOUSE,
            BANK,
            SKIN,
            ADMIN,
            TEST,
        }

        public enum ParameterError
        {
            ARGUMENTS_LENGTH,
            INTEGER
        }

        private Func<Player, object[], CmdReturn> action;
        private string[] syntax;
        private Type[] syntaxTypes;
        private string permission;
        public CommandType Type;
        public CommandStatus Status;

        public string Name { get; }

        public Command(string name, Func<Player, object[], CmdReturn> action, CommandType type, string[] syntax,
            Type[] syntaxTypes, string permission = "")
        {
            this.Status = CommandStatus.DEFAULT;
            this.Name = name;
            this.action = action;
            this.syntax = syntax;
            this.syntaxTypes = syntaxTypes;
            this.Type = type;
            this.permission = permission;
        }

        public string Syntax()
        {
            string syntaxString = "";
            if (this.syntax != null)
            {
                syntaxString += $"/{Name.Replace("_", " ")}";
                syntaxString = this.syntax.Aggregate(syntaxString, (current, s) => current + $" [{s}]");
            }

            return syntaxString;
        }

        public CmdReturn Execute(Player player, string[] parametersString)
        {
            parametersString = RemoveCommandName(parametersString);
            CmdReturn result = ConvertParamameters(player, parametersString, out object[] parameters);
            if (result.Type != CmdReturn.CmdReturnType.SUCCESS) return result;
            player.SendMessage("PERM : " + permission + " ? " + player.PermissionExist(permission));
            if (!string.IsNullOrWhiteSpace(permission) && !player.PermissionExist(permission)) return new CmdReturn("Pas permission");
            CmdReturn cmdReturn = action(player, parameters); // Launch the command
            player.Game.DbPlayer.Save(player);
            return cmdReturn;
        }

        public CmdReturn ConvertParamameters(Player player, string[] parameters, out object[] result)
        {
            result = new object[parameters.Length];
            if (parameters.Length < syntax.Length) return new CmdReturn(Syntax(), CmdReturn.CmdReturnType.SYNTAX);
            int i = 0;
            for (; i < syntax.Length; i++)
            {
                //Cas string
                if (syntaxTypes[i] == typeof(string))
                {
                    result[i] = parameters[i];
                }
                else if (syntaxTypes[i] == typeof(int))
                {
                    if (!int.TryParse(parameters[i], out int value)) return new CmdReturn($"[DEBUG] 1 - {syntax[i]} doit être un nombre", CmdReturn.CmdReturnType.WARNING);
                    result[i] = value;
                }
                else if (syntaxTypes[i] == typeof(uint))
                {
                    if (!uint.TryParse(parameters[i], out uint value)) return new CmdReturn($"[DEBUG] 1 - {syntax[i]} doit être un nombre suppérieur à zéro", CmdReturn.CmdReturnType.WARNING);
                    result[i] = value;
                }
                else if (syntaxTypes[i] == typeof(short))
                {
                    if (!short.TryParse(parameters[i], out short value)) return new CmdReturn($"[DEBUG] 1 - {syntax[i]} doit être un nombre", CmdReturn.CmdReturnType.WARNING);
                    result[i] = value;
                }
                else if (syntaxTypes[i] == typeof(byte))
                {
                    if (!byte.TryParse(parameters[i], out byte value)) return new CmdReturn($"[DEBUG] 1 - {syntax[i]} doit être un nombre entre 0 et 255", CmdReturn.CmdReturnType.WARNING);
                    result[i] = value;
                }
                else if (syntaxTypes[i] == typeof(Player))
                {
                    string charName = parameters[i];
                    Player[] players = player.Game.GetPlayers(charName);
                    CmdReturn cmdReturn = CmdReturn.OnlyOnePlayer(players);
                    if (cmdReturn.Type != CmdReturn.CmdReturnType.SUCCESS) return cmdReturn;
                    result[i] = players[0];
                }
                else if (syntaxTypes[i] == typeof(Item))
                {
                    if (!int.TryParse(parameters[i], out int itemid)) return new CmdReturn($"{syntax[i]} doit être un identifier d objet", CmdReturn.CmdReturnType.WARNING);
                    if (player.Inventory.Items.Count < i) return new CmdReturn($"Le nombre doit etre inférieur à {player.Inventory.Items.Count}, vous avez {player.Inventory.Items.Count} objets dans votre inventaire", CmdReturn.CmdReturnType.WARNING);
                    Item item = player.Inventory.Items[itemid];
                    if (item == null) return new CmdReturn("Vous n'avez pas cet objet dans votre inventaire", CmdReturn.CmdReturnType.WARNING);
                    result[i] = item;
                }
                else if (syntaxTypes[i] == typeof(BaseItem))
                {
                    if (!uint.TryParse(parameters[i], out uint itemid)) return new CmdReturn($"{syntax[i]} doit être un identifier d objet", CmdReturn.CmdReturnType.WARNING);
                    BaseItem baseItem = player.Game.GetBaseItem(itemid);
                    if (baseItem == null) return new CmdReturn("L objet n existe pas", CmdReturn.CmdReturnType.WARNING);
                    result[i] = baseItem;
                }
                else if (syntaxTypes[i] == typeof(Organization))
                {
                    Organization[] orgs = player.Game.GetOrganizations(parameters[i]);
                    CmdReturn cmdReturn = CmdReturn.OnlyOneOrganization(orgs);
                    if (cmdReturn.Type != CmdReturn.CmdReturnType.SUCCESS) return cmdReturn;
                    result[i] = orgs[0];
                }
                else if (syntaxTypes[i] == typeof(Rank))
                {
                    if (i <= 0 || syntaxTypes[i - 1] != typeof(Organization)) return CmdReturn.NotExceptedError;
                    Organization org = (Organization)result[i - 1];
                    Rank[] ranks = org.GetRanks(parameters[i]);
                    CmdReturn cmdReturn = CmdReturn.OnlyOneRank(ranks);
                    if (cmdReturn.Type != CmdReturn.CmdReturnType.SUCCESS) return cmdReturn;
                    result[i] = ranks[0];

                }
                else if (syntaxTypes[i] == typeof(Interior))
                {
                    Interior[] interiors = player.Game.GetInteriors(parameters[i]);
                    CmdReturn cmdReturn = CmdReturn.OnlyOneInterior(interiors);
                    if (cmdReturn.Type != CmdReturn.CmdReturnType.SUCCESS) return cmdReturn;
                    result[i] = interiors[0];
                }
                else
                {
                    result[i] = parameters[i];
                }
            }

            for (; i < parameters.Length; i++)
            {
                result[i] = parameters[i];
            }
            return CmdReturn.Success;
        }

        public string[] RemoveCommandName(string[] parameters)
        {
            int nbrToRemove = Name.Split("_").Length;
            return parameters.Slice(nbrToRemove, parameters.Length).ToArray();
        }

        public static Command[] GetAllCommands(string namespaceName = "", Assembly assembly = null)
        {

            if (namespaceName.Length == 0) namespaceName = typeof(Command).Namespace; //If no namespace is specified
            if (assembly == null) assembly = Assembly.GetExecutingAssembly();
            Type[] types = GetTypesInNamespace(assembly, namespaceName); // Get all classes in this namespace
            Alt.Log($"Nombre de types dans le namespace {namespaceName}: " + types.Length);
            List<Command> commands = new List<Command>();
            foreach (Type type in types)
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    if (method.GetCustomAttributes(typeof(CommandAttribute), false).Length <= 0) continue;
                    if (method.GetParameters()[0].ParameterType != typeof(Player) ||
                        method.GetParameters()[1].ParameterType != typeof(object[])) throw new Exception($"Commande {method.Name} mal définie");
                    string name = method.Name.ToLower();
                    Func<Player, object[], CmdReturn> action = (Func<Player, object[], CmdReturn>)Delegate.CreateDelegate(typeof(Func<Player, object[], CmdReturn>), method);
                    CommandAttribute commandAttribute = (CommandAttribute)method.GetCustomAttributes(typeof(CommandAttribute), false)[0];
                    CommandType commandType = commandAttribute.Type;
                    SyntaxAttribute[] syntaxAttributes = (SyntaxAttribute[])method.GetCustomAttributes(typeof(SyntaxAttribute), false);
                    if (syntaxAttributes.Length == 0 && commandAttribute.Length > 0) throw new Exception("Command dont have syntax attribute");
                    SyntaxTypeAttribute[] syntaxTypeAttributes = (SyntaxTypeAttribute[])method.GetCustomAttributes(typeof(SyntaxTypeAttribute), false);
                    if (syntaxTypeAttributes.Length == 0 && commandAttribute.Length > 0) throw new Exception("Command dont have syntax type attribute");
                    string[] syntax = new string[0];
                    Type[] syntaxTypes = new Type[0];
                    if (syntaxAttributes.Length > 0 && syntaxAttributes[0].Attributes.Length > 0)
                    {
                        syntax = syntaxAttributes[0].Attributes;
                    }
                    if (syntaxTypeAttributes.Length > 0 && syntaxTypeAttributes[0].Types.Length > 0)
                    {
                        syntaxTypes = syntaxTypeAttributes[0].Types;
                    }
                    if (commandAttribute.Length != syntax.Length || commandAttribute.Length != syntaxTypes.Length) throw new Exception($"Commande {method.Name} mal définie");
                    Command command = new Command(name, action, commandType, syntax, syntaxTypes);
                    StatusAttribute[] statusAttributes = (StatusAttribute[])method.GetCustomAttributes(typeof(StatusAttribute), false);
                    if (statusAttributes.Length > 0) command.Status = statusAttributes[0].Status;
                    PermissionAttribute[] permissionsAttributes = (PermissionAttribute[])method.GetCustomAttributes(typeof(PermissionAttribute), false);
                    if (permissionsAttributes.Length > 0) command.permission = permissionsAttributes[0].Permission;
                    Alt.Log($"-{command.Name} registered");
                    commands.Add(command);

                    /*Command.CommandFunc function = (Command.CommandFunc)Delegate.CreateDelegate(typeof(Command.CommandFunc), method, false); // Create the delegate of my méthod
                    //if (function == null) Alt.Log("FUNCTION NULL" + method.Name);
                    CommandAttribute attribute = (CommandAttribute)method.GetCustomAttributes(typeof(CommandAttribute), false)[0];
                    PermissionAttribute[] permissionAttributes = (PermissionAttribute[])method.GetCustomAttributes(typeof(PermissionAttribute), false);
                    string permission = "";
                    if (permissionAttributes.Length > 0) permission = permissionAttributes[0].Permission;
                    Command command = new Command(method.Name.ToLower(), function, attribute.Type, attribute.Syntax, permission);
                    AddCommand(command); // Register the command*/
                }
            }

            return commands.ToArray();
        }
        public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }


    }
}
