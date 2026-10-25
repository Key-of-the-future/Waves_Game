using UnityEngine;
using UnityEngine.InputSystem;
public class CanonCtrl : MonoBehaviour
{
    public int capacity = 15;
    public int currentBullets = 00;
    public AudioSource BulletSound;
    public AudioSource ReloadSound;
    public Transform spawnPoint;
    public GameObject bullet;
    public InputActionAsset inputActionAsset;
    public float fireRate = 0.5f;
    public float reloadRate = 1f;
    public TMPro.TMP_Text bulletText;
    public TMPro.TMP_Text bulletTotalText;
    public Transform camara;       // arrastra la cámara del jugador
    public float alcance = 100f;

    private InputActionMap _inputActionMap;
    private InputAction _fire;
    private InputAction _reload;
    private float _nextFireTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputActionMap = inputActionAsset.FindActionMap("Player");
        _fire = _inputActionMap.FindAction("Attack");
        _reload = _inputActionMap.FindAction("Crouch");

        currentBullets = capacity;

         bulletText.text = currentBullets.ToString();
        bulletTotalText.text = capacity + " / " + currentBullets;
    }

    // Update is called once per frame
    void Update()
    {
        if (_fire.triggered && currentBullets > 0 && Time.time >= _nextFireTime)
        {
            Debug.Log("Fire");
            currentBullets--;
             bulletText.text = currentBullets.ToString();
             bulletTotalText.text = capacity + " / " + currentBullets;
           // Dirección hacia donde apunta la cámara
            Vector3 direccion = camara.forward;
            // Si la mira apunta a algo, la bala va hacia ese punto (ignorando al propio jugador)
            if (Physics.Raycast(camara.position, camara.forward, out RaycastHit hit, alcance)
            && !hit.collider.transform.IsChildOf(transform.root))
            {
                direccion = (hit.point - spawnPoint.position).normalized;
            }
            Instantiate(bullet, spawnPoint.position, Quaternion.FromToRotation(Vector3.up, direccion));
            BulletSound.Play();
            _nextFireTime = Time.time + fireRate;
        }
        if (_reload.triggered)
        {
            Debug.Log("Reload");
            currentBullets = capacity;
            bulletText.text = capacity + " / " + currentBullets;
            bulletTotalText.text = capacity + " / " + currentBullets;
            bulletText.text = currentBullets.ToString();
            _nextFireTime = Time.time + reloadRate;
            ReloadSound.Play();
        }
    }
}
