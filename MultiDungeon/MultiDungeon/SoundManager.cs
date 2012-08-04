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


        public static void Init(ContentManager c)
        {
            engine = new AudioEngine(@"Content\\Sound\\dungeon-sounds.xgs");
            soundBank = new SoundBank(engine, c.RootDirectory + "\\Sound\\Sound Bank.xsb");
            waveBank = new WaveBank(engine, c.RootDirectory + "\\Sound\\Wave Bank.xwb");
        }

        public static void Update()
        {
            engine.Update();
        }

        public static void PlaySound(string s)
        {
            Cue c = soundBank.GetCue(s);
            c.Play();
        }

    }
}
