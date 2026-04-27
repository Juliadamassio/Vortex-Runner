using UnityEngine;
using UnityEngine.EventSystems;

public class OnChangePosition : MonoBehaviour

    //Julia Damasio
    //Stefany Anne
    //Pietra Ruiz
    //Maria Souza
{
    //Configurações do Jogo
    public PolygonCollider2D hole2dColider; // É o desenho invisível que define o formato do buraco
    // O script usa esse desenho como um "molde" para saber onde o chão deve sumir

    public PolygonCollider2D ground2dColider; // Colisor 2D do chão para recorte
    //É o colisor que representa a área total do seu chão, recortando o espaço para as coisas caírem
    // exemplo os o "chao" que os quadrados vao cair

    public float initialScale = 0.5f; // O tamanho inicial do buraco
    // Aqui voce modfica o tamanho caso ache que esta muito grande ou muito pequeno 

    public float growthRate = 0.1f; // "Taxa de crescimento" 
    // Define quant o buraco expande a cada item que que ele engole

    //Referências de Colisão
    public Collider GroundColider; // O script precisa "desligar" a colisão dessa área específica
    // para que os objetos parem de bater no chão e comecem a cair dentro do buraco

    public MeshCollider GenerateMeshColider; // Cria fisica internas dentro do buraco
    //para que os objetos não fiquem flutuando no vazio.

    private Mesh GenerateMesh;
    //É a "malha" (o desenho 3D) invisível que o script cria na memória

    public void Move(BaseEventData myEvent)
    // Move o buraco utilizando o mouse (usado via EventTrigger no Unity)
    {
        if (((PointerEventData)myEvent).pointerCurrentRaycast.isValid)
        //É um modo de garantir que o buraco não "voe" para o infinito

        {
            transform.position = ((PointerEventData)myEvent).pointerCurrentRaycast.worldPosition;
            //É o que faz o buraco "grudar" no mouse
        }
    }

    private void Start()
    {
        //Configura todos os obstáculos da cena ao iniciar
        //O script faz uma varredura completa na cena e cria uma lista com todos os objetos que existem no seu jogo
        GameObject[] AllGOs = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (var go in AllGOs)
        {
            if (go.layer == LayerMask.NameToLayer("Obstacles"))
            //So vai se mexer nos objetos que marcamos com a Layer (camada) chamada "Obstacles"
            //O que for chão ou cenário fixo, nao vai se mover

            {
                // Impede que o objeto bata na malha interna do buraco antes de cair
                //Ignora a colisao dentro do buraco para nao ter riscos de algum objeto
                //Ficar preso la dentro  ou na borda
                Physics.IgnoreCollision(go.GetComponent<Collider>(), GenerateMeshColider, true);

                // Trava o objeto no ar (tira a física ativa)
                Rigidbody rb = go.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                    // Para "congelar" o objeto, ele ainda tem física, mas não cai e não se mexe sozinho

                    rb.useGravity = true;
                    //Mesmo estando congelado (isKinematic) quando o buraco passar por baixo dele,
                    //o script vai desligar o congelamento, o objeto vai cair direto no buraco
                }
            }
        }
    }

    // Detecta quando um bstáculo "cair" dentro do buraco
    private void OnTriggerEnter(Collider other)
    {
        // Se o objeto estiver na Layer "Obstacles", ele vai cair dentro do buraco
        //Para apenas os obstaculos cair no buraco
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            //Chama a função Eat (Comer)
            //É esse comando que vai tirar o "congelamento" do objeto para ele cair
            Eat(other.gameObject);
        }
    }

    // Lógica para fazer o objeto cair e o buraco crescer
    void Eat(GameObject obj)
    {
        // Faz o objeto ignorar o chão (para ele atravessar e cair)
        //(Passar por dentro do chao)
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), GroundColider, true);

        // Faz o objeto colidir com o interior do buraco
        Physics.IgnoreCollision(obj.GetComponent<Collider>(), GenerateMeshColider, false);

        // Ativa a física do objeto
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Permite que ele caia
            rb.WakeUp(); // Garantir que ele perceba a hora que o objeto precisa cair

            rb.AddForce(Vector3.down * 12f, ForceMode.Impulse); // Força extra para baixo
            //Para cair de um modo mais rapido e satisfatorio 

        }

        // Aumenta o tamanho do buraco conforme voce joga
        transform.localScale += new Vector3(growthRate, growthRate, growthRate);

        // Remove o objeto do jogo após o tempo de queda no caso (meio segundo)
        //"Deleta"
        Destroy(obj, 0.5f);

        //Escreve no console o tamanho que vai aumentando
        Debug.Log("Buraco aumentou para: " + transform.localScale.x);
    }

    private void OnTriggerExit(Collider other)
    //Sensor de Saida

    {
        //Para ter certeza que apenas os obstaculos sairam (camada e nome)
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            //Para o objeto voltar a interagir com a fisica do chao (false para ignorar)
            Physics.IgnoreCollision(other, GroundColider, false);
        }
    }


    private void FixedUpdate()
    //Diferente do Update comum, o FixedUpdate é usado para coisas que envolvem física
    //Ele roda em um ritmo fixo e constante

    {
        // Só atualiza a malha física se o buraco se moveu ou cresceu
        if (transform.hasChanged)
        {
            transform.hasChanged = false;
            //Reseta o sensor, esperando voce mexer novamente

            if (hole2dColider != null)
            {
                // Sincroniza a posição 2D do recorte com a posição 3D do buraco
                //A unity usa o "chão" geralmente com os eixos X e Z
                //O script pega esses valores e diz para o furo 2D onde ele deve estar
                Vector2 novaPosicao2D = new Vector2(transform.position.x, transform.position.z);
                hole2dColider.transform.position = novaPosicao2D;

                // Ajusta o tamanho do recorte 2D baseado no crescimento do buraco
                hole2dColider.transform.localScale = transform.localScale * initialScale;

                MakeHole2D();
                //Recorta o desenho do buraco no chao em 2d

                Make3DMeshColider();
                //Transforma esse recorte em um colisor 3d 
                //para que os objetos caiam
            }
        }
    }
    
    // Desenha o formato do buraco no plano 2D
    private void MakeHole2D()
    {
        Vector2[] PointPositions = hole2dColider.GetPath(0);
        //Anota a posicao de todos os pontos que formam esse desenho
        //Basicamente mapeando o contorno dele

        for (int i = 0; i < PointPositions.Length; i++)
        //Esse loop percorre ponto por ponto do contornox


        {
            PointPositions[i] = hole2dColider.transform.TransformPoint(PointPositions[i]);
            //converte a posição dos pontos (que estão "dentro" do objeto do buraco)
            //para a posição real no mapa do jogo
        }

        ground2dColider.pathCount = 2;
        //O chão tem um contorno externo e um buraco interno
        //Dois caminhos

        ground2dColider.SetPath(1, PointPositions);
        //entrega os pontos que mapeamos para esse segundo caminho do chão
    }


    // Transforma o desenho 2D em um colisor 3D real (vão)
    private void Make3DMeshColider()
    {
        if (GenerateMesh != null) Destroy(GenerateMesh);
        //Apaga o furo antigo para criar um novo
        //Como o buraco se move o tempo todo e tem seu tamanho alterado tbm
        //Precisa ir apagando as antigas para nao pesar o jogo

        GenerateMesh = ground2dColider.CreateMesh(true, true, true);
        // Usa o colisor 2D que utilizamos antes para 
        //transformar em uma malha 3D 
        //Os "truques" (true, true, true) os comandos dizem ao Unity para criar as bordas laterais
        //garantindo que o buraco tenha profundidade e não seja apenas um adesivo no chão

        GenerateMeshColider.sharedMesh = GenerateMesh;
        //Coloca a malha 3D que foi criada e add no campo de colisao
        //Para garantir que exista um buraco para os objetos cair
    }
}