namespace ByteCarve.Models;
using Microsoft.Data.Sqlite;
using static System.Console;

public class database
{
    private string path="/home/bro/my-creations/ByteCarve/ByteCarve/Assets/admin.db";
    public SqliteConnection con;
    public database()
    {
        con= new SqliteConnection(path);
        
    }

    public void add(string n,string a,string t,int i,int d)
    {
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "INSERT INTO tests (test_name,app_name,type,images,duration) VALUES ($n,$a,$t,$i,$d);";
        cmd.Parameters.AddWithValue("$n", n);
        cmd.Parameters.AddWithValue("$a", a);
        cmd.Parameters.AddWithValue("$t", t);
        cmd.Parameters.AddWithValue("$i", i);
        cmd.Parameters.AddWithValue("$d", d);
        cmd.ExecuteNonQuery();
    }
    
}