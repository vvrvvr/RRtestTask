using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Text cardName;
    [SerializeField] private Text description;
    [SerializeField] private Text manaCostText;
    [SerializeField] private Text healthText;
    [SerializeField] private Text attackText;

    private int health;
    private int mana;
    private int attack;
    private int cardIndexInHand;

    void Start()
    {
        health = Random.Range(1, 10);
        mana = Random.Range(1, 10);
        attack = Random.Range(1, 10);

        cardName.text = $"{gameObject.name}";
        description.text = $"card id: {this.GetInstanceID()}";
        manaCostText.text = $"{mana}";
        healthText.text = $"{health}";
        attackText.text = $"{attack}";
    }

    public void UpdateValues(int index)
    {
        cardIndexInHand = index;
        int newRandomValue = Random.Range(-2, 10);
        switch (Random.Range(0, 3)) // choose random card value: 0 - health, 1 - mana, 2 - attack
        {
            case 0:
                if(health != newRandomValue)
                    StartCoroutine(ChangeTextAndValue(health, newRandomValue, healthText, CheckHealth));
                break;
            case 1:
                if(mana != newRandomValue)
                    StartCoroutine(ChangeTextAndValue(mana, newRandomValue, manaCostText));
                break;
            case 2:
                if(attack != newRandomValue)
                    StartCoroutine(ChangeTextAndValue(attack, newRandomValue, attackText));
                break;
        }
    }

    private IEnumerator ChangeTextAndValue(int currentValue, int newValue, Text textField, System.Action callbackOnFinish = null)
    {
        #region change text
        if (currentValue < newValue)
        {
            while (currentValue != newValue)
            {
                yield return new WaitForSeconds(0.3f);
                textField.text = $"{++currentValue}";
            }
        }
        else if (currentValue > newValue)
        {
            while (currentValue != newValue)
            {
                yield return new WaitForSeconds(0.3f);
                textField.text = $"{--currentValue}";
            }
        }
        #endregion
        ChangeCardValue(currentValue, textField.name);
        yield return new WaitForSeconds(0.3f);
        callbackOnFinish?.Invoke();
    }

    private void CheckHealth()
    {
        if (health <= 0)
            GameManager.Singleton.RemoveCard(cardIndexInHand);
    }

    private void ChangeCardValue(int value, string fieldName)
    {
        switch (fieldName)
        {
            case "health":
                health = value;
                break;
            case "mana":
                mana = value;
                break;
            case "attack":
                attack = value;
                break;
        }
    }
}
