using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data;
using MySql.Data.MySqlClient;



public class DBClass : MonoBehaviour
    {

        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
		public string lampoTesti {get; set;}
		public string kulmax {get; set;}
		public string kulmay {get; set;}
		public string kulmaz {get; set;}
		public string nalkaArvo {get; set;}
        

        public DBClass() //konstruktori
        {
            Initialize();
        }

        private void Initialize() //alustetaan connectionstringin arvot database yhteyden tiedoilla
        {
            server = "mysli.oamk.fi";
            database = "opisk_t7hani00";
            uid = "t7hani00";
            password = "NEwttgtub7h3FHYq";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        public bool OpenConnection() //avataan yhteys
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.LogError("MySQL Error: " + ex.ToString());
            }
                return false;
            }
        

        
        public bool CloseConnection() //suljetaan yhteys
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.LogError("MySQL Error: " + ex.ToString());
                return false;
            }
        }

		public void readData() //haetaan dataa
		{			
			string query = "SELECT * FROM sensori_data WHERE data_id = 1"; //valitaan kaikki sensori_data taulusta

                var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var lampo = reader["lampotila"];
					lampoTesti = lampo.ToString();	//luetaan lampotila taulusta sensori_data
                    
					var kulma1 = reader["kulma_x"]; //luetaan kulma_x taulusta sensori_data
					kulmax = kulma1.ToString();
					
                    var kulma2 = reader["kulma_y"];
					kulmay = kulma2.ToString();
					
                    var kulma3 = reader["kulma_z"];
					kulmaz = kulma3.ToString();
					
					var nalka = reader["nalka"];
					nalkaArvo = nalka.ToString();

                    //label1.Text = "x: " + kulma1; //kulma_x:n arvo
                    //label2.Text = "y: " + kulma2; //y
                    //label3.Text = "z: " + kulma3; //z
					Debug.Log("lampotila: " + lampo);//lampötilan arvo
					Debug.Log("kulmax: " + kulma1);
					Debug.Log("kulmay: " + kulma2);
					Debug.Log("kulmaz: " + kulma3);
					Debug.Log("nalka: " + nalka);
                    //counter++; //timerin counter testaamista varten

                }
		}
		
		public void updateData() //päivitetään nälkäarvoa. kutsutaan kun painetaan nappia
		{
			string query;
			
			if(int.Parse(nalkaArvo) < 500) //estetään nälkäarvoa menemästä miinuksen puolelle
			{
				query = "UPDATE sensori_data SET nalka = 0 WHERE data_id = 1";
			}
			else
			{
				query = "UPDATE sensori_data SET nalka=nalka - 500 WHERE data_id = 1"; //vähennettävä määrä nälkäarvosta
			}
				
			MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection);
			cmd.ExecuteNonQuery();
		}
	}
