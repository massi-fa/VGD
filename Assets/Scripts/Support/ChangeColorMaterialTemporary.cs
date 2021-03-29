using UnityEngine;
using Color = UnityEngine.Color;


public class ChangeColorMaterialTemporary : MonoBehaviour
{

    public Color newColor = Color.red;
    public float timeBeforeResetttingColor = 0.2f;
    
    // riferimento ai renderer dei figli
    private Renderer[] _currentRenderers;
    
    // copai dei materiali originali (primo materiale di ogni renderer)
    private Material[] _originalFirstMaterial;

    // Start is called before the first frame update
    private void Start()
    {
        // trova i renderers dei figli
        _currentRenderers = GetComponentsInChildren<Renderer>();
        
        // crea una copia del primo materiale di ogni figlio
        // e lo salva nel vettore
        _originalFirstMaterial = new Material[_currentRenderers.Length];
        for (var i = 0; i < _currentRenderers.Length; i++)
        {
            _originalFirstMaterial[i] = new Material(_currentRenderers[i].material);
        }
    }

    public void FlashColor()
    {
        // Cambia il colore di ogni renderer 
        // nel colore settato dall'editor (red di default)
        foreach (Renderer rend in _currentRenderers)
        {
            rend.material.color = newColor;
        }
        
        // Invoca il resetcolor dopo <timeBeforeResetttingColor> secondi
        Invoke(nameof(ResetColor), timeBeforeResetttingColor);
    }

    public void ResetColor()
    {
        // Resetta il colore di ogni renderer
        // nel colore originale
        for (var i = 0; i < _currentRenderers.Length; i++)
        {
            if(_currentRenderers[i].material.HasProperty("_Color"))
                _currentRenderers[i].material.color = _originalFirstMaterial[i].color;
        }
    }
}