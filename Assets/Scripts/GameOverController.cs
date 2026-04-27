using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGameOver : MonoBehaviour
{
    public void JogarNovamente()
    {
        // Carrega a cena principal do seu jogo (Vortex Runner)
        // Se a sua cena principal tiver outro nome, mude "SampleScene" para o nome correto
        SceneManager.LoadScene("SampleScene");
    }

    public void IrParaMenu()
    {
        // Se você tiver uma cena de menu inicial
        SceneManager.LoadScene("Menu");
    }
    public void SairDoJogo()
    {
        Debug.Log("O jogador saiu do jogo!");

        // Fecha o aplicativo
        Application.Quit();
    }
}
