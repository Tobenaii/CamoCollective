using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeWinText : MonoBehaviour
{
    [SerializeField]
    private Image m_winnerNumberImage;
    [SerializeField]
    private List<Sprite> m_winnerNumberSprites;

    public void SetWinner(int winner)
    {
        m_winnerNumberImage.sprite = m_winnerNumberSprites[winner - 1];
        m_winnerNumberImage.SetNativeSize();
    }
}
