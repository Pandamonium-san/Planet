﻿using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Planet
{
  public static class AssetManager
  {
    public static Texture2D Pixel { get; private set; }

    private static ContentManager Content;
    private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    private static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
    private static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
    private static Dictionary<string, Song> songs = new Dictionary<string, Song>();
    private static Dictionary<string, Effect> effects = new Dictionary<string, Effect>();

    public static void LoadContent(ContentManager content)
    {
      AssetManager.Content = content;
      Pixel = GetTexture("pixel");

      textures = LoadFolderContent<Texture2D>(content, "Textures");
      fonts = LoadFolderContent<SpriteFont>(content, "Fonts");
      soundEffects = LoadFolderContent<SoundEffect>(content, "SFX");
      songs = LoadFolderContent<Song>(content, "BGM");
    }
    public static Texture2D GetTexture(string name)
    {
      name = name.ToLower();
      if (!textures.ContainsKey(name))
        LoadTexture(name, name);
      return textures[name];
    }
    public static SpriteFont GetFont(string name)
    {
      name = name.ToLower();
      if (!fonts.ContainsKey(name))
        LoadFont(name, name);
      return fonts[name];
    }
    public static SoundEffect GetSfx(string name)
    {
      name = name.ToLower();
      if (!soundEffects.ContainsKey(name))
        LoadSfx(name, name);
      return soundEffects[name];
    }
    public static Song GetSong(string name)
    {
      name = name.ToLower();
      if (!songs.ContainsKey(name))
        LoadSong(name, name);
      return songs[name];
    }
    public static Effect GetEffect(string name)
    {
      name = name.ToLower();
      return effects[name];
    }

    private static void LoadTexture(string name, string path)
    {
      textures.Add(name, AssetManager.Content.Load<Texture2D>(@"Textures\" + path));
    }
    private static void LoadFont(string name, string path)
    {
      fonts.Add(name, AssetManager.Content.Load<SpriteFont>(@"Fonts\" + path));
    }
    private static void LoadEffect(string name, string path)
    {
      effects.Add(name, AssetManager.Content.Load<Effect>(@"Effects\" + path));
    }
    private static void LoadSfx(string name, string path)
    {
      soundEffects.Add(name, AssetManager.Content.Load<SoundEffect>(@"Sfx\" + path));
    }
    private static void LoadSong(string name, string path)
    {
      songs.Add(name, AssetManager.Content.Load<Song>(@"Bgm\" + path));
    }
    public static Dictionary<string, T> LoadFolderContent<T>(this ContentManager contentManager, string contentFolder)
    {
      DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "/" + contentFolder);
      if (!dir.Exists)
        throw new DirectoryNotFoundException();
      Dictionary<string, T> result = new Dictionary<string, T>();

      FileInfo[] files = dir.GetFiles("*.*");
      foreach (FileInfo file in files)
      {
        string key = Path.GetFileNameWithoutExtension(file.Name).ToLower();
        result[key] = contentManager.Load<T>(contentFolder + "/" + key);
      }
      return result;
    }
  }
}
