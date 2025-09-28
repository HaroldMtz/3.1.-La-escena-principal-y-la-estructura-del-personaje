using UnityEngine;

public class PageController : MonoBehaviour
{
    [SerializeField] GameObject[] pages;
    int current = 0;

    void Start() { Show(0); }          // arranca en Page1

    public void Next() { Show((current + 1) % pages.Length); }
    public void Prev() { Show((current - 1 + pages.Length) % pages.Length); }

    void Show(int i)
    {
        for (int k = 0; k < pages.Length; k++)
            pages[k].SetActive(k == i);
        current = i;
    }
}
