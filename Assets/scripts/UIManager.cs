using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject resultPanel;
    [SerializeField] Text resultText;

    //HP
    [SerializeField]Text playerHeroHpTx,
                         enemyHeroHpTx;
    //cost
    [SerializeField]Text playerManaCostTx,
                         enemyManaCostTx;

    //éûä‘ä«óù
    [SerializeField] Text timeCountTx;

    public void ResultPanel()
    {
        resultPanel.SetActive(false);

    }
    public void ShowManaCost(int playerManaCost,int enemyManaCost)
    {
        playerManaCostTx.text = playerManaCost.ToString();
        enemyManaCostTx.text = enemyManaCost.ToString();
    }
    public void UpdateTime(int timeCount)
    {
        timeCountTx.text = timeCount.ToString();
    }

    public void ShowHeroHp(int playerHeroHp,int enemyHeroHp)
    {
        playerHeroHpTx.text = playerHeroHp.ToString();
        enemyHeroHpTx.text = enemyHeroHp.ToString();
    }

    public void ShowResultPanel(int HeroHp)
    {
        resultPanel.SetActive(true);
        if (HeroHp <= 0)
        {
            resultText.text = "LOSE";
        }
        else
        {
            resultText.text = "WIN";
        }
    }
}
