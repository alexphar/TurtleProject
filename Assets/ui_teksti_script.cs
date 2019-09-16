using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //tarvitaan UI:n tekstiä ja nappia varten
using MySql.Data;
using MySql.Data.MySqlClient; //tarvittava mysql kirjasto yhteyttä varten. lisää mysql dll:t ja system data dll assetseihin ja muuta asetuksista project settings -> player ja valitse net 4x

public class ui_teksti_script : MonoBehaviour 
{
	public Text testiTeksti; //muista lisätä ui_teksti_scriptiin teksti objekti jotta se voidaan ottaa käyttöön
	public Text teksti2;
	public Text teksti3;
	public Text teksti5;
	public Text rotationText;
	public Text temperatureText;
	
	public Button foodButton;

    AudioSource ree1; //äänite1
    AudioSource ree2; //äänite2

    bool m_Play;
	bool m_Play2;

	private DBClass testi;
	
	void Start ()// alustaja
	{
		Debug.Log("Startissa");

        AudioSource[] audioSources = GetComponents<AudioSource>(); //lisätään audiolista objektin komponentiksi
        ree1 = audioSources[0]; //lisätään äänitteet audiolistaan
        ree2 = audioSources[1];
        
		testi = gameObject.AddComponent<DBClass>(); //lisätään db luokka objektin komponenteista
        
		m_Play = false; //asetetaan booleat falseksi jotta ne voidaan ottaa käyttöön vain tietyissä tilanteissa
		m_Play2 = false;
		
		foodButton.onClick.AddListener(clicked); //ruokintanappi kuuntelee clicked funktiota, eli klikatessa konnalta vähenee nälkäpisteitä
	}
	
	
	void Update () // päivittää once per frame
	{
		 testi.OpenConnection(); //avataan yhteys DB luokan funktiolla
		 testi.readData(); //käytetään funktiota joka asettaa databasin arvot muuttujiksi joita pystytään lukemaan
		 textData(); //asetetaan arvot teksti objekteihin jotta käyttäjä voi lukea arvoja ilman control panelia
		 
		 transform.localEulerAngles = new Vector3(float.Parse(testi.kulmax),0.0f,float.Parse(testi.kulmay)); //asetetaan objektille (kokeiluvaiheessa cube) arvot x z ja y jotta objekti voi liikkua sensoreiden antamilla arvoilla
		 
		 wrongRotation(); //käytetään funktiota joka tuo esille valitustekstin kun konna on selällään tai kyljellään
		 temperature(); //funktio tuo esille konnan lämpöreaktiot
		 
		 testi.CloseConnection(); //suljetaan yhteys
	}
	
	void textData () //asetetaan teksti objekteille luettavat arvot
	{ 
		 testiTeksti.text = "lämpötila: " + testi.lampoTesti; //luetaan lämpöarvo tekstin kautta
		 teksti2.text = "kulma x: " + testi.kulmax; //kulma x
		 teksti3.text = "kulma y: " + testi.kulmay; //kulma y
		 teksti5.text = "nälkä: " + testi.nalkaArvo; //nälkälukema
	}
	
	void clicked () //päivitetään nälkäarvoa napin painalluksella. 500 nälkäpistettä vähenee per klikkaus
	{ 
		Debug.Log("Nappia painettu");
		
		testi.OpenConnection();
		testi.updateData();
		testi.CloseConnection();
	}
	
	void wrongRotation () //asetetaan arvot kulmille jotta valitusteksti tulee esille kun konna on selällään. valitusääni alkaa jo 90 kulman kohdalla
	{
			 
	  if (float.Parse(testi.kulmax) < -130 || float.Parse(testi.kulmax) > 130 || float.Parse(testi.kulmay) < -130 || float.Parse(testi.kulmay) > 130) //jos konna on selällään
	  {
		rotationText.enabled = true; //otetaan teksti käyttöön
		rotationText.text = "REEEEEE KÄÄÄNNÄ MUT";
	  }
	  
	  else if (float.Parse(testi.kulmax) < -90 || float.Parse(testi.kulmax) > 90 || float.Parse(testi.kulmay) < -90 || float.Parse(testi.kulmay) > 90) //kyljellään
	  {
		rotationText.enabled = true;
		rotationText.text = "REEEEEEEE";
            if(m_Play == false) //jos äänite ei ole jo käynnissä, soitetaan äänite 1
            {
                ree1.Play();
                m_Play = true;
            }
	  }
	  
	  else //muussa tapauksessa otetaan teksti ja äänite pois käytöstä
	  {
		 rotationText.enabled = false;
            if(m_Play == true)
            {
                ree1.Stop();
                m_Play = false;
            }
	  }
	}
	
	void temperature () //lampötilan vaikutus 
	{ 
		if(float.Parse(testi.lampoTesti) < 15 && float.Parse(testi.lampoTesti) >= 10 ) //jos lämpötilä on pienempi kuin 15 mutta suurempi tai yhtäsuuri kuin 10
		{
			temperatureText.enabled = true; //teksti käyttöön
			temperatureText.text = "Onpa kylmä!";
		}
		
		else if(float.Parse(testi.lampoTesti) < 10 && float.Parse(testi.lampoTesti) >= 0)
		{
			temperatureText.enabled = true;
			temperatureText.text = "Väsyttää.. horrostila kutsuu";
			
			if(m_Play2 == true) //otetaan äänite 2 pois käytöstä
            {
                ree2.Stop();
                m_Play2 = false;
            }
		}
		
		else if(float.Parse(testi.lampoTesti) < 0 && float.Parse(testi.lampoTesti) >= -40)
		{
			temperatureText.enabled = true;
			temperatureText.text = "Kuollut";
			
			if(m_Play2 == false) //laitetaan äänite 2 käyttöön kun lämpötila laskee alle nollaan
			{
				ree2.Play();
				m_Play2 = true;
			}
		}
		else if(float.Parse(testi.lampoTesti) > 30 && float.Parse(testi.lampoTesti) <= 45)
		{
			temperatureText.enabled = true;
			temperatureText.text = "Onpa lämmin!";
		}
		else if(float.Parse(testi.lampoTesti) > 45 && float.Parse(testi.lampoTesti) <= 60)
		{
			temperatureText.enabled = true;
			temperatureText.text = "Voitko viedä mut varjoon?";
			
			if(m_Play2 == true) //varmistetaan ettei äänite ole päällä
		    { 
			ree2.Stop();
			m_Play2 = false;
		    }

		}
		else if(float.Parse(testi.lampoTesti) > 60)
		{
			temperatureText.enabled = true;
			temperatureText.text = "Kuollut";
			
			if(m_Play2 == false) //käytetään äänite 2 jos lämpötila nousee yli 60 astetta
		    {
			ree2.Play();
			m_Play2 = true;
		    }
		}
		else
		{
			temperatureText.enabled = false; //muussa tapauksessa otetaan teksti ja äänite pois käytöstä
			
			 if(m_Play2 == true)
		    {
			ree2.Stop();
			m_Play2 = false;
		    }
		}	
	}
}
