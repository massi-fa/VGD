using UnityEngine;
using UnityEngine.UI;

public class GameCharacterHealthBarController : MonoBehaviour
{
    private float _health;
    private float _lerpTimer;
    private float _maxHealth;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    // Start is called before the first frame update    
    public void Start()
    {
		_maxHealth = GetComponent<ProtagonistStatistics>().maxHp;
        _health = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _health = Mathf.Clamp(_health,0, _maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI(){
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = _health / _maxHealth;
         
        if( fillB > hFraction) {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillB,hFraction,percentComplete);
        }

        if(fillF < hFraction){
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer/chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount =Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
    }
    public void TakeDamage(float damage){
        _health -=damage;
        _lerpTimer = 0f;
    }
    public void RestoreHealt(float healAmount){
        _health += healAmount;
        _lerpTimer = 0f;
    }
}
