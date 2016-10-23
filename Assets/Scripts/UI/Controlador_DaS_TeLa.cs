using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controlador_DaS_TeLa : MonoBehaviour {

	
    public void CarregarTela(int numeroDaTela)
    {
        SceneManager.LoadScene(numeroDaTela);
    }

	public void CarregarTela(string nomeDaCena)
	{
		SceneManager.LoadScene(nomeDaCena);
	}

    public void ChamarLinkDoTCE()
    {
		Application.OpenURL("http://www4.tce.sp.gov.br/");
    }
    
}
