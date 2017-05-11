using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class AudioManager
  {
    static Song bgm;

    public static void PlayBgm(string name, float volume = 0.1f)
    {
      MediaPlayer.Play(AssetManager.GetSong(name));
      MediaPlayer.Volume = volume;
      MediaPlayer.IsRepeating = true;
    }
    public static void Resume()
    {
      MediaPlayer.Resume();
    }
    public static void Pause()
    {
      MediaPlayer.Pause();
    }
    public static void SetBgmVolume(float volume)
    {
      MediaPlayer.Volume = volume;
    }
    public static void SetMasterVolume(float volume)
    {
      SoundEffect.MasterVolume = volume;
    }
    public static SoundEffectInstance PlayExplosion(float volume = 1.0f)
    {
      string path = "explosion";
      int i = Utility.RandomInt(1, 5);
      path += i.ToString();
      SoundEffectInstance si = AssetManager.GetSfx(path).CreateInstance();
      si.Volume = volume;
      si.Play();
      return si;
    }
    public static SoundEffectInstance PlaySound(string path, float volume = 1.0f)
    {
      SoundEffectInstance si = AssetManager.GetSfx(path).CreateInstance();
      si.Volume = volume;
      si.Play();
      return si;
    }
  }
}
