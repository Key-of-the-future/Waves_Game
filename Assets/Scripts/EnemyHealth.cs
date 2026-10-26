using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int vida = 3;
    public float tiempoMuerte = 2.5f;     // lo que dura la animacion de morir antes de desaparecer

    public GameObject municionPrefab;     // la caja de municion
    [Range(0f, 1f)] public float probDrop = 0.3f;   //probabilidad
    public int cantidadBalas = 10;        // balas que da la caja

    private Weapon arma;
    private bool muerto = false;

    void Start()
    {
        // busca el arma del jugador para saber si le quedan balas
        arma = FindFirstObjectByType<Weapon>();
    }

    void Update()
    {
        // si se cae del mapa, cuenta como eliminado para no trabar la partida
        if (!muerto && transform.position.y < -20f)
        {
            Morir();
        }
    }

    public void RecibirDanio(int cantidad)
    {
        if (muerto) return;

        vida = vida - cantidad;
        if (vida <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        muerto = true;

        IntentarSoltarMunicion();

        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }

        // avisa a EnemyAI para que reproduzca la animacion y el sonido de morir
        EnemyAI ia = GetComponent<EnemyAI>();
        if (ia != null)
        {
            ia.IniciarMuerte();
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemigoEliminado();
        }

        // el cuerpo desaparece cuando termina la animacion
        Destroy(gameObject, tiempoMuerte);
    }

    void IntentarSoltarMunicion()
    {
        if (municionPrefab == null) return;

        float probabilidad = probDrop;

        // si el jugador no tiene balas (cargador y reserva en cero), la probabilidad es 100%
        if (arma != null && arma.SinBalas())
        {
            probabilidad = 1f;
        }

        // Random.value da un numero entre 0 y 1; si es menor o igual a la probabilidad, suelta la caja
        if (Random.value <= probabilidad)
        {
            Vector3 posicion = transform.position + Vector3.up * 0.5f;
            GameObject caja = Instantiate(municionPrefab, posicion, Quaternion.identity);

            // le dice a la caja cuantas balas da
            AmmoPickup pickup = caja.GetComponent<AmmoPickup>();
            if (pickup != null)
            {
                pickup.balas = cantidadBalas;
            }
        }
    }
}