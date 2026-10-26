using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // igual que en el GameManager: asi cualquier script puede usar UIManager.Instance
    public static UIManager Instance;

    // HUD
    public Image[] iconosCorazon;     
    public TMP_Text textoMunicion;
    public TMP_Text textoOleada;
    public TMP_Text textoEliminados;
    public TMP_Text textoMensaje;      // oleadastxt

    // pantalla final
    public GameObject pantallaFinal;
    public TMP_Text textoResultado;
    public TMP_Text textoEstadisticas;
    public Button botonReiniciar;

    private float momentoBorrarMensaje;   

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;   // por si se reinicia desde la pantalla final
    }

    void Start()
    {
        pantallaFinal.SetActive(false);
        textoMensaje.text = "";

        botonReiniciar.onClick.AddListener(Reiniciar);
    }

    void Update()
    {
        if (textoMensaje.text != "" && Time.time >= momentoBorrarMensaje)
        {
            textoMensaje.text = "";
        }
    }

    public void ActualizarVida(int corazones, int corazonesMaximos)
    {
        for (int i = 0; i < iconosCorazon.Length; i++)
        {
            // oculta los corazones que sobren
            iconosCorazon[i].gameObject.SetActive(i < corazonesMaximos);

            if (i < corazones)
            {
                iconosCorazon[i].color = Color.white;
            }
            else
            {
                iconosCorazon[i].color = new Color(1f, 1f, 1f, 0.2f);
            }
        }
    }

    public void ActualizarMunicion(int cargador, int reserva)
    {
        textoMunicion.text = cargador + " / " + reserva;
    }

    public void ActualizarOleada(int oleada, int total, int restantes)
    {
        textoOleada.text = "Oleada: " + oleada + " / " + total + "\nQuedan: " + restantes;
    }

    public void ActualizarEliminados(int eliminados)
    {
        textoEliminados.text = "Eliminados: " + eliminados;
    }

    public void MostrarMensaje(string mensaje, float segundos)
    {
        textoMensaje.text = mensaje;
        momentoBorrarMensaje = Time.time + segundos;
    }

    public void MostrarPantallaFinal(bool victoria, int oleada, int eliminados)
    {
        textoMensaje.text = "";

        if (victoria)
        {
            textoResultado.text = "VICTORIA";
        }
        else
        {
            textoResultado.text = "DERROTA";
        }

        textoEstadisticas.text = "Oleada alcanzada: " + oleada + "\nEnemigos eliminados: " + eliminados;
        pantallaFinal.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        // vuelve a cargar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}