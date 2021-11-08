using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text hpText;
    [SerializeField] Text atText;
    [SerializeField] Text costText;
    [SerializeField] Image iconImage;
    [SerializeField] GameObject selectablePanel;
    [SerializeField] GameObject shieldPanel;
    [SerializeField] GameObject maskPanel;
    [SerializeField] GameObject SpellPanel;

    public void SetCard(CardModel cardModel)
    {
        nameText.text = cardModel.name;
        hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
        costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;
        maskPanel.SetActive(!cardModel.isPlayerCard);
        
        if(cardModel.isPlayerCard&&cardModel.spell != SPELL.NONE)
        {
            SpellPanel.SetActive(true);
        }
        else
        {
            SpellPanel.SetActive(false);
        }

        if (cardModel.ability == ABILITY.SHIELD)
        {
            shieldPanel.SetActive(true);
            
        }
        
        else
        {
            shieldPanel.SetActive(false);
            
        }
        if(cardModel.spell != SPELL.NONE)
        {
            hpText.gameObject.SetActive(false);
            

        }
        
    }
    public void Show()
    {
        maskPanel.SetActive(false);
        
    }
    public void Refresh(CardModel cardModel)
    {
        hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
    }
    public void SetActiveSelectablePanel(bool flag)
    {
        selectablePanel.SetActive(flag);
    }

}
