using UnityEngine;

public static class SaveManager {
    const string X="PLAYER_SAVE_X", Y="PLAYER_SAVE_Y", HAS="PLAYER_HAS_SAVE";
    public static void SavePosition(Vector2 p){ PlayerPrefs.SetFloat(X,p.x); PlayerPrefs.SetFloat(Y,p.y); PlayerPrefs.SetInt(HAS,1); PlayerPrefs.Save(); }
    public static bool TryLoadPosition(out Vector2 p){
        if(PlayerPrefs.GetInt(HAS,0)==1){ p=new Vector2(PlayerPrefs.GetFloat(X),PlayerPrefs.GetFloat(Y)); return true; }
        p=Vector2.zero; return false;
    }
    public static void Clear(){ PlayerPrefs.DeleteKey(X); PlayerPrefs.DeleteKey(Y); PlayerPrefs.DeleteKey(HAS); PlayerPrefs.Save(); }
}
