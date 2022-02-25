using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    public Image _healthBar;
    public Image _ammoAmountBar;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    public void UpdateHealthBar(int currentAmount, int maxAmount)
    {
        _healthBar.fillAmount = (float)currentAmount / (float)maxAmount;
    }

    public void UpdateAmmoAmountBar(int currentAmount, int maxAmount)
    {
        _ammoAmountBar.fillAmount = (float)currentAmount / (float)maxAmount;
    }
}
