namespace MicroServiceManager;

class Program
{
    public static HashSet<string> nameCache = new HashSet<string>();
    static void Main()
    {
        DataBase db = new DataBase();
        nameCache = db.LoadCache();
        Console.WriteLine("===   Welcome to MicroService Manager!   ===");
        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;
            string[] cmd_args = input.Split();

            switch (cmd_args[0])
            {
                case "create":
                    if (cmd_args.Length < 3) {Console.WriteLine("Using the command: create <name> <path> <log path (optional)>"); break;}
                    Commands.Create(cmd_args[1], cmd_args[2],  cmd_args.ElementAtOrDefault(3) ?? "");
                    break;
                case "remove":
                    if (cmd_args.Length < 2){Console.WriteLine("Using the command: remove <name>"); break;}
                    Commands.Remove(cmd_args[1]);
                    break;
                case "run":
                    if (cmd_args.Length <2){Console.WriteLine("Using the command: run <name>"); break;}
                    Commands.Run(cmd_args[1]);
                    break;
                case "stop":
                    if (cmd_args.Length <2){Console.WriteLine("Using the command: stop <name>"); break;}
                    Commands.Stop(cmd_args[1]);
                    break;
                case "stats":
                    if (cmd_args.Length <2){Console.WriteLine("Using the command: stats <name>"); break;}
                    Commands.Stats(cmd_args[1]);
                    break;
                case "dependencies":
                    if (cmd_args.Length <2){Console.WriteLine("Using the command: dependencies <name>"); break;}
                    Commands.Dependencies(cmd_args[1]);
                    break;
                case "errors":
                    Commands.Errors();
                    break;
                case "problems":
                    if (cmd_args.Length < 2){Console.WriteLine("Using command: problems <name>"); break;}
                    Commands.Problems(cmd_args[1]);
                    break;
                case "list":
                    Commands.List();
                    break;
                case "exit":
                    return;
                default:
                    Console.WriteLine($"Unknown command: {cmd_args[0]}");
                    break;
            }
        }
    }
}