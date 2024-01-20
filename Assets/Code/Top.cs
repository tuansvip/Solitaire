using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Top : MonoBehaviour
{
    public int value;
    public Card.Suit suit;
    public Vector3 offSet;
    public List<GameObject> cards;
   
    private void Awake()
    {
        value = 0;
        suit = Card.Suit.Spades;
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.draggingCard != null && cards.Count > 0)
        {
            if (GameManager.instance.draggingCard == cards[cards.Count - 1]) {

                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = -8;
                cards[cards.Count - 1].transform.position = mousePos;
                cards[cards.Count - 1].GetComponent<Card>().isOnTop = false;
                cards[cards.Count - 1].GetComponent<Card>().fromTop = true;
            }
        }
    }
    private void Update()
    {
        if (cards.Count == 0)
        {
            value = 0;
            suit = Card.Suit.Spades;
        }
        else
        {
            value = cards[cards.Count - 1].GetComponent<Card>().cardValue;
            suit = cards[cards.Count - 1].GetComponent<Card>().suit;
        }
        foreach (GameObject card in cards)
        {
            card.GetComponent<Card>().targetPos = transform.position + offSet * cards.IndexOf(card);
            if (!card.GetComponent<Card>().isDragging)
            card.GetComponent<Card>().isOnTop = true;
            card.GetComponent<Card>().fromBot = false;
        }

    }
}
