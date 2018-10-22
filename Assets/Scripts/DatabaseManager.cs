using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class DatabaseManager : MonoBehaviour {

	//void Start () {
 //       string conn = "URI=file:" + Application.dataPath + "/Plugins/CardsDatabase.s3db"; //Path to database.
 //       IDbConnection dbconn;
 //       dbconn = (IDbConnection)new SqliteConnection(conn);
 //       dbconn.Open(); //Open connection to the database.
 //       IDbCommand dbcmd = dbconn.CreateCommand();
 //       string sqlQuery = "SELECT * " + "FROM CardActions";
 //       dbcmd.CommandText = sqlQuery;
 //       IDataReader reader = dbcmd.ExecuteReader();
 //       while (reader.Read())
 //       {
 //           int id = reader.GetInt32(0);
 //           string name = reader.GetString(1);
 //           float health = reader.GetFloat(2);
 //           float attack = reader.GetFloat(3);
 //           float defense = reader.GetFloat(4);
 //           string special = reader.GetString(5);


 //           Debug.Log("id = " + id + "  name = " + name + "  health = " + health + "  attack = " + attack + "  defense = " + defense + "  special = " + special);
 //       }
 //       reader.Close();
 //       reader = null;
 //       dbcmd.Dispose();
 //       dbcmd = null;
 //       dbconn.Close();
 //       dbconn = null;
 //   }

    public void ConsultDatabase(Card.ActionStats stats)
    {
        string conn = "URI=file:" + Application.dataPath + "/Plugins/CardsDatabase.s3db"; //Path to database.
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM CardActions " + "WHERE Id = " + stats.GetID();
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            float health = reader.GetFloat(2);
            float attack = reader.GetFloat(3);
            float defense = reader.GetFloat(4);
            string special = reader.GetString(5);
            stats.SetID(id);
            stats.SetName(name);
            stats.SetHealthBonus(health);
            stats.SetAttack(attack);
            stats.SetDefense(defense);
            stats.SetSpecial(special);
        }

        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
}

