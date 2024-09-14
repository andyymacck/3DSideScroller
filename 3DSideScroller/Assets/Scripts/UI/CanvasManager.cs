using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Text textJumpCount;
    [SerializeField] private Text textIsGrounded;

    private void Start()
    {
        EventHub.Instance.Subcribe<JumpEvent>(UpdateJumpText);
        EventHub.Instance.Subcribe<IsGroundedEvent>(UpdateIsGroundedText);
    }

    private void UpdateJumpText(JumpEvent eventData)
    {
        textJumpCount.text = $"JUMPS:{eventData.JumpsCount}";
    }

    private void UpdateIsGroundedText(IsGroundedEvent eventData)
    {
        textIsGrounded.text = $"IS GROUNDED:{eventData.IsGrounded}";    
    }

    private void OnDestroy()
    {
        EventHub.Instance.UnSubcribe<JumpEvent>(UpdateJumpText);
        EventHub.Instance.UnSubcribe<IsGroundedEvent>(UpdateIsGroundedText);
    }
}