using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    public AudioClip clipCorrer;     
    public AudioClip clipAtaque;
    public AudioClip clipMuerte;
    [Range(0f, 1f)] public float volumen = 1f;
    public float distanciaMaxima = 25f;    

    private AudioSource audioCorrer;       
    private AudioSource audioEfectos;      // reproduce el de ataque y el de muerte

    void Awake()
    {
        // AddComponent crea un Audio Source nuevo en este objeto
        audioCorrer = gameObject.AddComponent<AudioSource>();
        audioEfectos = gameObject.AddComponent<AudioSource>();

        ConfigurarAudio(audioCorrer, true);
        ConfigurarAudio(audioEfectos, false);

        audioCorrer.clip = clipCorrer;
    }

    // deja el Audio Source configurado
    void ConfigurarAudio(AudioSource fuente, bool repetir)
    {
        fuente.playOnAwake = false;
        fuente.loop = repetir;
        fuente.volume = volumen;
        fuente.spatialBlend = 1f;    
        fuente.rolloffMode = AudioRolloffMode.Linear;
        fuente.maxDistance = distanciaMaxima;
    }

    public void Correr(bool corriendo)
    {
        if (clipCorrer == null) return;

        if (corriendo && !audioCorrer.isPlaying)
        {
            audioCorrer.Play();
        }
        if (!corriendo && audioCorrer.isPlaying)
        {
            audioCorrer.Stop();
        }
    }

    public void Atacar()
    {
        if (clipAtaque != null)
        {
            audioEfectos.PlayOneShot(clipAtaque);
        }
    }

    public void Morir()
    {
        audioCorrer.Stop();

        if (clipMuerte != null)
        {
            audioEfectos.PlayOneShot(clipMuerte);
        }
    }
}