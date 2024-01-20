using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<GameObject> cards;
    public Vector3 cardOffset;
    public bool isClicked = false;

    private void Update()
    {

        foreach (GameObject card in cards)
        {
            card.GetComponent<Card>().isFaceUp = false;
        }
    }
    public void setOffset()
    {

        foreach (GameObject card in cards)
        {
            card.GetComponent<Card>().targetPos = transform.position + cardOffset * cards.IndexOf(card);
        }
    }

    private void OnMouseDown()
    {
        isClicked = true;
        if (cards.Count >= 3)
        {
            GameManager.instance.drawZone.cards.Add(cards[cards.Count - 1]);
            GameManager.instance.drawZone.cards.Add(cards[cards.Count - 2]);
            GameManager.instance.drawZone.cards.Add(cards[cards.Count - 3]);
            GameManager.instance.drawZone.Draw(3);
            cards.RemoveAt(cards.Count - 1);
            cards.RemoveAt(cards.Count - 1);
            cards.RemoveAt(cards.Count - 1);

        }
        else if (cards.Count == 2)
        {
            GameManager.instance.drawZone.cards.Add(cards[cards.Count - 1]);
            GameManager.instance.drawZone.cards.Add(cards[cards.Count - 2]);
            GameManager.instance.drawZone.Draw(2);
            cards.RemoveAt(cards.Count - 1);
            cards.RemoveAt(cards.Count - 1);
        }
        else if (cards.Count == 1)
        {
            GameManager.instance.drawZone.cards.Add(cards[cards.Count - 1]);
            GameManager.instance.drawZone.Draw(1);
            cards.RemoveAt(cards.Count - 1);

        }
        else if (cards.Count == 0)
        {
            if (GameManager.instance.drawZone.cards.Count > 0)
            {
                GameManager.instance.drawZone.cards.Reverse();
                cards.AddRange(GameManager.instance.drawZone.cards);
                foreach(GameObject card in cards) card.transform.SetParent(transform);
                GameManager.instance.drawZone.cards.Clear();
                setOffset();
            }
        }
    }
    private void OnMouseUp()
    {
        isClicked = false;
    }
}
