using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemigoPrefab;                         // enemigo normal
    public GameObject enemigoDebilPrefab;                    // segundo tipo de enemigo
    [Range(0f, 1f)] public float probEnemigoDebil = 0.3f;   // 0.3 = 30% de que sea el debil

    public Transform[] puntosSpawn;                          // puntos de aparicion

    public int totalOleadas = 4;
    public int enemigosIniciales = 3;
    public int incrementoPorOleada = 2;
    public float intervaloEntreEnemigos = 0.5f;              // segundos entre un enemigo y otro

    private int enemigosPendientes = 0;   // cuantos faltan por aparecer en la oleada actual
    private float proximoSpawn;        
    
    // cuantos enemigos tiene una oleada (la 1 tiene los iniciales y cada una suma el incremento)
    public int EnemigosParaOleada(int oleada)
    {
        return enemigosIniciales + incrementoPorOleada * (oleada - 1);
    }

    // el GameManager la llama al empezar cada oleada
    public void GenerarOleada(int cantidad)
    {
        if (enemigoPrefab == null)
        {
            Debug.LogError("Spawner: falta asignar el enemigoPrefab.");
            return;
        }
        if (puntosSpawn.Length == 0)
        {
            Debug.LogError("Spawner: no hay puntos de aparicion.");
            return;
        }

        enemigosPendientes = cantidad;
        proximoSpawn = Time.time;     // el primero aparece de inmediato
    }

    void Update()
    {
        // si la partida termino, ya no aparecen mas enemigos
        if (GameManager.Instance != null && GameManager.Instance.PartidaTerminada) return;

        // si no hay enemigos por generar, no hace nada
        if (enemigosPendientes <= 0) return;

        // todavia no es hora del siguiente enemigo
        if (Time.time < proximoSpawn) return;

        // elige si este enemigo es el normal o el debil
        GameObject prefab = enemigoPrefab;
        if (enemigoDebilPrefab != null && Random.value < probEnemigoDebil)
        {
            prefab = enemigoDebilPrefab;
        }

        // elige un punto de aparicion al azar (el numero maximo de Random.Range no se incluye)
        int indice = Random.Range(0, puntosSpawn.Length);
        Transform punto = puntosSpawn[indice];

        Instantiate(prefab, punto.position, punto.rotation);

        // le avisa al GameManager que aparecio un enemigo
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EnemigoGenerado();
        }

        enemigosPendientes--;
        proximoSpawn = Time.time + intervaloEntreEnemigos;

        // si ya aparecieron todos los de la oleada, se lo avisa al GameManager
        if (enemigosPendientes == 0 && GameManager.Instance != null)
        {
            GameManager.Instance.FinGeneracion();
        }
    }
}