using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int corazonesMaximos = 10;
    public AudioSource sonidoDanio;    

    private string tagLava = "Lava";         
    private float alturaMinima = -20f;      

    private int corazones;
    private bool muerto = false;

    void Start()
    {
        corazones = corazonesMaximos;
        ActualizarUI();
    }

    void Update()
    {
        // si se cae del mapa, muere
        if (!muerto && transform.position.y < alturaMinima)
        {
            MatarDeGolpe();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagLava))
        {
            MatarDeGolpe();
        }
    }

   
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagLava))
        {
            MatarDeGolpe();
        }
    }
    
    public void RecibirDanio(int cantidad)
    {
        if (muerto) return;

        corazones = corazones - cantidad;
        if (corazones < 0)
        {
            corazones = 0;
        }
        ActualizarUI();

        if (sonidoDanio != null)
        {
            sonidoDanio.Play();
        }

        if (corazones == 0)
        {
            Morir();
        }
    }

    // muerte inmediata, sin importar cuantos corazones queden
    public void MatarDeGolpe()
    {
        if (muerto) return;

        corazones = 0;
        ActualizarUI();

        if (sonidoDanio != null)
        {
            sonidoDanio.Play();
        }

        Morir();
    }

    void ActualizarUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ActualizarVida(corazones, corazonesMaximos);
        }
    }

    void Morir()
    {
        muerto = true;

        // le avisa al GameManager que se perdio la partida
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Derrota();
        }
    }
}