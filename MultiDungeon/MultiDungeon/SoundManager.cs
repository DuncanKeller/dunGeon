using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace MultiDungeon
{
    public static class SoundManager
    {
        static AudioEngine engine;
        static SoundBank soundBank;
        static WaveBank waveBank;
        static Dictionary<string, SoundEffect> specialSounds = new Dictionary<string, SoundEffect>();

        public static AudioEngine AudioEngine
        {
            get { return engine; }
        }

        public static SoundBank SoundBank
        {
            get { return soundBank; }
        }

        public static WaveBank WaveBank
        {
            get { return waveBank; }
        }

        public static Dictionary<string, SoundEffect> SpecialSounds
        {
            get { return specialSounds; }
        }

        public static void Init(ContentManager c)
        {
            engine = new AudioEngine(@"Content\\Sound\\dungeon-sounds.xgs");
            soundBank = new SoundBank(engine, c.RootDirectory + "\\Sound\\Sound Bank.xsb");
            waveBank = new WaveBank(engine, c.RootDirectory + "\\Sound\\Wave Bank.xwb");

            specialSounds.Add("gunshot", c.Load<SoundEffect>("Sound\\gunshot"));
            specialSounds.Add("ding", c.Load<SoundEffect>("Sound\\ding"));
            specialSounds.Add("assault-rifle", c.Load<SoundEffect>("Sound\\assault-rifle"));
        }

        public static void Update()
        {
            engine.Update();
        }

        public static void PlaySound(string s)
        {
            soundBank.GetCue(s).Play();
        }

    }
}
