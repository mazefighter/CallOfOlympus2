using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public PositionOnField position;

    public enum PositionOnField
    {
        inHand,
        inMiddle,
        inBank,
        inDiscard,
        inDeck,
        inOppHand,
        inOppDiscard
    }
    private bool _isDraggable = false;
    
    private MiddleDeck _middleDeck;
    private int _middleNumber;

    public GameObject _player;
    public Transform _returnParent;
    private RectTransform _rectTransform;
    void Start()
    {
        _middleDeck = GameObject.Find("MiddleDeck").GetComponent<MiddleDeck>();
        _rectTransform = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Transform parent = gameObject.transform.parent;
        if (position == PositionOnField.inMiddle)
        {
            
            if (gameObject.GetComponent<DisplayCards>().cost > _player.GetComponent<AtackAndGoldSum>().playerGold)
            {
                _isDraggable = false;
            }
            else
            {
                _isDraggable = true;
                _returnParent = parent;
                gameObject.transform.SetParent(GameObject.Find("CardDragCanvas").transform);
            }
        }

        if (position == PositionOnField.inOppHand)
        {
            _isDraggable = false;
        }

        if (position == PositionOnField.inOppDiscard)
        {
            _isDraggable = false;

        }
        if (position == PositionOnField.inHand)
        {
            _isDraggable = true;
            _returnParent = parent;
            gameObject.transform.SetParent(GameObject.Find("CardDragCanvas").transform);
        }

        if (position == PositionOnField.inBank)
        {
            _isDraggable = false;
        }

        if (position == PositionOnField.inDeck)
        {
            _isDraggable = false;
        }

        if (position == PositionOnField.inDiscard)
        {
            _isDraggable = false;
            
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isDraggable)
        {
            if (position == PositionOnField.inHand)
            {
                if (_rectTransform.anchoredPosition.y is < -575 and > -775)
                {
                    _player.GetComponent<PlayerActionOnUser>().PlayCard(eventData.pointerDrag);
                }
            }

            if (position==PositionOnField.inMiddle)
            {
                if (_rectTransform.anchoredPosition.y < -60)
                {
                    _player.GetComponent<PlayerActionOnUser>().SelectMiddleCard(eventData.pointerDrag);
                }
            }
            transform.SetParent(_returnParent);
            transform.position = _returnParent.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDraggable)
        {
            gameObject.transform.position = eventData.position;
        }
    }
}
