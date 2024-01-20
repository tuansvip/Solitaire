using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Card : MonoBehaviour
{
    public enum Suit
    {
        Spades,
        Hearts,
        Clubs,
        Diamonds
    }
    public GameObject Back;
    public GameObject Face;
    public Collider2D lastCollider;
    public int cardValue;
    public Suit suit;
    public Sprite[] FaceSprite;
    public Vector3 targetPos;
    public int botIndex;

    public bool isFaceUp = false;
    public bool isOnTop = false;
    public bool isOnBottom = false;
    public bool isInDeck = false;
    public bool isOnDrawZone = false;
    public bool isDragging = false;
    public bool fromBot = false;
    public bool fromTop = false;

    private void Start()
    {
        targetPos = GameManager.instance.deck.transform.position;
    }
    private void Update()
    {
        if (!isFaceUp)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            GetComponent<BoxCollider2D>().enabled = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Top")
        {
            lastCollider = collision;
            Debug.Log("OnTop");
            isOnTop = true;
        }
        if (collision.CompareTag("Bot"))
        {
            lastCollider = collision;
            isOnBottom = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Top")
        {
            lastCollider = collision;
            isOnTop = true;
        }
        if (collision.CompareTag("Bot"))
        {
            lastCollider = collision;
            isOnBottom = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Top")
        {
            isOnTop = false;
        }
        if (collision.CompareTag("Bot"))
        {
            isOnBottom = false;
        }
    }

    private void MoveToTop()
    {
        if (fromBot)
        {
            GameManager.instance.bottom[botIndex].cards.RemoveAt(GameManager.instance.bottom[botIndex].cards.Count - 1);
            isOnBottom = false;
        }
        else if (isOnDrawZone)
        {
            GameManager.instance.drawZone.cards.Remove(gameObject);
            isOnDrawZone = false;
        }
        targetPos = lastCollider.transform.position + lastCollider.GetComponent<Top>().offSet * lastCollider.GetComponent<Top>().value;
        transform.SetParent(lastCollider.transform);
        lastCollider.GetComponent<Top>().cards.Add(gameObject);
        lastCollider.GetComponent<Top>().value++;
        lastCollider.GetComponent<Top>().suit = suit;
        targetPos = lastCollider.transform.position + lastCollider.GetComponent<Top>().offSet * lastCollider.GetComponent<Top>().value;

    }

    private void LateUpdate()
    {
        switch (suit)
        {
            case Suit.Spades:
                if (cardValue > 0)
                {
                    Face.GetComponent<SpriteRenderer>().sprite = FaceSprite[(cardValue - 1) * 4];
                }
                break;
            case Suit.Hearts:
                if (cardValue > 0)
                {
                    Face.GetComponent<SpriteRenderer>().sprite = FaceSprite[(cardValue - 1) * 4 + 3];
                }
                break;
            case Suit.Clubs:
                if (cardValue > 0)
                {
                    Face.GetComponent<SpriteRenderer>().sprite = FaceSprite[(cardValue - 1) * 4 + 1];
                }
                break;
            case Suit.Diamonds:
                if (cardValue > 0)
                {
                    Face.GetComponent<SpriteRenderer>().sprite = FaceSprite[(cardValue - 1) * 4 + 2];
                }
                break;
        }
        if (isFaceUp)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void OnMouseDown()
    {
        if (isFaceUp)
        {
            isDragging = true;
            GameManager.instance.draggingCard = gameObject;
        }
    }
    private void OnMouseUp()
    {
        GameManager.instance.draggingCard = null;
        if (lastCollider == null)
        {
            isDragging = false;
            return;
        }
        if (isOnTop)
        {
            
            if (cardValue == 1 && lastCollider.GetComponent<Top>().value == 0)
            {
                MoveToTop();
            } else if (suit == lastCollider.GetComponent<Top>().suit && cardValue == lastCollider.GetComponent<Top>().value + 1)
            {
                MoveToTop();
            }
            
        }
        if (isOnBottom)
        {
            if (cardValue == 13 && lastCollider.GetComponent<Bottom>().value == 14)
            {
                MoveToBot();
            }
            else if (cardValue == lastCollider.GetComponent<Bottom>().value - 1)
            {
                bool checkColor = false;
                switch (suit)
                {
                    case Suit.Spades:
                        if (lastCollider.GetComponent<Bottom>().suit == Suit.Diamonds || lastCollider.GetComponent<Bottom>().suit == Suit.Hearts)
                        {
                            checkColor = true;
                        }
                        break;
                    case Suit.Hearts:
                        if (lastCollider.GetComponent<Bottom>().suit == Suit.Clubs || lastCollider.GetComponent<Bottom>().suit == Suit.Spades)
                        {
                            checkColor = true;
                        }
                        break;
                    case Suit.Clubs:
                        if (lastCollider.GetComponent<Bottom>().suit == Suit.Diamonds || lastCollider.GetComponent<Bottom>().suit == Suit.Hearts)
                        {
                            checkColor = true;
                        }
                        break;
                    case Suit.Diamonds:
                        if (lastCollider.GetComponent<Bottom>().suit == Suit.Clubs || lastCollider.GetComponent<Bottom>().suit == Suit.Spades)
                        {
                            checkColor = true;
                        }
                        break;
                }
                if (checkColor)
                {
                    MoveToBot();
                }
            }
        }
        foreach (GameObject card in GameManager.instance.listDraggingBot)
        {
            card.GetComponent<Card>().isDragging = false;
        }
        GameManager.instance.listDraggingBot.Clear();
        isDragging = false;
    }

    private void MoveToBot()
    {
        if (fromTop)
        {
            gameObject.transform.GetComponentInParent<Top>().value--;
            gameObject.transform.GetComponentInParent<Top>().cards.Remove(gameObject);
            lastCollider.GetComponent<Bottom>().cards.Add(gameObject);
            botIndex = lastCollider.GetComponent<Bottom>().index;
            transform.SetParent(lastCollider.transform);
            isOnTop = false;
            fromTop = false;
        }
        else if (isOnDrawZone)
        {
            gameObject.transform.GetComponentInParent<DrawZone>().cards.Remove(gameObject);
            lastCollider.GetComponent<Bottom>().cards.Add(gameObject);
            botIndex = lastCollider.GetComponent<Bottom>().index;
            transform.SetParent(lastCollider.transform);
            isOnDrawZone = false;
        }
        else if (isOnBottom)
        {
            foreach (GameObject card in GameManager.instance.listDraggingBot)
            {
                GameManager.instance.bottom[card.GetComponent<Card>().botIndex].cards.Remove(card);
                lastCollider.GetComponent<Bottom>().cards.Add(card);
                card.GetComponent<Card>().targetPos = lastCollider.transform.position + lastCollider.GetComponent<Bottom>().offSet * (lastCollider.GetComponent<Bottom>().cards.Count - 1);
                card.GetComponent<Card>().botIndex = lastCollider.GetComponent<Bottom>().index;
                card.transform.SetParent(lastCollider.transform);
                card.GetComponent<Card>().isOnBottom = true;
                card.GetComponent<Card>().isDragging = false;
                
            }
        }
        targetPos = lastCollider.transform.position + lastCollider.GetComponent<Bottom>().offSet * (lastCollider.GetComponent<Bottom>().cards.Count - 1);
        transform.SetParent(lastCollider.transform);

    }

    private void FixedUpdate()
    {
        if (!isDragging)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.3f);
        }
    }
}
