using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float tiempoVida = 2f; 
    public int danio = 1;

    void Start()
    {
        // Ignorar al jugador y su arma para que no choque al salir
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider miCollider = GetComponent<Collider>();
            foreach (Collider c in player.GetComponentsInChildren<Collider>())
            {
                Physics.IgnoreCollision(miCollider, c);
            }
        }

        GetComponent<Rigidbody>().AddForce(transform.up * 1500f);
        Destroy(gameObject, tiempoVida);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyCtrl enemigo = collision.gameObject.GetComponentInParent<EnemyCtrl>();
            if (enemigo != null) enemigo.RecibirDanio(danio);
        }

        Destroy(gameObject);   // choque con enemigo o con cualquier objeto
    }
}