using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    private Image _heartPrefab;
    private Sprite _fullHeartSprite;
    private Sprite _emptyHeartSprite;

    private List<Image> _hearts = new List<Image>();

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void OnInitMaxHearts(int maxHearts)
    {
        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(_heartPrefab, transform);
            newHeart.sprite = _fullHeartSprite;
            newHeart.color = Color.red;
            _hearts.Add(newHeart);
        }
    }

    private void OnHeartUpdate(int currentHealth)
    {
        for(int i = 0; i < _hearts.Count; i++)
        {
            if (i<currentHealth)
            {
                _hearts[i].gameObject.SetActive(false);
            }
        }
    }
}
