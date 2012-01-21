using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Xna.Framework;
using System.Threading;

namespace SS12Game
{
    class Vocal
    {
        public enum Verbs
        {
            None = 0,
            Ready,
            Fire,
            Number,
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
            public int angle;
        }

        Dictionary<string, WhatSaid> GameplayPhrases = new Dictionary<string, WhatSaid>()
        {
            {"Ready", new WhatSaid()       {verb=Verbs.Ready}},
            {"Fire", new WhatSaid()       {verb=Verbs.Fire}}
        };

        //set up dictionary for basic colors
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

        Dictionary<string, WhatSaid> NumberPhrases = new Dictionary<string, WhatSaid>()
        {
            {"0", new WhatSaid()                {verb=Verbs.Number, angle = 0}},
            {"1", new WhatSaid()                {verb=Verbs.Number, angle = 1}},
            {"2", new WhatSaid()                {verb=Verbs.Number, angle = 2}},
            {"3", new WhatSaid()                {verb=Verbs.Number, angle = 3}},
            {"4", new WhatSaid()                {verb=Verbs.Number, angle = 4}},
            {"5", new WhatSaid()                {verb=Verbs.Number, angle = 5}},
            {"6", new WhatSaid()                {verb=Verbs.Number, angle = 6}},
            {"7", new WhatSaid()                {verb=Verbs.Number, angle = 7}},
            {"8", new WhatSaid()                {verb=Verbs.Number, angle = 8}},
            {"9", new WhatSaid()                {verb=Verbs.Number, angle = 9}},
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

        public class SaidSomethingArgs : EventArgs
        {
            public Verbs Verb { get; set; }
            public Color RGBColor { get; set; }
            public string Phrase { get; set; }
            public string Matched {get; set; }
        }

        public event EventHandler<SaidSomethingArgs> SaidSomething;

        private KinectAudioSource kinectSource;
        private SpeechRecognitionEngine sre;
        private const string RecognizerId = "SR_MS_en-US_Kinect_10.0";
        private bool paused = false;
        private bool valid = false;

        public Vocal()
        {
            RecognizerInfo ri = SpeechRecognitionEngine.InstalledRecognizers().Where(r => r.Id == RecognizerId).FirstOrDefault();
            if (ri == null)
                return;

            sre = new SpeechRecognitionEngine(ri.Id);

            var single = new Choices();
            foreach (var phrase in SinglePhrases)
                single.Add(phrase.Key);

            var gameplay = new Choices();
            foreach (var phrase in GameplayPhrases)
                gameplay.Add(phrase.Key);

            var colors = new Choices();
            foreach (var phrase in ColorPhrases)
                colors.Add(phrase.Key);

            var numbers = new Choices();
            foreach (var phrase in NumberPhrases)
                numbers.Add(phrase.Key);

            var numberAction = new GrammarBuilder();
            numberAction.Append(gameplay);
            numberAction.Append(numbers);

            var doubleNumber = new GrammarBuilder();
            numberAction.Append(gameplay);
            numberAction.Append(numbers);
            numberAction.Append(numbers);
            
            var objectChoices = new Choices();
            objectChoices.Add(gameplay);
            objectChoices.Add(colors);
            objectChoices.Add(doubleNumber);

            var actionGrammar = new GrammarBuilder();
            actionGrammar.AppendWildcard();
            actionGrammar.Append(objectChoices);

            var allChoices = new Choices();
            allChoices.Add(actionGrammar);
            allChoices.Add(single);

            var gb = new GrammarBuilder();
            gb.Append(allChoices);

            var g = new Grammar(gb);
            sre.LoadGrammar(g);
            sre.SpeechRecognized += sre_SpeechRecognized;
            sre.SpeechHypothesized += sre_SpeechHypothesized;
            sre.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(sre_SpeechRecognitionRejected);
            
            var t = new Thread(StartDMO);
            t.Name = "Kinect Audio";
            t.Start();

            valid = true;
        }
        
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

        void sre_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            var said = new SaidSomethingArgs();
            said.Verb = Verbs.None;
            said.Matched = "?";
            //SaidSomething(new object(), said);
            Console.WriteLine("\nSpeech Rejected");
        }

        void sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Console.Write("\rSpeech Hypothesized: \t{0}", e.Result.Text);
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.Write("\rSpeech Recognized: \t{0}", e.Result.Text);
            
            if (SaidSomething == null)
                return;
            
            var said = new SaidSomethingArgs();
            said.RGBColor = new Color(0, 0, 0);
            said.Verb = 0;
            said.Phrase = e.Result.Text;

            // First check for color, in case both color _and_ shape were both spoken
            bool foundColor = false;
            foreach (var phrase in ColorPhrases)
                if (e.Result.Text.Contains(phrase.Key) && (phrase.Value.verb == Verbs.Colorize))
                {
                    said.RGBColor = phrase.Value.color;
                    said.Matched = phrase.Key;
                    foundColor = true;
                    break;
                }
            
            // Look for a match in the order of the lists below, first match wins.
            List<Dictionary<string, WhatSaid>> allDicts = new List<Dictionary<string, WhatSaid>>()
                { ColorPhrases, SinglePhrases };

            bool found = false;
            for (int i = 0; i < allDicts.Count && !found; ++i)
            {
                foreach (var phrase in allDicts[i])
                {
                    if (e.Result.Text.Contains(phrase.Key))
                    {
                        said.Verb = phrase.Value.verb;
                        {
                            said.Matched = phrase.Key;
                            said.RGBColor = phrase.Value.color;
                        }
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
                return;

            if (paused) // Only accept restart or reset
            {
                if ((said.Verb != Verbs.Resume) && (said.Verb != Verbs.Reset))
                    return;
                paused = false;
            }
            else
            {
                if (said.Verb == Verbs.Resume)
                    return;
            }
            
            if (said.Verb == Verbs.Pause)
                paused = true;

            SaidSomething(new object(), said);
        }
    }
}
