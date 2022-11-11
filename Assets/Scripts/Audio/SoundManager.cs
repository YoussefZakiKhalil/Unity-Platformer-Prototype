using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{

    //https://docs.unity3d.com/2019.3/Documentation/Manual/class-AudioSource.html

    //public AudioClip collect; //ok
    //public AudioClip win;
    //public AudioClip jump;
    //public AudioClip hitEnemy;
    //public AudioClip hitPlayer;
    //public AudioClip alert;
    //public AudioClip miss;

    List<AudioClip> clips = new List<AudioClip>();

    private GameObject soundGameObject = null;
    public AudioSource audioSource = null;

    public enum Sound
    {
        Collect,
        Win,
        Jump,
        HitEnemy,
        HitPlayer,
        Alert,
        Miss,
        Bump,
        Phase2,
    }

    public void Init()
    {

        if (soundGameObject == null)
        {

            soundGameObject = new GameObject("Sound");
            audioSource = soundGameObject.AddComponent<AudioSource>();

            clips.Add(Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-33")); //colltect
            clips.Add(Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-18"));//win
            clips.Add(Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-07"));//jump
            clips.Add(Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-21"));//hitemeny
            clips.Add(Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-10"));//hitplayer
            clips.Add(Resources.Load<AudioClip>("ENGALRT"));//alert
            clips.Add(Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-25"));//miss
            clips.Add(Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-03"));//bump
            clips.Add(Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-09"));//phase2

            //Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-03");//bump
            //collect = Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-33"); //colltect
            //win = Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-18");//win
            //jump = Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-41");//jump
            //hitEnemy = Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-21");//hitemeny
            //hitPlayer = Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-10");//hitplayer
            //alert = Resources.Load<AudioClip>("ENGALRT");//alert
            //miss = Resources.Load<AudioClip>("CasualGameSounds/DM-CGS-25");//miss
            

        }

    }

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(clips[(int)sound]);
        //if (sound == Sound.Collect)
        //{
        //    audioSource.PlayOneShot(collect);
        //}
        //else if (sound == Sound.Win)
        //{
        //    audioSource.PlayOneShot(win);
        //}
        //else if (sound == Sound.Jump)
        //{
        //    audioSource.PlayOneShot(jump);
        //}
        //else if (sound == Sound.HitEnemy)
        //{
        //    audioSource.PlayOneShot(hitEnemy);
        //}
        //else if (sound == Sound.HitPlayer)
        //{
        //    audioSource.PlayOneShot(hitPlayer);
        //}
        //else if (sound == Sound.Alert)
        //{
        //    audioSource.PlayOneShot(alert);
        //}
        //else if (sound == Sound.Miss)
        //{
        //    audioSource.PlayOneShot(miss);
        //}
        //else
        //{
        //    Debug.Log("SoundManager: unknown sound");
        //}

    }


}
