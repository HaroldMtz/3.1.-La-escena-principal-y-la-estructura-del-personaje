using System.Threading.Tasks;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class FirebaseInitializer : MonoBehaviour
{
    public static string CurrentUid { get; private set; }
    public static bool IsReady { get; private set; }

    private async void Awake()
    {
        DontDestroyOnLoad(gameObject);
        await InitFirebase();
    }

    private async Task InitFirebase()
    {
        // Verifica e instala dependencias si hace falta
        var status = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (status != DependencyStatus.Available)
        {
            Debug.LogError($"[Firebase] Dependencias no disponibles: {status}");
            return;
        }

        // Inicia sesión anónima
        var auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser == null)
        {
            var cred = await auth.SignInAnonymouslyAsync();
            CurrentUid = cred.User.UserId;
        }
        else
        {
            CurrentUid = auth.CurrentUser.UserId;
        }

        IsReady = true;
        Debug.Log($"[Firebase] Listo. UID: {CurrentUid}");
    }
}
