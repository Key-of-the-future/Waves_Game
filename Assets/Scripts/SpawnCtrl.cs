using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemigoPrefab;
    public Transform[] puntosSpawn;
    public float intervalo = 3f;     
    public int maxEnemigos = 8; 

    void Start()
    {
        foreach (Transform punto in puntosSpawn)
        {
            StartCoroutine(SpawnEnPunto(punto));
        }
    }

    IEnumerator SpawnEnPunto(Transform punto)
    {
        // pequeño desfase para que no aparezcan todos al mismo tiempo
        yield return new WaitForSeconds(Random.Range(0f, intervalo));

        while (true)
        {
            // saber si ya son los maximos
            while (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemigos)
            {
                yield return new WaitForSeconds(0.5f);
            }

            GameObject enemigo = Instantiate(enemigoPrefab, punto.position, punto.rotation);

            // tiempecillo entre los spawns
            yield return new WaitUntil(() => enemigo == null);
            yield return new WaitForSeconds(intervalo);
        }
    }
}