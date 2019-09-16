using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data;
using MySql.Data.MySqlClient;

public class DBClass : MonoBehaviour
    {
        private MySqlConnection connection; //alustetaan yhteyteen tarvittavat tiedot
        private string server;
        private string database;
        private string uid;
        private string password;
		
		public string lampoTesti {get; set;} //alustetaan databasin arvot jotta niitä voidaan lukea ja ottaa käyttöön
		public string kulmax {get; set;}
		public string kulmay {get; set;}
		public string kulmaz {get; set;}
		public string nalkaArvo {get; set;}
        
        public DBClass() //konstruktori
        {
            Initialize(); //kutsuu alustajaa
        }

        private void Initialize() //alustetaan connectionstringin arvot database yhteyden tiedoilla
        {
            server = "mysli.oamk.fi";
            database = "opisk_t7hani00";
            uid = "t7hani00";
            password = "************"; //muutettu julkaisun vuoksi
            
		string connectionString; //luodaan connectionstring yhteyttä varten. tarvitaan databasen omistajan tiedot
            
		connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString); //luodaan tästä connectionstringistä olio jotta yhteys voidaan avata ja sulkea
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
                Debug.LogError("MySQL Error: " + ex.ToString()); //tuodaan error viesti jos yhteyden luomisessa on ongelmia
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

	public void readData() //haetaan dataa ja luetaan
	{			
		string query = "SELECT * FROM sensori_data WHERE data_id = 1"; //valitaan kaikki arvot sensori_data taulusta

                var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection); //luodaan olio joka tietää connection stringin ja queryn
                var reader = cmd.ExecuteReader(); //reader joka osaa lukea arvoja tietokannasta

                while (reader.Read())
                {
                    var lampo = reader["lampotila"]; //asetetaan lampotila muuttujaksi taulusta sensori_data
			lampoTesti = lampo.ToString();	//lampotesti on string lampotilasta. voidaan hyödyntää muun muuassa tekstien kautta
                    
			var kulma1 = reader["kulma_x"]; //asetetaan kulma_x muuttujaksi taulusta sensori_data
			kulmax = kulma1.ToString();
					
                    var kulma2 = reader["kulma_y"]; //asetetaan kulma_y muuttujaksi taulusta sensori_data
			kulmay = kulma2.ToString();
					
                    var kulma3 = reader["kulma_z"]; //asetetaan kulma_z muuttujaksi taulusta sensori_data
			kulmaz = kulma3.ToString();
					
			var nalka = reader["nalka"]; //asetetaan nalka muuttujaksi taulusta sensori_data
			nalkaArvo = nalka.ToString();

			Debug.Log("lampotila: " + lampo);//luetaan arvot debuggina. tässä lämpötilan lukua 
			Debug.Log("kulmax: " + kulma1);
			Debug.Log("kulmay: " + kulma2);
			Debug.Log("kulmaz: " + kulma3);
			Debug.Log("nalka: " + nalka);
                }
	}
		
	public void updateData() //päivitetään nälkäarvoa. kutsutaan kun painetaan nappia
	{
		string query;
			
		if(int.Parse(nalkaArvo) < 500) //estetään nälkäarvoa menemästä miinuksen puolelle
		{
			query = "UPDATE sensori_data SET nalka = 0 WHERE data_id = 1"; //asetetaan nalka nollaksi tässä tilanteessa
		}
		else
		{
			query = "UPDATE sensori_data SET nalka=nalka - 500 WHERE data_id = 1"; //vähennettävä haluttu määrä nälkäarvosta
		}
				
		MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, connection); //tarvittava olio databasen yhteydestä ja querystä
		cmd.ExecuteNonQuery(); //toteutetaan query
	}
}
