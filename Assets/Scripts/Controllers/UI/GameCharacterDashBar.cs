using UnityEngine;
using UnityEngine.UI;

public class GameCharacterDashBar : MonoBehaviour
{
    private float _dash;
    private float _maxDash = 100;
    public Image frontDashBar;
    public Image backHealthBar;
    [HideInInspector]
    public float dashCountDown=3f;

    // Start is called before the first frame update    
    public void Start()
    {
        _maxDash = GetComponent<ProtagonistStatistics>().maxHp;
        _dash = _maxDash;
    }

    // Update is called once per frame
    private void Update()
    {
        _dash = Mathf.Clamp(_dash,0, _maxDash);
        UpdateDashUI();
    }

    
    private void UpdateDashUI(){
        var fillB = backHealthBar.fillAmount;
        var hFraction = _dash / _maxDash;

        if (!(fillB > hFraction)) return;
        
        var increment = Time.deltaTime / dashCountDown;
        _dash += increment;
        frontDashBar.fillAmount += increment;
    }

    public void ResetBar()
    {
        _dash = frontDashBar.fillAmount = 0f;
    }
}
