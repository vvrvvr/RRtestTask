using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Canvas canvas;

    private List<GameObject> cardsInHand = new List<GameObject>();
    private float cardMovingSpeed = 0.5f;
    private float cardAngle = 7f;
    private int currentCardIndex = 0;

    public static GameManager Singleton;

    private void Awake()
    {
        Singleton = this;
        PlaceCards();
    }

    private void PlaceCards()
    {
        int cardsAmount = Random.Range(4, 7);
        float totalDegree = (cardsAmount - 1) * cardAngle;
        float currentAngle = totalDegree / 2;
        for (int i = 0; i < cardsAmount; i++)
        {
            GameObject card = Instantiate(cardPrefab, canvas.GetComponent<Transform>());
            cardsInHand.Add(card);
            card.transform.rotation = Quaternion.Euler(0, 0, currentAngle);
            currentAngle -= cardAngle;
        }
    }
    
    public void RemoveCard(int index)
    {
        if (cardsInHand.Count <= 0)
            return;
        DOTween.Kill(cardsInHand[index].GetInstanceID());
        Destroy(cardsInHand[index]);
        cardsInHand.RemoveAt(index);
        currentCardIndex--;
        if (currentCardIndex < 0)
            currentCardIndex = 0;
        RepositionCards();
    }
 
    private void RepositionCards()
    {
        float cardsTotal = cardsInHand.Count;
        float totalDegree = (cardsTotal - 1) * cardAngle;
        float currentAngle = totalDegree / 2;
        for (int i = 0; i < cardsTotal; i++)
        {
            GameObject card = cardsInHand[i];
            if (cardsTotal == 1)
            {
                card.transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), cardMovingSpeed).SetId(card.GetInstanceID());
                return;
            }
            card.transform.DORotateQuaternion(Quaternion.Euler(0, 0, currentAngle), cardMovingSpeed).SetId(card.GetInstanceID());
            currentAngle -= cardAngle;
        }
    }

    public void ChangeCardValue()
    {
        Card card = cardsInHand[currentCardIndex].GetComponent<Card>();
        card.UpdateValues(currentCardIndex); 

        currentCardIndex++;
        if (currentCardIndex >= cardsInHand.Count)
            currentCardIndex = 0;
    }
}
