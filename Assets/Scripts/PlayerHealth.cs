using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int corazonesMaximos = 10;
    public AudioSource sonidoDanio;      // sonido al recibir un golpe

    private int corazones;
    private bool muerto = false;

    void Start()
    {
        corazones = corazonesMaximos;
        ActualizarUI();
    }

    // los enemigos la llaman cuando golpean al jugador
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