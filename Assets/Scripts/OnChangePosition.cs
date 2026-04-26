using UnityEngine; // Dicionario da Unity

public class OnChangePosition : MonoBehaviour // Define o nome do script que esta sendo utilizado
                                              // é essencial para voce atribuir o script para dentro de um objeto no Unity
{
    // Arraste o objeto com o PolygonCollider2D para cá no Inspector
    public PolygonCollider2D hole2dColider;

    public PolygonCollider2D ground2dColider;

    public float initialScale = 0.5f;

    public Collider GroundColider;

    Mesh GenerateMesh;

    public MeshCollider GenerateMeshColider;

    
    
    private void Start()
    {
        GameObject[] AllGOs = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (var go in AllGOs)
        {
            if (go.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(), GenerateMeshColider, true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, GroundColider, true);
        Physics.IgnoreCollision(other, GenerateMeshColider, false);
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, GroundColider, false);
        Physics.IgnoreCollision(other, GroundColider, true);

    }

    // Diferente do Update comum, o FixedUpdate roda em intervalos de tempo fixo (geralmente 50 por segundo)
    // Sendo melhor para utilizar com colliders ou algo que envolva fisica.
    private void FixedUpdate()
    {
        // O unity tem uma variavel interna que se chama "has.changed". ela se torna true (verdadeira)

        if (transform.hasChanged)
        {
            // Importante: Reseta a flag para que o IF não rode infinitamente sem necessidade
            transform.hasChanged = false;

            if (hole2dColider != null)
            {
                // Criando o Vector2 que você deseja usar
                Vector2 novaPosicao2D = new Vector2(transform.position.x, transform.position.z);

                // O Unity aceita atribuir Vector2 ao position (que é Vector3)
                // Ele automaticamente define o Z como 0.
                hole2dColider.transform.position = novaPosicao2D;
                hole2dColider.transform.localScale = transform.localScale * initialScale;
                MakeHole2D();
                Make3DMeshColider();
            }
        }
    }

    private void MakeHole2D()
    {
        Vector2[] PointPositions = hole2dColider.GetPath(0);

        for (int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] =hole2dColider.transform.TransformPoint(PointPositions[i]);
        }

        ground2dColider.pathCount = 2;
        ground2dColider.SetPath(1, PointPositions);
    }
    private void Make3DMeshColider()
    {
        if (GenerateMesh != null) Destroy(GenerateMesh);
        {
            GenerateMesh = ground2dColider.CreateMesh(true, true, true);
            GenerateMeshColider.sharedMesh = GenerateMesh;

        }

    }
}