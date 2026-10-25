using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    public float velocidad = 3.5f;
    public int vida = 3;
    public int danio = 10;
    public float tiempoEntreGolpes = 1f;

    private NavMeshAgent agent;
    private Transform player;
    private float _proximoGolpe;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = velocidad;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (player != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
        }
    }

    // Al tocar al jugador le resta vida (con un pequeño cooldown)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && Time.time >= _proximoGolpe)
        {
            gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("attack");
            PlayerHealth salud = collision.gameObject.GetComponent<PlayerHealth>();
            if (salud != null) salud.RecibirDanio(danio);
            _proximoGolpe = Time.time + tiempoEntreGolpes;
            SetVelocidad(0f);
        }
    }

    public void SetVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
        if (agent != null) agent.speed = velocidad;
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;
        if (vida <= 0) Destroy(gameObject);
    }
}