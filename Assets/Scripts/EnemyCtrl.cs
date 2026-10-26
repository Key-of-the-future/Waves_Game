using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    public float velocidad = 3.5f;
    public int vida = 3;
    public int danio = 1;                    // corazones que quita por golpe
    public float distanciaAtaque = 2f;       // distancia minima para atacar
    public float tiempoEntreGolpes = 1f;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private PlayerHealth salud;
    private float _proximoGolpe;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = velocidad;
        animator = GetComponentInChildren<Animator>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            salud = p.GetComponent<PlayerHealth>();
        }

        if (player == null) Debug.LogWarning("EnemyCtrl: no hay ningun objeto con tag Player.");
        else if (salud == null) Debug.LogWarning("EnemyCtrl: el objeto Player no tiene PlayerHealth.");
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        Vector3 diferencia = player.position - transform.position;
        diferencia.y = 0f;

        if (diferencia.magnitude <= distanciaAtaque)
        {
            agent.isStopped = true;
            if (Time.time >= _proximoGolpe) Atacar();
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    void Atacar()
    {
        _proximoGolpe = Time.time + tiempoEntreGolpes;

        if (animator != null) animator.SetTrigger("attack");
        if (salud != null) salud.RecibirDanio(danio);
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