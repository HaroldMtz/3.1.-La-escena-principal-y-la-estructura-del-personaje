using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] private GameObject iconsGrid;

    public void OnButtonOne()
    {
        if (iconsGrid) iconsGrid.SetActive(!iconsGrid.activeSelf);
    }

    public void OnButtonTwo()
    {
        var img = GetComponent<UnityEngine.UI.Image>();
        if (img) img.color = new Color(Random.value, Random.value, Random.value, 0.2f);
    }

    public void OnButtonThree_Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
