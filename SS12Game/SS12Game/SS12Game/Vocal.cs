using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Xna.Framework;

namespace SS12Game
{
    class Vocal
    {
        public enum Verbs
        {
            None = 0,
            Colorize,
            RandomColors,
            ShapesAndColors,
            Reset,
            Pause,
            Resume
        };

        struct WhatSaid
        {
            public Verbs verb;
            public Color color;
        }

        //set up dictionary for 
        Dictionary<string, WhatSaid> ColorPhrases = new Dictionary<string, WhatSaid>()
        {
            {"Every Color", new WhatSaid()      {verb=Verbs.RandomColors}},
            {"All Colors", new WhatSaid()       {verb=Verbs.RandomColors}},
            {"Random Colors", new WhatSaid()    {verb=Verbs.RandomColors}},
            {"Red", new WhatSaid()              {verb=Verbs.Colorize, color = new Color(240,60,60)}},
            {"Green", new WhatSaid()            {verb=Verbs.Colorize, color = new Color(60,240,60)}},
            {"Blue", new WhatSaid()             {verb=Verbs.Colorize, color = new Color(60,60,240)}},
            {"Yellow", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(240,240,60)}},
            {"Orange", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(255,110,20)}},
            {"Purple", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(70,30,255)}},
            {"Violet", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(160,30,245)}},
            {"Pink", new WhatSaid()             {verb=Verbs.Colorize, color = new Color(255,128,225)}},
            {"Gray", new WhatSaid()             {verb=Verbs.Colorize, color = new Color(192,192,192)}},
            {"Brown", new WhatSaid()            {verb=Verbs.Colorize, color = new Color(130,80,50)}},
            {"Dark", new WhatSaid()             {verb=Verbs.Colorize, color = new Color(40,40,40)}},
            {"Black", new WhatSaid()            {verb=Verbs.Colorize, color = new Color(5,5,5)}},
            {"Bright", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(240,240,240)}},
            {"White", new WhatSaid()            {verb=Verbs.Colorize, color = new Color(255,255,255)}},
        };

        //Set up dictionary for single command phrases
        Dictionary<string, WhatSaid> SinglePhrases = new Dictionary<string, WhatSaid>()
        {
            {"Reset", new WhatSaid()            {verb=Verbs.Reset}},
            {"Clear", new WhatSaid()            {verb=Verbs.Reset}},
            {"Stop", new WhatSaid()             {verb=Verbs.Pause}},
            {"Pause", new WhatSaid()            {verb=Verbs.Pause}},
            {"Pause Game", new WhatSaid()       {verb=Verbs.Pause}},
            {"Freeze", new WhatSaid()           {verb=Verbs.Pause}},
            {"Unfreeze", new WhatSaid()         {verb=Verbs.Resume}},
            {"Resume", new WhatSaid()           {verb=Verbs.Resume}},
            {"Continue", new WhatSaid()         {verb=Verbs.Resume}},
            {"Play", new WhatSaid()             {verb=Verbs.Resume}},
            {"Start", new WhatSaid()            {verb=Verbs.Resume}},
            {"Go", new WhatSaid()               {verb=Verbs.Resume}},
        };

        private KinectAudioSource kinectSource;
        private SpeechRecognitionEngine sre;
        private const string RecognizerId = "SR_MS_en-US_Kinect_10.0";
        private bool paused = false;
        private bool valid = false;

        public bool IsValid()
        {
            return valid;
        }

        private void StartDMO()
        {
            kinectSource = new KinectAudioSource();
            kinectSource.SystemMode = SystemMode.OptibeamArrayOnly;
            kinectSource.FeatureMode = true;
            kinectSource.AutomaticGainControl = false;
            kinectSource.MicArrayMode = MicArrayMode.MicArrayAdaptiveBeam;
            var kinectStream = kinectSource.Start();
            sre.SetInputToAudioStream(kinectStream, new SpeechAudioFormatInfo(
                                                  EncodingFormat.Pcm, 16000, 16, 1,
                                                  32000, 2, null));
            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void Stop()
        {
            if (sre != null)
            {
                sre.RecognizeAsyncCancel();
                sre.RecognizeAsyncStop();
                kinectSource.Dispose();
            }
        }

    }
}
