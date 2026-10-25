using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int vidaMaxima = 100;
    public TMP_Text textoVida;
    public TMP_Text textoMensaje;     // texto grande al centro, vacío al inicio

    private int vida;
    private bool muerto;

    void Start()
    {
        vida = vidaMaxima;
        textoMensaje.text = "";
        ActualizarUI();
    }

    public void RecibirDanio(int cantidad)
    {
        if (muerto) return;

        vida = Mathf.Max(vida - cantidad, 0);
        ActualizarUI();

        if (vida == 0) StartCoroutine(Morir());
    }

    void ActualizarUI()
    {
        textoVida.text = "Vida: " + vida;
    }

    IEnumerator Morir()
    {
        muerto = true;
        textoMensaje.text = "GAME OVER";
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}