using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //tarvitaan UI:n tekstiä varten
using MySql.Data;
using MySql.Data.MySqlClient; //lisää mysql dll:t ja system data dll assetseihin ja muuta asetuksista project settings -> player ja valitse net 4x

public class ui_teksti_script : MonoBehaviour {


	public Text testiTeksti; //muista lisätä ui_teksti_scriptiin teksti objecti
	public Text teksti2;
	public Text teksti3;
	public Text teksti5;
	public Text rotationText;
	public Text temperatureText;
	public Button foodButton;

    AudioSource ree1;
    AudioSource ree2;

    bool m_Play;
	bool m_Play2;

	private DBClass testi;
	
	
	
	// Use this for initialization
	void Start () {
	Debug.Log("Startissa");
        //testiTeksti.text = "moi";
        AudioSource[] audioSources = GetComponents<AudioSource>();
        ree1 = audioSources[0];
        ree2 = audioSources[1];
        testi = gameObject.AddComponent<DBClass>(); //lisätään db luokka objectin komponenteista
        m_Play = false;
		m_Play2 = false;
		
		foodButton.onClick.AddListener(clicked); //ruokintanappi
	
	}
	
	// Update is called once per frame
	void Update () {
	 
	 //testi = GetComponen<DBClass>();	
     testi.OpenConnection();
	 testi.readData(); //asetetaan lämpöarvo muuttujaan
	 textData(); //asetetaan arvot tekstiobjekteihin
	 
	 transform.localEulerAngles = new Vector3(float.Parse(testi.kulmax),0.0f,float.Parse(testi.kulmay)); //asetetaan objektille (kokeiluvaiheessa cube) arvot x y ja z jotta objekti voi liikkua sensoreiden antamilla arvoilla
	 
	 wrongRotation(); //valitusteksti kun konna on selällään tai kyljellään
	 temperature(); //lämpöreaktiot
	 
     testi.CloseConnection();
	}
	
	void textData () {
	 
	 testiTeksti.text = "lämpötila: " + testi.lampoTesti; //luetaan lämpöarvo tekstin kautta
	 teksti2.text = "kulma x: " + testi.kulmax;
	 teksti3.text = "kulma y: " + testi.kulmay;
	 teksti5.text = "nälkä: " + testi.nalkaArvo;
	}
	
	void clicked () { //päivitetään nälkäarvoa napin painalluksella. 500 nälkäpistettä vähenee per klikkaus
		Debug.Log("Nappia painettu");
		
		testi.OpenConnection();
		testi.updateData();
		testi.CloseConnection();
	}
	
	void wrongRotation () { //asetetaan arvot kulmille jotta valitusteksti tulee esille kun konna on selällään
			 
	  if (float.Parse(testi.kulmax) < -130 || float.Parse(testi.kulmax) > 130 || float.Parse(testi.kulmay) < -130 || float.Parse(testi.kulmay) > 130) //selällään
	  {
		rotationText.enabled = true;
		rotationText.text = "REEEEEE KÄÄÄNNÄ MUT";
	  }
	  
	  else if (float.Parse(testi.kulmax) < -90 || float.Parse(testi.kulmax) > 90 || float.Parse(testi.kulmay) < -90 || float.Parse(testi.kulmay) > 90) //kyljellään
	  {
		rotationText.enabled = true;
		rotationText.text = "REEEEEEEE";
            if(m_Play == false)
            {
                ree1.Play();
                m_Play = true;
            }
	  }
	  
	  else
	  {
		 rotationText.enabled = false;
            if(m_Play == true)
            {
                ree1.Stop();
                m_Play = false;
            }
	  }
	}
	
	void temperature () { //lampötilan vaikutus 
		
		if(float.Parse(testi.lampoTesti) < 15 && float.Parse(testi.lampoTesti) >= 10 )
		{
			temperatureText.enabled = true;
			temperatureText.text = "Onpa kylmä!";
		}
		
		else if(float.Parse(testi.lampoTesti) < 10 && float.Parse(testi.lampoTesti) >= 0)
		{
			temperatureText.enabled = true;
			temperatureText.text = "Väsyttää.. horrostila kutsuu";
			
			if(m_Play2 == true)
            {
                ree2.Stop();
                m_Play2 = false;
            }
		}
		
		else if(float.Parse(testi.lampoTesti) < 0 && float.Parse(testi.lampoTesti) >= -40)
		{
			temperatureText.enabled = true;
			temperatureText.text = "Kuollut";
			
			if(m_Play2 == false)
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
			temperatureText.text = "Voitko viedä minut varjoon?";
			
			if(m_Play2 == true)
            {
                ree2.Stop();
                m_Play2 = false;
            }

		}
		else if(float.Parse(testi.lampoTesti) > 60)
		{
			temperatureText.enabled = true;
			temperatureText.text = "Kuollut";
			
			if(m_Play2 == false)
            {
                ree2.Play();
                m_Play2 = true;
            }
		}
		else
		{
			temperatureText.enabled = false;
			
			 if(m_Play2 == true)
            {
                ree2.Stop();
                m_Play2 = false;
            }
		}
			
	}
}