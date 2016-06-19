using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using BaldurSuchtFiona.Models;

namespace BaldurSuchtFiona
{
    public class MusicComponent : GameComponent
    {
        private Game1 game;

        private float totalFadeTime = 1500f;

        private float maxVolume;

        private Dictionary<string, SoundEffect> songs;

        private SoundEffectInstance currentSong = null;

        private SoundEffect currentEffect = null;

        private SoundEffect nextEffect = null;

        private SoundEffect areaEffect = null;

        private bool menu = false;

        public MusicComponent(Game1 game) : base(game)
        {
            this.game = game;
            maxVolume = 0.3f;

            songs = new Dictionary<string, SoundEffect>();
            songs.Add("generalGameSound", game.Content.Load<SoundEffect>("demo"));
        }

        public override void Update(GameTime gameTime)
        {
            if(game.World.Area != null)
                Play(game.World.Area.SongName);

            if (currentEffect == nextEffect)
                nextEffect = null;

            if (currentEffect != null && nextEffect != null)
            {
                float currentVolume = currentSong.Volume;
                currentVolume -= (float)gameTime.ElapsedGameTime.TotalMilliseconds / totalFadeTime;
                if (currentVolume <= 0f)
                {
                    currentSong.Volume = 0;
                    currentSong.Stop();
                    currentSong.Dispose();
                    currentSong = null;
                    currentEffect = null;
                }
                else
                {
                    currentSong.Volume = currentVolume;
                }
            }

            if (currentEffect == null && nextEffect != null)
            {
                currentEffect = nextEffect;
                nextEffect = null;

                currentSong = currentEffect.CreateInstance();
                currentSong.IsLooped = true;
                currentSong.Volume = 0f;
                currentSong.Play();
            }

            if (currentEffect != null && nextEffect == null && currentSong.Volume < maxVolume)
            {
                float currentVolume = currentSong.Volume;
                currentVolume += (float)gameTime.ElapsedGameTime.TotalMilliseconds / totalFadeTime;
                currentVolume = Math.Min(currentVolume, maxVolume);
                currentSong.Volume = currentVolume;
            }
        }

        private void Play(string song)
        {
            if (String.IsNullOrWhiteSpace (song))
                song = "generalGameSound";

            SoundEffect soundEffect;
            if (songs.TryGetValue(song, out soundEffect))
            {
                areaEffect = soundEffect;
                //todo false weg machen
                if (!menu && false)
                    nextEffect = soundEffect;
            }
        }

        public void OpenMenu()
        {
            nextEffect = songs["menu"];
            menu = true;
        }

        public void CloseMenu()
        {
            nextEffect = areaEffect;
            menu = false;
        }
    }
}

