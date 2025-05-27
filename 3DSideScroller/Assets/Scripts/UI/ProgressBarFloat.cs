using UnityEngine;
using UnityEngine.UI;

public class ProgressBarFloat : MonoBehaviour
{
    [SerializeField] private Image m_barImage;

    public void UpdateBar(float fillAmount)
    {
        m_barImage.fillAmount = fillAmount;
    }
}
