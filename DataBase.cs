using Microsoft.Data.Sqlite;
namespace MicroServiceManager;
class DataBase
{
    private readonly string connectionString = $"Data Source= Database/MSMData.db";

    public void CreateDB() //единоразовый метод для создания файлов БД
    {
        if (!Directory.Exists("Database"))
        {
            Directory.CreateDirectory("Database");
            File.Create("Database/MSMData.db").Close();

            string createServicesTable = "CREATE TABLE Services (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name VARCHAR(128), Path VARCHAR(512), Log VARCHAR(512), Dependencies TEXT)";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            using var command = new SqliteCommand(createServicesTable, connection);
            command.ExecuteNonQuery();
        }
    }
    public HashSet<string> LoadCache() //метод для загрузки кэша использованных имён сервисов
    {
        HashSet<string> nameCache = new HashSet<string>();
        string loadNameCache = "SELECT Name FROM Services";
        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        using var command = new SqliteCommand(loadNameCache, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            nameCache.Add(reader.GetString(0));
        }
        return nameCache;
    }
}