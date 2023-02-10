using UnityEngine;
using MySql.Data.MySqlClient;


public class DatabaseConnection : MonoBehaviour
{
    private MySqlConnection connection;
    private string server;
    private string database;
    private string uid;
    private string password;
    private int port;

    void Start()
    {
        server = "db-battleheaven-do-user-13276797-0.b.db.ondigitalocean.com";
        port = 25060;
        database = "mydb";
        uid = "adrian";
        password = "AVNS_8bvMSQErn_sucVxVkAb";

        string connectionString = "Server=" + server + "; Port=" + port + "; Database=" + database + "; Uid=" + uid + "; Pwd=" + password + "; SslMode=REQUIRED;" + "Connection Timeout=30;";

        connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            Debug.Log("Connected to database");
            RetrieveData();
        }
        catch (MySqlException ex)
        {
            Debug.Log(ex);
            switch (ex.Number)
            {
                case 0:
                    Debug.Log("Cannot connect to server. Contact administrator");
                    break;
                case 1045:
                    Debug.Log("Invalid username/password, please try again");
                    break;
            }
        }
    }

    private void RetrieveData()
    {
        string query = "SELECT * FROM skill_category;";
        MySqlCommand cmd = new MySqlCommand(query, connection);
        MySqlDataReader dataReader = cmd.ExecuteReader();
        while (dataReader.Read())
        {
            Debug.Log("skill_category_id: " + dataReader["skill_category_id"] + ", name: " + dataReader["name"] + ", description: " + dataReader["description"]);
        }
        dataReader.Close();
    }

    void OnApplicationQuit()
    {
        if (connection != null)
        {
            connection.Close();
        }
    }
}
