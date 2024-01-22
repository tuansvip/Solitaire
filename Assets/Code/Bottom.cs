using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class Bottom : MonoBehaviour
{
    public List<GameObject> cards = new List<GameObject>();
    public Vector3 offSet;
    public int value;
    public Card.Suit suit;
    public int index;

    BoxCollider2D boxCollider2D;
    

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void Update()
    {
        if (cards.Count == 0)
        {
            value = 14;
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.draggingCard != null)
        {
            if (cards.IndexOf(GameManager.instance.draggingCard) >= 0)
            {
                GameManager.instance.listDraggingBot.Clear();
                GameManager.instance.listDraggingBot.Add(cards[cards.IndexOf(GameManager.instance.draggingCard)]);
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = -7;
                cards[cards.IndexOf(GameManager.instance.draggingCard)].transform.position =  mousePos; 
                for (int i = cards.IndexOf(GameManager.instance.draggingCard) + 1; i < cards.Count; i++)
                {
                    GameManager.instance.listDraggingBot.Add(cards[i]);
                    cards[i].GetComponent<Card>().isDragging = true;
                    cards[i].GetComponent<Card>().transform.position = cards[cards.IndexOf(GameManager.instance.draggingCard)].transform.position + offSet * (i - cards.IndexOf(GameManager.instance.draggingCard));
                }
            }
        }
        else
        {
            if (GameManager.instance.listDraggingBot.Count > 0)
            {
                for (int i = 0; i < GameManager.instance.listDraggingBot.Count; i++)
                {
                    GameManager.instance.listDraggingBot[i].GetComponent<Card>().isDragging = false;
                }
            }

            if (cards.Count > 0)
            {
                for (int i = 0; i < cards.Count; i++)
                {
                    cards[i].GetComponent<Card>().targetPos = transform.position + offSet * i;
                    cards[i].GetComponent<Card>().isOnBottom = true;
                    cards[i].GetComponent<Card>().botIndex = index;
                    cards[i].GetComponent<Card>().fromBot = true;
                }
                boxCollider2D.size = new Vector2(0.1987472f, cards[cards.Count - 1].GetComponent<BoxCollider2D>().size.y);

                boxCollider2D.offset = new Vector2(-0.01567078f, cards[cards.Count - 1].GetComponent<BoxCollider2D>().offset.y) - new Vector2(0, transform.position.y - cards[cards.Count - 1].transform.position.y);
            }
            if (cards.Count > 0 && !cards[cards.Count - 1].GetComponent<Card>().isFaceUp)
            {
                cards[cards.Count - 1].GetComponent<Card>().isFaceUp = true;
            }
            GameManager.instance.listDraggingBot.Clear();
        }
        if (cards.Count == 0)
        {
            value = 14;
            suit = Card.Suit.Spades;
        }
        else
        {
            value = cards[cards.Count - 1].GetComponent<Card>().cardValue;
            suit = cards[cards.Count - 1].GetComponent<Card>().suit;
        }
    }
}
