using System.Collections;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] objetos;    // Add os obstaculos (Quadrados coloridos)
    public Transform[] spawnPoints; // Pontos espalhados pelo mapa para gerar novos "spawnPoints"
    public float intervalo = 1f;    // A cada 1 segundos spawna um quadrado novo (obstaculo)

    void Start()
    {
        //Ele foi recomendado pois não chama a função de um jeito comum
        //Ele usa o "StartCoroutine" porque permite que a função "pare no tempo" sem travar o jogo inteiro
        StartCoroutine(SpawnLoop());
    }

    //IEnumerator:é uma função que permite pausas
    IEnumerator SpawnLoop()
    {
        //while (true): cria um loop infinito
        //enquanto o jogo estiver rodando, tudo o que estiver dentro dessas chaves { }vai se repetir
        while (true)
        {
            //Essa linha foi recomendada pq se nao o unity tentaria criar infinitos objetos no mesmo milissegundo e o seu computador travaria
            yield return new WaitForSeconds(intervalo);

            // Escolhe um objeto aleatório da lista
            GameObject objetoAleatorio = objetos[Random.Range(0, objetos.Length)];

            // Escolhe um ponto aleatório do mapa para gera o spawn
            Transform ponto = spawnPoints[Random.Range(0, spawnPoints.Length)];

            //É comando que faz o objeto aparecer fisicamente no jogo
            Instantiate(objetoAleatorio, ponto.position, ponto.rotation);
        }
    }
}