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
    [SerializeField] GameObject selectpanel,
                                shieldPnel;
    

    [SerializeField] Image iconImage;

    public void Show(CardModel cardModel)
    {
        nameText.text = cardModel.name;
        hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
        costText.text = cardModel.cost.ToString();
        iconImage.sprite = cardModel.icon;
        if(cardModel.ability == ABILITY.SHIELD)
        {
            shieldPnel.SetActive(true);
        }else
        {
            shieldPnel.SetActive(false);
        }
        if(cardModel.spell != SPELL.NONE)
        {
            hpText.gameObject.SetActive(false);
        }
    }
    public void Reflesh(CardModel cardModel)
    {
        hpText.text = cardModel.hp.ToString();
        atText.text = cardModel.at.ToString();
    }
    public void ActiveSelect(bool flag)
    {
        selectpanel.SetActive(flag);
    }

}
