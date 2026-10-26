using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public AudioSource jumpSound;         
    public float fuerzaSalto = 1000f;       
    public float distanciaSuelo = 1.1f;    

    private InputAction jumpAction;
    private Rigidbody rb;

    private void OnEnable()
    {
        inputActionAsset.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActionAsset.FindActionMap("Player").Disable();
    }

    void Start()
    {
        jumpAction = inputActionAsset.FindActionMap("Player").FindAction("Jump");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // si la partida termino, ya no se salta
        if (GameManager.Instance != null && GameManager.Instance.PartidaTerminada) return;

        // solo salta si se presiono el boton Y hay suelo debajo
        if (jumpAction.WasPerformedThisFrame() && EstaEnElSuelo())
        {
            rb.AddForce(Vector3.up * fuerzaSalto);
            if (jumpSound != null)
            {
                jumpSound.Play();
            }
        }

        
        Debug.DrawRay(transform.position, Vector3.down * distanciaSuelo, Color.green);
    }

    bool EstaEnElSuelo()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanciaSuelo))
        {
            if (!hit.collider.isTrigger)
            {
                return true;
            }
        }
        return false;
    }
}