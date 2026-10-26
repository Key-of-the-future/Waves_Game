using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public Transform camara;
    public float alcance = 100f;      // distancia maxima del disparo
    public int danio = 1;
    public float fireRate = 1f;       // segundos entre disparo y disparo

    public int tamanoCargador = 15;
    public int reservaMaxima = 60;
    public float tiempoRecarga = 3f;
    public bool restaurarReservaAlAgotarse = true;

    public AudioSource sonidoDisparo;
    public AudioSource sonidoClicVacio;
    public AudioSource sonidoRecarga;

    // las balas actuales
    public int balasCargador;
    public int balasReserva;

    private InputAction _fire;
    private InputAction _reload;
    private float _nextFireTime;
    private bool recargando = false;
    private float finRecarga;         // momento en que termina la recarga

    void Start()
    {
        InputActionMap mapa = inputActionAsset.FindActionMap("Player");
        _fire = mapa.FindAction("Attack");
        _reload = mapa.FindAction("Reload");
        mapa.Enable();

        balasCargador = tamanoCargador;
        balasReserva = reservaMaxima;
        ActualizarUI();
    }

    void Update()
    {
        // si la partida ya termino, el arma no hace nada
        if (GameManager.Instance != null && GameManager.Instance.PartidaTerminada)
        {
            return;
        }

        // si la reserva llega a 0, se restaura al maximo
        if (restaurarReservaAlAgotarse && balasReserva == 0)
        {
            balasReserva = reservaMaxima;
            ActualizarUI();
        }

        if (recargando && Time.time >= finRecarga)
        {
            int faltan = tamanoCargador - balasCargador;
            int pasan = faltan;
            if (balasReserva < faltan)
            {
                pasan = balasReserva;      
            }

            balasCargador = balasCargador + pasan;
            balasReserva = balasReserva - pasan;
            recargando = false;
            ActualizarUI();
        }

        if (_fire.IsPressed())
        {
            Disparar();
        }
        if (_reload.triggered)
        {
            Recargar();
        }
    }

    public void Disparar()
    {
        if (recargando) return;
        if (Time.time < _nextFireTime) return;

        _nextFireTime = Time.time + fireRate;

        // cargador vacio: solo suena el clic
        if (balasCargador <= 0)
        {
            sonidoClicVacio.Play();
            return;
        }

        balasCargador--;
        sonidoDisparo.Play();
        ActualizarUI();

        // el rayo sale del centro de la camara hacia donde mira
        RaycastHit hit;
        if (Physics.Raycast(camara.position, camara.forward, out hit, alcance))
        {
            // busca el script de vida en el objeto golpeado (o en su padre)
            EnemyHealth enemigo = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemigo != null)
            {
                enemigo.RecibirDanio(danio);
            }
        }
    }

    public void Recargar()
    {
        if (recargando) return;                          
        if (balasCargador >= tamanoCargador) return;     
        if (balasReserva <= 0) return;                  

        recargando = true;
        finRecarga = Time.time + tiempoRecarga;
        sonidoRecarga.Play();
    }

    public bool SinBalas()
    {
        return balasCargador == 0 && balasReserva == 0;
    }

    // la usa AmmoPickup al recoger una caja
    public void AgregarReserva(int cantidad)
    {
        balasReserva = balasReserva + cantidad;
        if (balasReserva > reservaMaxima)
        {
            balasReserva = reservaMaxima;
        }
        ActualizarUI();
    }

    public Vector3 PuntoDeMira()
    {
        RaycastHit hit;
        if (Physics.Raycast(camara.position, camara.forward, out hit, alcance))
        {
            return hit.point;
        }
        return camara.position + camara.forward * alcance;
    }

    void ActualizarUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ActualizarMunicion(balasCargador, balasReserva);
        }
    }
}