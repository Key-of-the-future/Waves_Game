using UnityEngine;

public class BulletCounter : MonoBehaviour
{

    
    private int bullets = 10;
    public TextCtrl textBullets;
    private void OnCollisionEnter(Collision collision)
    {
    
        if (collision.gameObject.tag == "counter")
        {
            Destroy(collision.gameObject);
            bullets--;
            Debug.Log("Bullets: " + bullets);
            textBullets.UpdateBullets(bullets);
        }
    }
}