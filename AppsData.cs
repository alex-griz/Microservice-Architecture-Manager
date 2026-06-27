using System.Diagnostics;
using Microsoft.Data.Sqlite;
namespace MicroServiceManager;
class AppsData
{
    public static int GetStatus(string name)
    {
        using Process process = Process.GetProcessesByName(name).FirstOrDefault();
        if (process == null){return 0;}
        else{process.Refresh(); return 1;}
    }
    public static string[] GetData(string name)
    {
        string[] response = ["","",""];
        using var connection = new SqliteConnection($"Data Source= Database/MSMData.db");
        using var command = new SqliteCommand("SELECT Path, Log, Dependencies FROM Services WHERE Name = @N", connection);
        command.Parameters.AddWithValue("@N", name);
        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            response[0] = reader["Path"].ToString();
            response[1] = reader["Log"].ToString();
            response[2] = reader["Dependencies"].ToString();
        }
        return response;
    }
    public static string[] GetMetrics(string name)
    {
        try
        {
            using Process process = Process.GetProcessesByName(name).FirstOrDefault();
            TimeSpan processTime1 = process.TotalProcessorTime;

            DateTime realTime1 = DateTime.UtcNow;
            Thread.Sleep(1000);
            TimeSpan processTime2 = process.TotalProcessorTime;
            DateTime realTime2 = DateTime.UtcNow;
            double CpuUsage = (processTime2 - processTime1).TotalMilliseconds / ((realTime2 - realTime1).TotalMilliseconds * Environment.ProcessorCount) * 100;

            TimeSpan uptime = DateTime.Now - process.StartTime;
            double ramUsage = process.WorkingSet64 / 1024.0 / 1024.0;
            return [uptime.ToString(), CpuUsage.ToString(), ramUsage.ToString()];
        }
        catch
        {
            return ["00:00:00", "0", "0"];
        }
    }
}