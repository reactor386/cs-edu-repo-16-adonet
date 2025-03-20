//-
using System;
using System.IO;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using EFPractices.Tools;
using EFPractices.Entities;


namespace EFPractices;

public class AppContext : DbContext
{
    // Объекты таблиц
    public DbSet<User> Users { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Author> Authors { get; set; }

/*
    // DbContext has RequiresUnreferencedCodeAttribute
    //  and isn't fully compatible with trimming

    // rebuild tables if they don't exist in db
    public AppContext()
    {
        // Database.EnsureDeleted();
        // Database.EnsureCreated();
    }
*/

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(GetConnectionString());
    }


    private static string GetConnectionString()
    {
        // получаем значение строки подключения из файла
        string connectionString = string.Empty;
        string privateDataFolder = DirectoryTools.GetRootForFolderName("private-data");
        if (!string.IsNullOrWhiteSpace(privateDataFolder))
        {
            string connectionStringFilePath = Path.Combine(privateDataFolder, "private-data", "SQL_SERVER_CONNECTION_STRING");
            if (File.Exists(connectionStringFilePath))
            {
                connectionString = File.ReadLines(connectionStringFilePath).First();
            }
        }

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("connection string is not found");

        return connectionString;
    }
}
