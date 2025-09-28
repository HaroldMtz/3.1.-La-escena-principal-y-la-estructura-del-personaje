using UnityEngine;

public class SpriteLooper : MonoBehaviour
{
    public enum LoopType { Loop, PingPong }
    public Sprite[] frames;
    [Range(1,60)] public int fps = 12;
    public LoopType loopType = LoopType.Loop;

    private SpriteRenderer sr;
    private int idx, dir = 1;
    private float t;

    void Awake(){
        sr = GetComponent<SpriteRenderer>();
        if (frames != null && frames.Length > 0) sr.sprite = frames[0];
    }

    void Update(){
        if (frames == null || frames.Length == 0) return;
        t += Time.deltaTime;
        float step = 1f / fps;
        while (t >= step){
            t -= step;
            if (loopType == LoopType.Loop){
                idx = (idx + 1) % frames.Length;
            } else {
                idx += dir;
                if (idx >= frames.Length){ dir = -1; idx = frames.Length - 2; }
                else if (idx < 0){ dir = 1; idx = 1; }
            }
            sr.sprite = frames[idx];
        }
    }
}
