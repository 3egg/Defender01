using System;
using System.Collections;
using System.Collections.Generic;
using constant;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    private Button _videoButton;
    public Sprite play;
    public Sprite pause;

    private void Awake()
    {
        _videoPlayer = GetComponentInChildren<VideoPlayer>();
        _videoButton = GetComponentInChildren<Button>();
    }

    public void playOrPausevideo()
    {
        if (_videoPlayer.isPlaying)
        {
            _videoButton.GetComponent<Image>().sprite = play;
            _videoPlayer.Pause();
        }
        else
        {
            _videoButton.GetComponent<Image>().sprite = pause;
            _videoPlayer.Play();
        }
    }
}