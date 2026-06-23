using System.Diagnostics;
using Microsoft.Data.Sqlite;
namespace MicroServiceManager;
class Commands
{
    private static readonly string connectionString = $"Data Source= Database/MSMData.db";
    public static void Create(string name, string path, string log_path = "", string dependence_list = "")
    {
        if (Program.nameCache.Contains(name)) {Console.WriteLine("Service with this name already exists"); return;}
        using var connection = new SqliteConnection(connectionString);
        using var command = new SqliteCommand("INSERT INTO Services (Name, Path, Log, Dependencies) VALUES (@N, @P, @L, @D)", connection);
        command.Parameters.AddWithValue("@N", name);
        command.Parameters.AddWithValue("@P", path);
        command.Parameters.AddWithValue("@L", log_path);
        command.Parameters.AddWithValue("@D", dependence_list);

        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            Program.nameCache.Add(name);
            Console.WriteLine("Service added successfully!");
        }
        catch
        {
            Console.WriteLine("Error while adding service");
        }
    }
    public static void Remove(string name)
    {
        using var connection = new SqliteConnection(connectionString);
        using var command = new SqliteCommand("DELETE FROM Services WHERE `Name` = @N", connection);
        command.Parameters.AddWithValue("@N", name);
        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            Program.nameCache.Remove(name);
            Console.WriteLine("Service removed successfully!");
        }
        catch
        {
            Console.WriteLine("Error while removing service");
        }
    }
    public static void Run(string name)
    {
        string path = GetData(name)[0];
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("Service with this name not found!");
            return;
        }
        string ext = Path.GetExtension(path);
        if (ext == ".exe")
        {
            try
            {
                Process.Start(path);
                Console.WriteLine("Component launched successfully");
            }
            catch
            {
                Console.WriteLine("Error while launching this component");
            }
            return;
        }
        string startCommand = ext switch
        {
            ".py" => "python",
            ".js" => "node",
            ".ts" => "ts-node",
            ".go" => "go run",
            ".jar" => "java -jar",
            ".sh" => "bash",
            ".bat" => "cmd /c",
            _ => null
        };
        if (startCommand != null)
        {
            try
            {
                Process.Start(startCommand, path);
                Console.WriteLine("Component launched successfully");
            }
            catch
            {
                Console.WriteLine("Error while launching this component");
            }
        }
        else
        {
            Console.WriteLine("This component is not intended to run");
        }
    }
    public static void Stop(string name)
    {
        string processName = Path.GetFileNameWithoutExtension(GetData(name)[0]);
        Process[] processes = Process.GetProcessesByName(processName);
        try
        {
            foreach (var p in processes)
            {
                p.Kill();
            }
            Console.WriteLine("Service stopped successfully");
        }
        catch
        {
            Console.WriteLine("Failed to stop the service. This may require special permissions.");
        }
    }
    public static void Stats(string name)
    {
        Process currentProcess = Process.GetProcessesByName(name).FirstOrDefault();
        if (currentProcess == null)
        {
            Console.WriteLine("Status:   disabled");
            return;
        }
        currentProcess.Refresh();
        Console.WriteLine("Status:   enabled");

        TimeSpan uptime = DateTime.Now - currentProcess.StartTime;
        Console.WriteLine($"Uptime:   {uptime}");
        double cpuUsage = GetCPU(currentProcess);
        Console.WriteLine($"CPU Usage:   {cpuUsage}%");
        double ramUsage = currentProcess.WorkingSet64 / 1024.0 / 1024.0;
        Console.WriteLine($"RAM Usage:   {ramUsage}Mb");
    }
    public static void Errors(string name)
    {
        string path = GetData(name)[1];
        if (string.IsNullOrEmpty(path))
        {
            Console.WriteLine("This service don't have log files!");
            return;
        }
        foreach (string line in File.ReadLines(path))
        {
            string newLine = line.ToLower();
            if (newLine.Contains("error"))
            {
                Console.WriteLine(line);
            }
        }
    }
    public static void Problems(){}
    public static void List()
    {
        using var connection = new SqliteConnection(connectionString);
        using var command = new SqliteCommand("SELECT * FROM Services", connection);
        try
        {
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine("ID: "+reader["ID"].ToString());
                Console.WriteLine("Name:  "+reader["Name"].ToString());
                Console.WriteLine("Path:  "+ reader["Path"].ToString());
                Console.WriteLine("Log path:  "+reader["Log"].ToString()+"\n ");
            }
        }
        catch   
        {
            Console.WriteLine("Error while loading list of services");
        }
    }
    public static void Edit(string name, string path, string log_path = "", string dependence_list = "")
    {
        using var connection = new SqliteConnection(connectionString);
        using var command = new SqliteCommand("UPDATE Services SET Path = @P, Log = @L, Dependencies= @D WHERE Name = @N", connection);
        command.Parameters.AddWithValue("@N", name);
        command.Parameters.AddWithValue("@P", path);
        command.Parameters.AddWithValue("@L", log_path);
        command.Parameters.AddWithValue("@D", dependence_list);
        try
        {
            connection.Open();
            command.ExecuteNonQuery();
            Program.nameCache.Add(name);
            Console.WriteLine("Service data updated successfully!");
        }
        catch
        {
            Console.WriteLine("Error while updating service data");
        }
    }
    public static string[] GetData(string name)
    {
        string[] response = ["",""];
        using var connection = new SqliteConnection(connectionString);
        using var command = new SqliteCommand("SELECT Path, Log FROM Services WHERE Name = @N", connection);
        command.Parameters.AddWithValue("@N", name);
        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            response[0] = reader["Path"].ToString();
            response[1] = reader["Log"].ToString();
        }
        return response;
    }
    public static double GetCPU(Process process)
    {
        TimeSpan processTime1 = process.TotalProcessorTime;
        DateTime realTime1 = DateTime.UtcNow;
        Thread.Sleep(1000);
        TimeSpan processTime2 = process.TotalProcessorTime;
        DateTime realTime2 = DateTime.UtcNow;
        double CpuUsage = (processTime2 - processTime1).TotalMilliseconds / ((realTime2 - realTime1).TotalMilliseconds * Environment.ProcessorCount) * 100;

        return CpuUsage;
    }
}