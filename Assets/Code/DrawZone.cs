using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawZone : MonoBehaviour
{
    public Vector3 offset = new Vector3(0.3f, 0, -0.001f);
    public List<GameObject> cards;

    private void Update()
    {
        if (GameManager.instance.draggingCard == null)
        {
            foreach (GameObject card in cards)
            {
                card.GetComponent<Card>().isFaceUp = true;
                card.GetComponent<BoxCollider2D>().enabled = false;
                card.GetComponent<Card>().isOnDrawZone = true;
            }
        }
        if (cards.Count > 0)
        {
            cards[cards.Count - 1].GetComponent<BoxCollider2D>().enabled = true;
            if (GameManager.instance.draggingCard != null && GameManager.instance.draggingCard == cards[cards.Count - 1])
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = -8;
                cards[cards.Count - 1].transform.position = mousePos;
            }
        }

    }
    public void Draw(int cardCount)
    {
        if (cardCount == 2)
        {
            cards[cards.Count - 1].GetComponent<Card>().targetPos = new Vector3(transform.position.x + offset.x * (cardCount - 0), transform.position.y, transform.position.z + offset.z * (cards.Count - 1));
            cards[cards.Count - 2].GetComponent<Card>().targetPos = new Vector3(transform.position.x + offset.x * (cardCount - 1), transform.position.y, transform.position.z + offset.z * (cards.Count - 2));
            cards[cards.Count - 1].transform.SetParent(transform);
            cards[cards.Count - 2].transform.SetParent(transform);
        }
        else if (cardCount == 1)
        {
            cards[cards.Count - 1].GetComponent<Card>().targetPos = new Vector3(transform.position.x + offset.x * 2, transform.position.y, transform.position.z + offset.z * (cards.Count - 1));
            cards[cards.Count - 1].transform.SetParent(transform);

        }

        else if (cardCount >= 3)
        {
            //cards[cards.Count - 3].GetComponent<Card>().targetPos = transform.position + offset * (cards.Count - 3);
            cards[cards.Count - 1].GetComponent<Card>().targetPos = new Vector3(transform.position.x + offset.x * (cardCount - 1), transform.position.y, transform.position.z + offset.z * (cards.Count - 1));
            cards[cards.Count - 2].GetComponent<Card>().targetPos = new Vector3(transform.position.x + offset.x * (cardCount - 2), transform.position.y, transform.position.z + offset.z * (cards.Count - 2));
            cards[cards.Count - 3].GetComponent<Card>().targetPos = new Vector3(transform.position.x + offset.x * (cardCount - 3), transform.position.y, transform.position.z + offset.z * (cards.Count - 3));
            cards[cards.Count - 1].transform.SetParent(transform);
            cards[cards.Count - 2].transform.SetParent(transform);
            cards[cards.Count - 3].transform.SetParent(transform);

        }
    }
}
