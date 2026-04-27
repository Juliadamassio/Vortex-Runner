using UnityEngine;
using UnityEngine.SceneManagement; // Biblioteca para trocar de cenas

public class MenuManager : MonoBehaviour
{
    // Função para o botão JOGAR
    public void Jogar()
    {
        Debug.Log("Carregando o jogo: SampleScene");

        // Carrega diretamente a SampleScene
        SceneManager.LoadScene("SampleScene");
    }

    // Função para o botão SAIR
    public void SairDoJogo()
    {
        Debug.Log("O jogador saiu do jogo!");

        // Fecha o aplicativo
        Application.Quit();
    }
}