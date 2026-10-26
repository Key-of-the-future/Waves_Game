using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float velocidad = 3.5f;
    public int danio = 1;                 // corazones que quita cada golpe
    public float distanciaAtaque = 2f;    // a que distancia empieza a atacar
    public float tiempoEntreGolpes = 1f;

    // nombres de los parametros del deste Animator
    public string parametroVelocidad = "Speed";
    public string triggerAtaque = "attack";
    public string triggerMuerte = "die";

    private NavMeshAgent agent;
    private Animator animator;
    private EnemyAudio sonidos;
    private Transform player;
    private PlayerHealth salud;
    private float proximoGolpe;
    private bool muerto = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = velocidad;

        animator = GetComponentInChildren<Animator>();
        sonidos = GetComponent<EnemyAudio>();

        // el enemigo nace en tiempo de ejecucion, asi que busca al jugador por su tag
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null)
        {
            player = jugador.transform;
            salud = jugador.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (muerto) return;
        if (player == null) return;
        if (!agent.isOnNavMesh) return;

        // si la partida termino, el enemigo se queda quieto
        if (GameManager.Instance != null && GameManager.Instance.PartidaTerminada)
        {
            agent.isStopped = true;
            animator.SetFloat(parametroVelocidad, 0f);
            if (sonidos != null) sonidos.Correr(false);
            return;
        }

        // distancia al jugador, sin contar la altura
        Vector3 diferencia = player.position - transform.position;
        diferencia.y = 0f;
        float distancia = diferencia.magnitude;

        if (distancia <= distanciaAtaque)
        {
            // esta cerca: se detiene y ataca cuando pase el tiempo entre golpes
            agent.isStopped = true;
            if (Time.time >= proximoGolpe)
            {
                Atacar();
            }
        }
        else
        {
            // esta lejos: sigue persiguiendo al jugador
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        // le dice al Animator que tan rapido se mueve (para la animacion de correr)
        float rapidez = agent.velocity.magnitude;
        animator.SetFloat(parametroVelocidad, rapidez);

        // sonido de correr solo mientras se mueve
        if (sonidos != null)
        {
            sonidos.Correr(!agent.isStopped && rapidez > 0.1f);
        }
    }

    void Atacar()
    {
        proximoGolpe = Time.time + tiempoEntreGolpes;

        animator.SetTrigger(triggerAtaque);
        if (sonidos != null) sonidos.Atacar();
        if (salud != null) salud.RecibirDanio(danio);
    }

    // EnemyHealth la llama cuando la vida llega a 0
    public void IniciarMuerte()
    {
        if (muerto) return;
        muerto = true;

        agent.enabled = false;   // deja de moverse
        if (sonidos != null) sonidos.Morir();
        animator.SetTrigger(triggerMuerte);
    }

    public void SetVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
        if (agent != null) agent.speed = velocidad;
    }
}