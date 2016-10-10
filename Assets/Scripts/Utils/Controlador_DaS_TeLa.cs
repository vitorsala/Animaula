using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controlador_DaS_TeLa : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
    public void CarregarTela(int numeroDaTela)
    {
        SceneManager.LoadScene(numeroDaTela) ;
    }

	public void CarregarTela(string nomeDaCena)
	{
		SceneManager.LoadScene(nomeDaCena) ;
	}

    public void ChamarLinkDoTCE()
    {
        Application.OpenURL("www.tce.sp.gov.br");
    }
}
