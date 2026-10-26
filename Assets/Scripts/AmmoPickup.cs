using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int balas = 10;         
    public AudioClip sonido;     

    void OnTriggerEnter(Collider other)
    {
        PlayerHealth jugador = other.GetComponentInParent<PlayerHealth>();
        if (jugador == null) return;

        // busca el arma del jugador y le suma las balas a la reserva
        Weapon arma = FindFirstObjectByType<Weapon>();
        if (arma != null)
        {
            arma.AgregarReserva(balas);
        }

        // reproduce el sonido en la posicion de la caja
        if (sonido != null)
        {
            AudioSource.PlayClipAtPoint(sonido, transform.position);
        }

        Destroy(gameObject);
    }
}