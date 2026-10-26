using UnityEngine;
using UnityEngine.InputSystem;

public class MovementCtrl : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public GameObject camara;
    public float sensibilidad = 0.1f;
    public float velocidad = 10f;

    private InputActionMap _inputActionMap;
    private InputAction _move;
    private InputAction _look;
    private Rigidbody _rb;

    private float _pitch = 0f;          // cuanto esta inclinada la camara hacia arriba o abajo
    private float _pitchLimit = 80f;    // limite para que la camara no de la vuelta

    void Start()
    {
        _inputActionMap = inputActionAsset.FindActionMap("Player");
        _move = _inputActionMap.FindAction("Move");
        _look = _inputActionMap.FindAction("Look");
        _inputActionMap.Enable();

        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // si la partida termino, ya no se mueve la camara
        if (GameManager.Instance != null && GameManager.Instance.PartidaTerminada) return;

        Vector2 look = _look.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * look.x * sensibilidad);

        _pitch = _pitch - look.y * sensibilidad;
        _pitch = Mathf.Clamp(_pitch, -_pitchLimit, _pitchLimit);
        camara.transform.localRotation = Quaternion.Euler(_pitch, 0, 0);
    }

    void FixedUpdate()
    {
        Vector2 input = _move.ReadValue<Vector2>();

        Vector3 direccion = transform.right * input.x + transform.forward * input.y;

        // se cambia solo la velocidad horizontal; la vertical se deja para la gravedad y el salto
        Vector3 velocidadActual = _rb.linearVelocity;
        _rb.linearVelocity = new Vector3(direccion.x * velocidad, velocidadActual.y, direccion.z * velocidad);
    }
}