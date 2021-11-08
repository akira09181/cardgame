using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject resultPanel;

    [SerializeField] Text resultText;
    [SerializeField] Text playerHeroHpText;
    [SerializeField] Text enemyHeroHpText;
    [SerializeField] Text playerManaCostText;
    [SerializeField] Text enemyManaCostText;
    [SerializeField] Text timeCountText;

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);
    }
    public void ShowManaCost(int playerManaCost,int enemyManaCost)
    {
        playerManaCostText.text = playerManaCost.ToString();
        enemyManaCostText.text = enemyManaCost.ToString();
    }
    public void UpdateTime(int timeCount)
    {
        timeCountText.text = timeCount.ToString();
    }
    public void ShowHeroHP(int playerHeroHp,int enemyHeroHp)
    {
        playerHeroHpText.text = playerHeroHp.ToString();
        enemyHeroHpText.text = enemyHeroHp.ToString();
    }
    public void ShowResultPanel(int heroHp)
    {
        resultPanel.SetActive(true);
        if (heroHp <= 0)
        {
            resultText.text = "Death by disease";
        }
        else
        {
            resultText.text = "Disease Kill";
        }
    }

}
