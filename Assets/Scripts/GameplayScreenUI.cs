using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayScreenUI : MonoBehaviour
{
    public void OnClick_TwoCardShow()
    {
        PowerUpManager.Instance.Activate(PowerUpType.ShowTwoCard);
    }

    public void OnClick_RevealAllCard()
    {
        PowerUpManager.Instance.Activate(PowerUpType.RevealAll);
    }
}
