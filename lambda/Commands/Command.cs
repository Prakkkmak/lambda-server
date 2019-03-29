using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AltV.Net;
using Lambda.Entity;
using Lambda.Administration;

namespace Lambda.Commands
{
    /// <summary>
    /// Custom attribute for specify commands methods
    /// </summary>
    public class CommandAttribute : Attribute
    {
        public Command.CommandType Type;
        public string[] Syntax;
        public CommandAttribute(Command.CommandType type, params string[] syntax)
        {
            this.Type = type;
            this.Syntax = syntax;
        }
    }

    /// <summary>
    /// A command is an action triggered by the player with the command in game or with an UI
    /// </summary>
    public class Command
    {
        public delegate CmdReturn CommandFunc(Player player, string[] parameters); // The declaration delegate of the method

        public enum CommandType
        {
            DEFAULT,
            ACCOUNT,
            ADMIN,
            TEST
        }
        public string Name { get; } // The Name of the command
        private CommandFunc func; // The function associed
        private CommandType type;
        private string[] syntax;
        /// <summary>
        /// Construct a command
        /// </summary>
        /// <param Name="name">The Name of the command</param>
        /// <param Name="func">The function to perform</param>
        public Command(string name, CommandFunc func, CommandType type, string[] syntax)
        {
            this.Name = name;
            this.func = func;
            this.syntax = syntax;
            this.type = type;
        }
        /// <summary>
        /// Generate the syntax of the command
        /// </summary>
        /// <returns>The cmd return with the syntax</returns>
        public CmdReturn Syntax()
        {
            if (this.Name == null) throw new NullReferenceException();
            string syntaxString = "";
            if (this.syntax != null)
            {
                syntaxString += $"/{Name.Replace("_", " ")}";
                syntaxString = this.syntax.Aggregate(syntaxString, (current, s) => current + $" [{s}]");
            }
            return new CmdReturn(syntaxString, CmdReturn.CmdReturnType.SYNTAX);
        }

        /// <summary>
        /// Execute the f° associated with the command
        /// </summary>
        /// <param name="account">The account who perform the command</param>
        /// <param name="parameters">The parameters list given</param>
        /// <returns></returns>
        public CmdReturn Execute(Player player, string[] parameters)
        {
            if (syntax == null) throw new NullReferenceException();
            if (player.Account == null && type != CommandType.ACCOUNT)
            {
                return new CmdReturn("Vous n etes pas connecté", CmdReturn.CmdReturnType.WARNING);
            }

            if (parameters.Length < syntax.Length + Name.Split("_").Length)  // syntax.length is parameters. The strict is for the Name
            {
                return Syntax();
            }

            if (func == null) throw new NullReferenceException("Aucune fonction n'est définie");
            CmdReturn cmdReturn = func(player, parameters); // Launch the command
            if (player.Account != null) player.Save();
            return cmdReturn;
        }
        /// <summary>
        /// Get commands by parameters
        /// </summary>
        /// <param name="parameters">The parameters ( name + params ) used</param>
        /// <returns>Returns the commands</returns>
        public static Command[] GetCommands(string[] parameters)
        {
            List<Command> cmds = new List<Command>(Commands);
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
        /// <summary>
        /// Register a command to the list of commands
        /// </summary>
        /// <param Name="command">The command</param>
        public static void RegisterCommand(Command command)
        {
            Commands.Add(command);
            Alt.Log($"[COMMAND]{command.Name} registered");
        }
        /// <summary>
        /// Register all commands in a specific namespace. The function need to have the [Command] attribute
        /// </summary>
        /// <param Name="namespaceName">the namespace. Leave it to get the current namespace</param>
        public static void RegisterAllCommands(string namespaceName = "", Assembly assembly = null)
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
                        CommandFunc function = (CommandFunc)Delegate.CreateDelegate(typeof(CommandFunc), method, false); // Create the delegate of my méthod
                        if (function == null) Alt.Log("FUNCTION NULL" + method.Name);
                        CommandAttribute attribute = (CommandAttribute)method.GetCustomAttributes(typeof(CommandAttribute), false)[0];
                        Command command = new Command(method.Name.ToLower(), function, attribute.Type, attribute.Syntax);
                        RegisterCommand(command); // Register the command
                    }
                    else
                    {
                        //TODO Erreur dans l'enregistrement de la commande mauvais param
                    }
                }
            }
        }
        /// <summary>
        /// Get all classes in a namespace
        /// </summary>
        /// <param Name="assembly">The assembly ( use Assembly.GetExecutingAssembly() for the current)</param>
        /// <param Name="nameSpace">The namespace to use</param>
        /// <returns>Returns all types</returns>
        public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToArray();
        }

        public static List<Command> Commands = new List<Command>();
    }
}
