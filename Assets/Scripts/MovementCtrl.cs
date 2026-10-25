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

    private float _pitch = 0f;
    private float _pitchLimit = 80f;

    void Start()
    {
        _inputActionMap = inputActionAsset.FindActionMap("Player");
        _move = _inputActionMap.FindAction("Move");
        _look = _inputActionMap.FindAction("Look");
        _rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // MIRAR: horizontal gira al personaje, vertical solo la cámara
        Vector2 look = _look.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * look.x * sensibilidad);

        _pitch -= look.y * sensibilidad;
        _pitch = Mathf.Clamp(_pitch, -_pitchLimit, _pitchLimit);
        camara.transform.localRotation = Quaternion.Euler(_pitch, 0, 0);
    }

    void FixedUpdate()
    {
        // MOVER con la física, relativo hacia donde mira el personaje
        Vector2 input = _move.ReadValue<Vector2>();
        Vector3 direccion = transform.TransformDirection(new Vector3(input.x, 0, input.y)) * velocidad;

        // conserva la velocidad vertical (gravedad y salto)
        _rb.linearVelocity = new Vector3(direccion.x, _rb.linearVelocity.y, direccion.z);
    }
}