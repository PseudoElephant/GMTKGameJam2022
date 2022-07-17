using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class DiceDisplay : MonoBehaviour
{
    public GameObject diceObjectPlaceHolder;
    private Image _image;
    private Vector2 _originalScale;

    public TextMeshProUGUI text;

    public Sprite[] goodDice;

    public Sprite[] badDice;

    private Sprite[] spritesDice;
    private bool _isEvilDice;
    private bool _isDisplaying = false;
    private Queue<bool> _diceQueue;

    private void Awake()
    {
        _originalScale = diceObjectPlaceHolder.transform.localScale;
        _image = diceObjectPlaceHolder.GetComponent<Image>();
        LeanTween.scaleX(diceObjectPlaceHolder, 0f, 0f).setEaseInOutSine();
        LeanTween.scaleY(text.gameObject, 0f, 0f);
        _diceQueue = new Queue<bool>();
        
        RegisterEvents();
    }

    private void PauseAndShowCanvas(bool isEvil)
    {
        _isDisplaying = true;
        _isEvilDice = isEvil;
        spritesDice = (isEvil) ? badDice : goodDice;
        _image.sprite = spritesDice[0];
        
        LevelManager.BroadcastEvent(LevelManager.Event.DiceRoll);
        
        LeanTween.scaleX(diceObjectPlaceHolder, _originalScale.x, 0.7f).setEaseInOutSine().setOnComplete(FlipDice);
    }

    private void FlipDice()
    {
        int _finalSprite = 0;
        if (_isEvilDice)
        {
            Dice.BadDice dice = Dice.SetBadDice();
            _finalSprite = (int)dice % 6;
            text.text = Dice.GetBadDiceString(dice);
            
            text.outlineWidth = 0.1f;
            text.outlineColor = new Color32(255, 0, 68, 255);
        }
        else
        {
            Dice.GoodDice dice = Dice.SetGoodDice();
            _finalSprite = (int)dice % 6;
            text.text = Dice.GetGoodDiceString(dice);
            
            text.outlineWidth = 0.1f;
            text.outlineColor = new Color32(0, 149, 233, 255);
        }
        
        LeanTween.scaleX(diceObjectPlaceHolder, 0f, 0.3f).setEaseInOutSine().setOnComplete(() =>
        {
            _image.sprite = spritesDice[(_finalSprite + 2) % 6];
            LeanTween.scaleX(diceObjectPlaceHolder, _originalScale.x, 0.3f).setEaseInOutSine().setOnComplete(() =>
            {
                LeanTween.scaleX(diceObjectPlaceHolder, 0f, 0.3f).setEaseInOutSine().setOnComplete(() =>
                {
                    _image.sprite = spritesDice[(_finalSprite + 4) % 6];
                    LeanTween.scaleX(diceObjectPlaceHolder, _originalScale.x, 0.3f).setEaseInOutSine().setOnComplete(() =>
                    {
                        LeanTween.scaleX(diceObjectPlaceHolder, 0f, 0.3f).setEaseInOutSine().setOnComplete(() =>
                        {
                            _image.sprite = spritesDice[(_finalSprite + 5) % 6];
                            LeanTween.scaleX(diceObjectPlaceHolder, _originalScale.x, 0.3f).setEaseInOutSine().setOnComplete(() =>
                            {
                                LeanTween.scaleX(diceObjectPlaceHolder, 0f, 0.3f).setEaseInOutSine().setOnComplete(() =>
                                {
                                    _image.sprite = spritesDice[(_finalSprite) % 6];
                                    LeanTween.scaleY(text.gameObject, 1f, 0.3f).setEaseOutSine();
                                    LeanTween.scaleX(diceObjectPlaceHolder, _originalScale.x, 0.3f).setEaseInOutSine().setOnComplete(
                                        () =>
                                        {
                                            LeanTween.scale(diceObjectPlaceHolder,
                                                new Vector3(_originalScale.x, _originalScale.y, 1f) + (Vector3.one * 0.1f), 0.5f).setEaseInOutSine().setOnComplete(() =>
                                            {
                                                LeanTween.scale(diceObjectPlaceHolder, _originalScale, 0.3f)
                                                    .setEaseInOutSine().setOnComplete(HideDice);
                                            });
                                        });
                                });
                            });
                        });
                    });
                });
            });
        });
    }

    private void HideDice()
    {
        LeanTween.scaleY(text.gameObject, 0f, 0.3f).setEaseOutSine();
        LeanTween.scaleX(diceObjectPlaceHolder, 0f, 0.3f).setEaseInOutSine().setOnComplete(()=>
        {
            _isDisplaying = false;
            CheckQueue();
        });
    }
    
    public void TriggerGoodDice()
    {
        if (_isDisplaying)
        {
            _diceQueue.Enqueue(false);
            return;
        }
        
        PauseAndShowCanvas(false);
    }

    private void CheckQueue()
    {
        if (_diceQueue.Count == 0) return;
        
        if (_diceQueue.Dequeue())
        {
            TriggerBadDice();
        } else
        {
            TriggerGoodDice();
        }
    }
    public void TriggerBadDice()
    {
        if (_isDisplaying)
        {
            _diceQueue.Enqueue(true);
            return;
        }
        
        PauseAndShowCanvas(true);
    }

    private void RegisterEvents()
    {
        LevelManager.OnGoodDiceRoll += TriggerGoodDice;
        LevelManager.OnBadDiceRoll += TriggerBadDice;
    }
}
