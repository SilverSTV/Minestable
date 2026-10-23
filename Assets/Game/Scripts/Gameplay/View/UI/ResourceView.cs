using System;
using Game.Scripts.Gameplay.PlayerResources;
using Game.Scripts.Gameplay.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Gameplay.View.UI
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private Image _icon;
        
        private ResourceType _resourceType;
        private int _amount;

        public ResourceType ResourceType => _resourceType;

        public void Init(ResourceDefinition definition)
        {
            _resourceType = definition.ResourceType;

            if (_icon is not null)
                _icon.sprite = definition.Icon;
            
            gameObject.SetActive(false);
        }

        public void SetAmount(int amount)
        {
            if (amount < 0)
                throw new ArgumentException(nameof(amount));

            _amount = amount;

            var visible = amount > 0;
            gameObject.SetActive(visible);
            
            if(!visible)
                return;

            if (_amountText is not null)
                _amountText.text = _amount.ToString();
        }
    }
}
