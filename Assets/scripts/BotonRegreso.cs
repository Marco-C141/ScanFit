using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonRegreso : MonoBehaviour
{
    [Header("Nombre de la escena a la que vas a regresar")]
    public string escenaDestino;

    void Update()
    {
        // Detecta el gesto de "Atrás" en Android o la tecla Esc en PC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(escenaDestino);
        }
    }
}