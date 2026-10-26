using UnityEngine;

public class GameManager : MonoBehaviour
{
    // "Instance" permite que otros scripts encuentren al GameManager
    // escribiendo GameManager.Instance, sin arrastrarlo en el Inspector
    public static GameManager Instance;

    public Spawner spawner;
    public float tiempoEntreOleadas = 10f;   // descanso entre oleadas (segundos)

    public AudioClip clipVictoria;
    public AudioClip clipDerrota;

    // estado de la partida (otros scripts lo leen)
    public int OleadaActual = 0;
    public int EnemigosVivos = 0;
    public int EnemigosEliminados = 0;
    public bool PartidaTerminada = false;

    private bool generando = false;          // true mientras el Spawner todavia esta creando enemigos
    private bool esperandoOleada = false;    // true durante el descanso entre oleadas
    private float momentoSiguienteOleada;    // hora (Time.time) a la que empieza la siguiente oleada
    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        ActualizarHUD();

        // la primera oleada empieza 1 segundo despues de comenzar el juego
        esperandoOleada = true;
        momentoSiguienteOleada = Time.time + 1f;
    }

    void Update()
    {
        if (PartidaTerminada) return;

        if (esperandoOleada)
        {
            float restante = momentoSiguienteOleada - Time.time;

            if (restante <= 0f)
            {
                // se acabo el descanso: empieza la oleada
                esperandoOleada = false;
                IniciarOleada();
            }
            else if (OleadaActual > 0 && UIManager.Instance != null)
            {
                // cuenta regresiva en pantalla (no se muestra antes de la primera oleada)
                UIManager.Instance.MostrarMensaje("Oleada " + OleadaActual + " superada\nSiguiente en " + Mathf.CeilToInt(restante), 0.3f);
            }
        }
    }

    public void IniciarOleada()
    {
        if (PartidaTerminada) return;

        OleadaActual++;
        generando = true;

        // le pedimos al Spawner que genere los enemigos de esta oleada
        int cantidad = spawner.EnemigosParaOleada(OleadaActual);
        spawner.GenerarOleada(cantidad);

        ActualizarHUD();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.MostrarMensaje("Oleada " + OleadaActual, 2f);
        }
    }

    // el Spawner la llama cada vez que aparece un enemigo
    public void EnemigoGenerado()
    {
        EnemigosVivos++;
        ActualizarHUD();
    }

    // el Spawner la llama cuando ya genero a todos los enemigos de la oleada
    public void FinGeneracion()
    {
        generando = false;
        RevisarFinDeOleada();
    }

    // EnemyHealth la llama cuando un enemigo muere
    public void EnemigoEliminado()
    {
        if (PartidaTerminada) return;

        EnemigosVivos--;
        if (EnemigosVivos < 0)
        {
            EnemigosVivos = 0;
        }
        EnemigosEliminados++;
        ActualizarHUD();

        RevisarFinDeOleada();
    }

    // la oleada termina solo si ya aparecieron todos Y no queda ninguno vivo
    void RevisarFinDeOleada()
    {
        if (PartidaTerminada) return;
        if (generando) return;
        if (esperandoOleada) return;
        if (EnemigosVivos > 0) return;

        if (OleadaActual >= spawner.totalOleadas)
        {
            Victoria();
        }
        else
        {
            // empieza el descanso antes de la siguiente oleada
            esperandoOleada = true;
            momentoSiguienteOleada = Time.time + tiempoEntreOleadas;
        }
    }

    public void Victoria()
    {
        FinDePartida(true);
    }

    public void Derrota()
    {
        FinDePartida(false);
    }

    void FinDePartida(bool victoria)
    {
        if (PartidaTerminada) return;
        PartidaTerminada = true;

        // sonido de victoria o de derrota
        if (audioSource != null)
        {
            if (victoria && clipVictoria != null)
            {
                audioSource.PlayOneShot(clipVictoria);
            }
            if (!victoria && clipDerrota != null)
            {
                audioSource.PlayOneShot(clipDerrota);
            }
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.MostrarPantallaFinal(victoria, OleadaActual, EnemigosEliminados);
        }
    }

    void ActualizarHUD()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ActualizarOleada(OleadaActual, spawner.totalOleadas, EnemigosVivos);
            UIManager.Instance.ActualizarEliminados(EnemigosEliminados);
        }
    }
}