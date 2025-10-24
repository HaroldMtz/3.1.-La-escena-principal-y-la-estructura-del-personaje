using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;

namespace Backend
{
    [Serializable]
    public class PlayerData
    {
        public int totalCoins = 0;
        public int currentLevel = 1;
        public List<int> unlockedLevels = new();
        public long updatedAtUnix = 0;

        public static PlayerData Default()
        {
            return new PlayerData
            {
                totalCoins = 0,
                currentLevel = 1,
                unlockedLevels = new List<int> { 1 },
                updatedAtUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }
    }

    public class GameDataService : MonoBehaviour
    {
        public static GameDataService Instance { get; private set; }

        private FirebaseApp _app;
        private FirebaseAuth _auth;
        private FirebaseUser _user;
        private FirebaseFirestore _db;

        public PlayerData Data { get; private set; } = PlayerData.Default();

        private bool _initialized = false;
        private bool _initializing = false;

        private DocumentReference DocRef =>
            _db.Collection("users").Document(_user.UserId)
               .Collection("progress").Document("main");

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause && _initialized)
            {
                _ = SaveAsync(); 
            }
        }

        private void OnApplicationQuit()
        {
            if (_initialized)
            {
                _ = SaveAsync();
            }
        }

        public async Task Initialize()
        {
            if (_initialized || _initializing) return;
            _initializing = true;

            var deps = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (deps != DependencyStatus.Available)
            {
                _initializing = false;
                throw new Exception($"Firebase dependencies not available: {deps}");
            }

            _app  = FirebaseApp.DefaultInstance;
            _auth = FirebaseAuth.DefaultInstance;

            if (_auth.CurrentUser == null)
            {
                var result = await _auth.SignInAnonymouslyAsync();
                _user = result.User;
            }
            else
            {
                _user = _auth.CurrentUser;
            }

            _db = FirebaseFirestore.DefaultInstance;

            _initialized = true;
            _initializing = false;
        }

        public async Task<PlayerData> LoadAsync()
        {
            if (!_initialized) await Initialize();

            try
            {
                var snap = await DocRef.GetSnapshotAsync(Source.Server);
                if (snap.Exists)
                {
                    var dict = snap.ToDictionary();

                    Data = new PlayerData
                    {
                        totalCoins = dict.ContainsKey("totalCoins") ? Convert.ToInt32(dict["totalCoins"]) : 0,
                        currentLevel = dict.ContainsKey("currentLevel") ? Convert.ToInt32(dict["currentLevel"]) : 1,
                        unlockedLevels = dict.ContainsKey("unlockedLevels")
                            ? ((List<object>)dict["unlockedLevels"]).ConvertAll(o => Convert.ToInt32(o))
                            : new List<int> { 1 },
                        updatedAtUnix = dict.ContainsKey("updatedAtUnix")
                            ? Convert.ToInt64(dict["updatedAtUnix"])
                            : DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                    };
                }
                else
                {
                    Data = PlayerData.Default();
                    await SaveAsync(); 
                }

                return Data;
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameDataService] LoadAsync error: {e.Message}");
                Data ??= PlayerData.Default();
                return Data;
            }
        }

        public async Task SaveAsync()
        {
            if (!_initialized) await Initialize();

            try
            {
                Data.updatedAtUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                var payload = new Dictionary<string, object>
                {
                    { "totalCoins", Data.totalCoins },
                    { "currentLevel", Data.currentLevel },
                    { "unlockedLevels", Data.unlockedLevels },
                    { "updatedAtUnix", Data.updatedAtUnix }
                };

                await DocRef.SetAsync(payload, SetOptions.MergeAll);
            }
            catch (Exception e)
            {
                Debug.LogError($"[GameDataService] SaveAsync error: {e.Message}");
            }
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            Data.totalCoins += amount;
            _ = SaveAsync();
        }

        public void SetCurrentLevel(int levelIndex)
        {
            if (levelIndex < 1) levelIndex = 1;
            Data.currentLevel = levelIndex;
            if (!Data.unlockedLevels.Contains(levelIndex))
                Data.unlockedLevels.Add(levelIndex);
            _ = SaveAsync();
        }

        public bool IsLevelUnlocked(int levelIndex) =>
            Data.unlockedLevels != null && Data.unlockedLevels.Contains(levelIndex);

        public int GetTotalCoins() => Data.totalCoins;
        public int GetCurrentLevel() => Data.currentLevel;
    }
}
