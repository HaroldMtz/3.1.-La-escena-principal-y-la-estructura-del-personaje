using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontControllerTMP : MonoBehaviour
{
    [Header("Textos a controlar")]
    public List<TMP_Text> targets = new();

    [Header("Fuentes (TMP Font Assets)")]
    public List<TMP_FontAsset> fonts = new();

    [Header("Colores a rotar")]
    public List<Color> colors = new()
    {
        Color.white, Color.black,
        new Color(1f, 0.85f, 0.31f), // dorado
        new Color(0f, 0.82f, 1f),    // cian
        new Color(1f, 0.25f, 0.5f)   // rosa
    };

    [Header("Configuración")]
    [Min(0.5f)] public float stepSize = 8f;

    int fontIndex = -1, colorIndex = -1;

    readonly Dictionary<TMP_Text, float>       baseSize  = new();
    readonly Dictionary<TMP_Text, Color>       baseColor = new();
    readonly Dictionary<TMP_Text, TMP_FontAsset> baseFont  = new();

    void Reset()
    {
        // Autorrellena con todos los TMP_Text bajo este objeto (útil al arrastrar el script)
        targets = new List<TMP_Text>(GetComponentsInChildren<TMP_Text>(true));
    }

    void Awake() => CacheDefaults();

    void CacheDefaults()
    {
        foreach (var t in targets)
        {
            if (!t) continue;
            t.enableAutoSizing = false;                    // permite controlar tamaño manual
            if (!baseSize.ContainsKey(t))  baseSize[t]  = t.fontSize;
            if (!baseColor.ContainsKey(t)) baseColor[t] = t.color;
            if (!baseFont.ContainsKey(t))  baseFont[t]  = t.font;
        }
    }

    // ---------------- Tamaño ----------------
    public void IncreaseSize() { foreach (var t in targets) if (t) t.fontSize += stepSize; }
    public void DecreaseSize() { foreach (var t in targets) if (t) t.fontSize = Mathf.Max(1f, t.fontSize - stepSize); }
    public void ResetSize()    { foreach (var t in targets) if (t && baseSize.TryGetValue(t, out var s)) t.fontSize = s; }

    // ---------------- Fuente ----------------
    public void NextFont()
    {
        if (fonts == null || fonts.Count == 0) return;
        fontIndex = (fontIndex + 1) % fonts.Count;
        ApplyFont(fonts[fontIndex]);
    }

    public void SetFontByIndex(int index)
    {
        if (fonts == null || fonts.Count == 0) return;
        index = Mathf.Clamp(index, 0, fonts.Count - 1);
        fontIndex = index;
        ApplyFont(fonts[fontIndex]);
    }

    public void ResetFont()
    {
        foreach (var t in targets)
            if (t && baseFont.TryGetValue(t, out var f)) t.font = f;
    }

    void ApplyFont(TMP_FontAsset asset)
    {
        if (!asset) return;
        foreach (var t in targets) if (t) t.font = asset;
    }

    // ---------------- Color ----------------
    public void NextColor()
    {
        if (colors == null || colors.Count == 0) return;
        colorIndex = (colorIndex + 1) % colors.Count;
        ApplyColor(colors[colorIndex]);
    }

    public void SetColor(Color c) => ApplyColor(c);

    public void ResetColor()
    {
        foreach (var t in targets)
            if (t && baseColor.TryGetValue(t, out var c)) ApplyColorTo(t, c);
    }

    void ApplyColor(Color c)
    {
        foreach (var t in targets) if (t) ApplyColorTo(t, c);
    }

    void ApplyColorTo(TMP_Text t, Color c)
    {
        t.enableVertexGradient = false;               // evita que el gradiente bloquee el color
        t.color = c;                                   // color del TMP
        var mat = t.fontMaterial;                      // instancia por-objeto
        if (mat && mat.HasProperty(ShaderUtilities.ID_FaceColor))
            mat.SetColor(ShaderUtilities.ID_FaceColor, c); // fuerza Face Color del shader TMP
    }
}
