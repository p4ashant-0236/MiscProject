using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreenUI : MonoBehaviour
{
    [SerializeField] private Button startNewGameButton;
    [SerializeField] private Button loadGameButton;

    [SerializeField] private Button button_3x4;
    [SerializeField] private Button button_4x5;
    [SerializeField] private Button button_5x6;

    private UIManager uIManager;

    internal void PrepareDefaultState(UIManager uIManager)
    {
        this.uIManager = uIManager;
    }

    private void OnEnable()
    {
        startNewGameButton.gameObject.SetActive(true);
        loadGameButton.gameObject.SetActive(true);

        button_3x4.gameObject.SetActive(false);
        button_4x5.gameObject.SetActive(false);
        button_5x6.gameObject.SetActive(false);
    }

    public void OnClick_StartNewGame()
    {
        startNewGameButton.gameObject.SetActive(false);
        loadGameButton.gameObject.SetActive(false);

        button_3x4.gameObject.SetActive(true);
        button_4x5.gameObject.SetActive(true);
        button_5x6.gameObject.SetActive(true);
    }

    public void OnClick_3x4()
    {
        uIManager.StartNewGame(3,4);
        this.gameObject.SetActive(false);
    }

    public void OnClick_4x5()
    {
        uIManager.StartNewGame(4, 5);
        this.gameObject.SetActive(false);
    }

    public void OnClick_5x6()
    {
        uIManager.StartNewGame(5, 6);
        this.gameObject.SetActive(false);
    }
}
