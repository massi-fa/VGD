using UnityEngine;
using Color = UnityEngine.Color;


public class ChangeColorMaterialTemporary : MonoBehaviour
{

    public Color newColor = Color.red;
    public float timeBeforeResetttingColor = 0.2f;
    
    // riferimento ai renderer dei figli
    private Renderer[] currentRenderers;
    
    // copai dei materiali originali (primo materiale di ogni renderer)
    private Material[] originalFirstMaterial;

    // Start is called before the first frame update
    void Start()
    {
        // trova i renderers dei figli
        currentRenderers = GetComponentsInChildren<Renderer>();
        
        // crea una copia del primo materiale di ogni figlio
        // e lo salva nel vettore
        originalFirstMaterial = new Material[currentRenderers.Length];
        for (int i = 0; i < currentRenderers.Length; i++)
        {
            originalFirstMaterial[i] = new Material(currentRenderers[i].material);
        }
    }

    public void FlashColor()
    {
        // Cambia il colore di ogni renderer 
        // nel colore settato dall'editor (red di default)
        foreach (Renderer rend in currentRenderers)
        {
            rend.material.color = newColor;
        }
        
        // Invoca il resetcolor dopo <timeBeforeResetttingColor> secondi
        Invoke("ResetColor", timeBeforeResetttingColor);
    }

    public void ResetColor()
    {
        // Resetta il colore di ogni renderer
        // nel colore originale
        for (int i = 0; i < currentRenderers.Length; i++)
        {
            currentRenderers[i].material.color = originalFirstMaterial[i].color;
        }
    }
}