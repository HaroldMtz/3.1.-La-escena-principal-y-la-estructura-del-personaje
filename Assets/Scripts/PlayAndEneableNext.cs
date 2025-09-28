using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayAndEnableNext : MonoBehaviour
{
    public string playTrigger = "Play";
    public Button nextButton;
    public float openDuration = 0.5f;  // pon aquí la duración real de Open.anim

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();  // lo toma del mismo InteractiveIcon
    }

    public void Run()
    {
        if (anim == null) { Debug.LogWarning("Sin Animator en InteractiveIcon"); return; }
        Debug.Log("[PlayAndEnableNext] Disparo trigger '" + playTrigger + "'");
        anim.ResetTrigger(playTrigger);
        anim.SetTrigger(playTrigger);

        if (nextButton) StartCoroutine(EnableAfter(openDuration));
    }

    IEnumerator EnableAfter(float t)
    {
        yield return new WaitForSeconds(t);
        if (nextButton) nextButton.interactable = true;
    }
}
