using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    [SerializeField] private Image _heartPrefabImage;
    [SerializeField] private Sprite _fullHeartSprite;
    // private Sprite _emptyHeartSprite;

    private List<Image> _hearts = new List<Image>();

    private void OnEnable()
    {
        EventBus<HeartsPayload>.Subscribe("OnHeartUpdate", OnHeartUpdate);
    }

    private void OnDisable()
    {
        EventBus<HeartsPayload>.Unsubscribe("OnHeartUpdate", OnHeartUpdate);
    }

    private void Awake()
    {
        OnInitMaxHearts(3);
    }

    private void OnInitMaxHearts(int maxHearts)
    {
        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(_heartPrefabImage, transform);
            newHeart.sprite = _fullHeartSprite;
            newHeart.color = Color.red;
            _hearts.Add(newHeart);
        }
    }

    private void OnHeartUpdate(IEventPayload payload)
    {
        int _hearts_number = ((HeartsPayload)payload).hearts_number;
        for (int i = 0; i < _hearts.Count; i++)
        {
            if (i < _hearts_number)
            {
                _hearts[i].color = Color.red;
            }
            else
            {
                _hearts[i].color = Color.white;
            }
        }
    }
}
