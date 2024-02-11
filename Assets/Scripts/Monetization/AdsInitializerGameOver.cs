using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializerGameOver : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    [SerializeField] RewardedAdsButtonGameOver rewardedAdsButtonGameOver;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
#if UNITY_IOS
            _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
#elif UNITY_EDITOR
            _gameId = _androidGameId; //Only for testing the functionality in the Editor
#endif
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Debug.Log("are we here? initialized");
            Advertisement.Initialize(_gameId, _testMode, this);
        }
        else
        {
            Debug.Log("are we here? already initialized");
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete. (game over)");
        rewardedAdsButtonGameOver.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}