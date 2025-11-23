using UnityEngine;
using UnityEngine.Video;

public class VideoTransition : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadNextScene();
            Debug.Log("Loading next scene from video end");
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
