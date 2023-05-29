using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    public PositionOnField position;

    public enum PositionOnField
    {
        inHand,
        inMiddle,
        inBank,
        inDiscard,
        inDiscardDisplay,
        inDeck,
        inOppHand,
        inOppDiscard,
        inOppDiscardDisplay
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
        if (_player.GetComponent<TurnSystem>().isTurn)
        {
            if (_player.GetComponent<PlayerActionOnUser>().isScrappingDiscard|| _player.GetComponent<PlayerActionOnUser>().isScrappingHand || _player.GetComponent<PlayerActionOnUser>().isScrappingHandAndDeck)
            {
                _isDraggable = false;
            }
            else
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
                if (position == PositionOnField.inDiscardDisplay)
                {
                    _isDraggable = false;
            
                }
            }
        }
        else
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
                    if (eventData.pointerDrag.gameObject.GetComponent<DisplayCards>().originObject.scrapDeck)
                    {
                        if (_player.GetComponent<CardLocationManager>().discardCards.Count > 0)
                        {
                            _player.GetComponent<PlayerActionOnUser>().ScrapDeck(eventData.pointerDrag);
                            _player.GetComponent<PlayerActionOnUser>().isScrappingDiscard = true;
                            eventData.pointerDrag.SetActive(false);
                        }
                        else
                        {
                            _player.GetComponent<PlayerActionOnUser>().PlayCard(eventData.pointerDrag); 
                        }
                        
                    }
                    else if (eventData.pointerDrag.gameObject.GetComponent<DisplayCards>().originObject.scrapHand)
                    {
                        if (_player.GetComponent<CardLocationManager>().handCards.Count > 0)
                        {
                            _player.GetComponent<PlayerActionOnUser>().ScrapHand(eventData.pointerDrag);
                            _player.GetComponent<PlayerActionOnUser>().isScrappingHand = true;
                            eventData.pointerDrag.SetActive(false);
                        }
                        else
                        {
                            _player.GetComponent<PlayerActionOnUser>().PlayCard(eventData.pointerDrag); 
                        }
                    }
                    //Feature to add if enough time, because it could have many bugs
                    
                    /*else if (eventData.pointerDrag.gameObject.GetComponent<DisplayCards>().originObject.scrapDeckAndHand)
                    {
                        if (_player.GetComponent<CardLocationManager>().handCards.Count > 0 && _player.GetComponent<CardLocationManager>().discardCards.Count > 0 )
                        {
                            _player.GetComponent<PlayerActionOnUser>().ScrapHandAndDeck(eventData.pointerDrag);
                            _player.GetComponent<PlayerActionOnUser>().isScrappingHandAndDeck = true;
                            eventData.pointerDrag.SetActive(false);
                        }
                        else
                        {
                            _player.GetComponent<PlayerActionOnUser>().PlayCard(eventData.pointerDrag); 
                        }
                    }*/
                    else
                    {
                        _player.GetComponent<PlayerActionOnUser>().PlayCard(eventData.pointerDrag); 
                    }
                    
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_player.GetComponent<PlayerActionOnUser>().isScrappingDiscard)
        {
            if (position == PositionOnField.inDiscardDisplay)
            {
                _player.GetComponent<PlayerActionOnUser>().CardToScrap = eventData.pointerClick;
                _player.GetComponent<PlayerActionOnUser>().scrapNothing = false;
            }     
        }

        if (_player.GetComponent<PlayerActionOnUser>().isScrappingHand)
        {
            if (position == PositionOnField.inHand)
            {
                _player.GetComponent<PlayerActionOnUser>().CardToScrap = eventData.pointerClick;
                _player.GetComponent<PlayerActionOnUser>().scrapNothing = false;
            }     
        }
        if (_player.GetComponent<PlayerActionOnUser>().isScrappingHandAndDeck)
        {
            if (position is PositionOnField.inHand or PositionOnField.inDiscard)
            {
                _player.GetComponent<PlayerActionOnUser>().CardToScrap = eventData.pointerClick;
                _player.GetComponent<PlayerActionOnUser>().scrapNothing = false;
            }     
        }

        if (position is PositionOnField.inDiscard or PositionOnField.inOppDiscard)
        {
            _player.GetComponent<PlayerActionOnUser>().WatchDiscard();
        }

    }
}
