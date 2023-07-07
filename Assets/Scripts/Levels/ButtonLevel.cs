using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLevel : MonoBehaviour
{
    [SerializeField] private Sprite unlockSprite;
    [SerializeField] private Sprite lockSprite;
    private int _level = 0;
    private bool _isUnlock;
    private Button _button;
    private Image _image;

    private void OnEnable()
    {
        // if (player.activeSelf && !Menu.first)
        //     player.SetActive(false);
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }
    
    public void SetUp(int level, bool isUnlock)
    {
        this._level = level;
        if (isUnlock)
        {
            this._isUnlock = true;
            _image.sprite = unlockSprite;
            _button.enabled = true;
        }
        else
        {
            _image.sprite = lockSprite;
            _button.enabled = false;
        }
    }

    public void OnClick()
    {
        if (!_isUnlock) return;
        LevelManager.SetLevel(_level);
        LevelManager.SetNumOfHalves();
        SceneManager.LoadScene("Level" + _level);
    }

   
    
 
}
