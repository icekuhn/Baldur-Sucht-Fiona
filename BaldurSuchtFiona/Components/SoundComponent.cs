using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;

namespace BaldurSuchtFiona
{
    public class SoundComponent : GameComponent
    {
        private Game1 game;

        private Dictionary<string, SoundEffect> sounds;

        private float volume;

        public SoundComponent(Game1 game) : base(game)
        {
            this.game = game;
            volume = 0.5f;

            sounds = new Dictionary<string, SoundEffect>();
//            sounds.Add("", game.Content.Load<SoundEffect>("click"));
        }

        private void Play(string sound)
        {
            SoundEffect soundEffect;
            if (sounds.TryGetValue(sound, out soundEffect))
                soundEffect.Play(volume, 0f, 0f);
        }
    }
}

