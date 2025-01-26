using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _player1;

    [SerializeField]
    private TMP_Text _player2;

    private bool _isPlayer1Ready = false;
    private bool _isPlayer2Ready = false;

    void Update()
    {
        if(!_isPlayer2Ready && Input.anyKeyDown)
        {
            _isPlayer2Ready = true;
            _player2.text = "[Ready]";
            SoundController.Instance.PlaySound(SfxIdentifier.UiConfirm);
        }
        
        var gamepadButtonPressed = Gamepad.current != null && Gamepad.current.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic);

        if (!_isPlayer1Ready && gamepadButtonPressed)
        {
            _isPlayer1Ready = true;
            _player1.text = "[Ready]";
            SoundController.Instance.PlaySound(SfxIdentifier.UiConfirm);
        }

        if(_isPlayer1Ready && _isPlayer2Ready)
        {
            MusicBox.Instance.SwitchToGameplayMusic();
            SceneManager.LoadScene("Arena_01");
        }
    }
}
