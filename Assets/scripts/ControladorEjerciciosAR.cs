using UnityEngine;
using UnityEngine.Video;
using Vuforia;

public class ControladorEjerciciosAR : MonoBehaviour
{
    [Header("Configuración Principal")]
    public VideoPlayer reproductorVideo;
    public GameObject pantallaVideo; // El Quad que mostrará el video

    // Estructura para enlazar el nombre en la BD con su video
    [System.Serializable]
    public struct EjercicioMapping
    {
        public string nombreDelTarget; // Debe escribirse EXACTO como está en Vuforia
        public VideoClip clipInstruccion;
    }

    public EjercicioMapping[] videosPorEstacion;

    void Start()
    {
        // Asegurarnos de que la pantalla no se vea al iniciar si no hay target
        pantallaVideo.SetActive(false);

        // Buscar todos los ImageTargets en la escena automáticamente
        var targets = FindObjectsByType<ImageTargetBehaviour>(FindObjectsInactive.Exclude);
        
        foreach (var target in targets)
        {
            // Suscribirse al evento que avisa cuando la cámara encuentra o pierde una imagen
            target.OnTargetStatusChanged += OnStatusChanged;
        }
    }

    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        // Si la cámara detecta el target físico
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            MostrarVideo(behaviour.TargetName, behaviour.transform);
        }
        else
        {
            // Si el usuario aparta el celular, ocultamos la pantalla y pausamos
            if (behaviour.transform == pantallaVideo.transform.parent)
            {
                reproductorVideo.Pause();
                pantallaVideo.SetActive(false);
            }
        }
    }

    private void MostrarVideo(string nombreTarget, Transform padreTarget)
    {
        foreach (var mapa in videosPorEstacion)
        {
            if (mapa.nombreDelTarget == nombreTarget)
            {
                // 1. Teletransportar la pantalla para que sea "hija" del target actual
                pantallaVideo.transform.SetParent(padreTarget);
                
                // 2. Centrar la pantalla sobre la imagen (puedes ajustar el eje Y si quieres que flote)
                pantallaVideo.transform.localPosition = new Vector3(0, 0.1f, 0); 
                pantallaVideo.transform.localRotation = Quaternion.Euler(90, 0, 0); // Ajusta según tu Quad
                
                // 3. Reproducir
                pantallaVideo.SetActive(true);
                reproductorVideo.clip = mapa.clipInstruccion;
                reproductorVideo.Play();
                return;
            }
        }
    }
}