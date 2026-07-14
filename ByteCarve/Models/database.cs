namespace ByteCarve.Models;
using Microsoft.Data.Sqlite;
using static System.Console;
using System.Collections.Generic;

public class database
{
    public class ops
    {
        public string name{ get; set; }
        public string a_name{ get; set; }
        public string type{ get; set; }
        public int images{ get; set; }
        public int dur{ get; set; }

    }
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

    public List<ops> fetch()
    {
        List<ops> tempo=new List<ops>();
        con.Open();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT * FROM tests;";
        using var res = cmd.ExecuteReader();
        while (res.Read())
        {
            ops temp=new ops();
            temp.name = res.GetString(0);
            temp.a_name = res.GetString(1);
            temp.type = res.GetString(2);
            temp.images = res.GetInt32(3);
            temp.dur = res.GetInt32(4);
            tempo.Add(temp);
        }

        return tempo;
    }
    
}