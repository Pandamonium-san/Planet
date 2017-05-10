using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planet
{
  class AudioManager
  {

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
