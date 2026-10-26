using UnityEngine;

public class PistolLine : MonoBehaviour
{
    public Transform lineOrigin;       // la punta del canon
    public LineRenderer lineRenderer;
    public Weapon arma;

    void Update()
    {
        lineRenderer.SetPosition(0, lineOrigin.position);
        lineRenderer.SetPosition(1, arma.PuntoDeMira());   // termina donde pega el disparo
    }
}