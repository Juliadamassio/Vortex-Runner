using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
    // Para utilizar um sistema de contagem definido por nos de apenas 10 segundos para ser um jogo curto
{
    //Configurações de Tempo

    public float tempoRestante = 10f;
    public TextMeshProUGUI textoTempo;

    //O "private bool tempoAcabou = false" é usado para dizer que o tempo ainda nao acabou
    // Assim, se ele tentar gerar a cena do GameOver varias vezes seguidas nao vai ter como

    private bool tempoAcabou = false;

    void Update()
    {
        //Se o tempo for true (o tempo já acabou), ele para o código aqui mesmo
        if (tempoAcabou) return;

        //Marca o tempo restante
        if (tempoRestante > 0)
        {
            //é o tempo que passou entre um tempo e outro "Time.deltaTime"
            tempoRestante -= Time.deltaTime;

            //Chama a função para atualizar o tempo restante
            ExibirTempo(tempoRestante);
        }
        else
        {
            //Aparecer no console da unity para sabermos que esta funcionando
            Debug.Log("O TEMPO ACABOU!");

            //Para nao aparecer numeros negativos
            tempoRestante = 0;

            //Para garantir que o tempo acabou 
            tempoAcabou = true;

            //Ordem para mudar a cena 
            FinalizarJogo();
        }
    }

    void ExibirTempo(float tempoParaExibir)
    {
        // Formato simples de segundos para 10s

        //Pega o total de segundos e divide por 60 para descobrir os minutos
        //FloorInt arredonda para   tirar as virgulas
        float minutos = Mathf.FloorToInt(tempoParaExibir / 60);

        //O simbolo do modulo pega o que sobrou da divisao por 60
        float segundos = Mathf.FloorToInt(tempoParaExibir % 60);

        //Ele pega os minutos e os segundos e coloca no texto assim: "00:00" e se um numero for menor que 10 ele add um 0 na frente
        textoTempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void FinalizarJogo()
    {
        // Quando acabar o tempo o jogador sera direcionado para a cena de "GameOver"
        SceneManager.LoadScene("GameOver");
    }
}