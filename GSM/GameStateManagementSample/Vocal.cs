using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.Kinect.Audio;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Xna.Framework;
using System.Threading;
using GameStateManagement;

namespace SS12Game
{
    class Vocal
    {
        public enum Verbs
        {
            None = 0,
            Ready,
            Fire,
            Angle,
            Number,
            Colorize,
            RandomColors,
            ShapesAndColors,
            Reset,
            Pause,
            Resume,
            Up,
            Down,
            Movement,
            Enter
        };

        struct WhatSaid
        {
            public Verbs verb;
            public Color color;
            public int angle;
        }

        Dictionary<string, WhatSaid> GameplayPhrases = new Dictionary<string, WhatSaid>()
        {
            {"Angle", new WhatSaid()      {verb=Verbs.Angle}},  
            {"Ready", new WhatSaid()      {verb=Verbs.Ready}},
            {"Fire", new WhatSaid()       {verb=Verbs.Fire}},
            {"Up", new WhatSaid()       {verb=Verbs.Up}},
            {"Down", new WhatSaid()       {verb=Verbs.Down}},
            {"Movement", new WhatSaid()       {verb=Verbs.Movement}},
            {"Enter", new WhatSaid()       {verb=Verbs.Enter}}
        };

        //set up dictionary for basic colors
        //        Dictionary<string, WhatSaid> ColorPhrases = new Dictionary<string, WhatSaid>()
        //        {
        //            {"Every Color", new WhatSaid()      {verb=Verbs.RandomColors}},
        //            {"All Colors", new WhatSaid()       {verb=Verbs.RandomColors}},
        //            {"Random Colors", new WhatSaid()    {verb=Verbs.RandomColors}},
        //            {"Red", new WhatSaid()              {verb=Verbs.Colorize, color = new Color(240,60,60)}},
        //            {"Green", new WhatSaid()            {verb=Verbs.Colorize, color = new Color(60,240,60)}},
        //            {"Blue", new WhatSaid()             {verb=Verbs.Colorize, color = new Color(60,60,240)}},
        //            {"Yellow", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(240,240,60)}},
        //            {"Orange", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(255,110,20)}},
        //            {"Purple", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(70,30,255)}},
        //            {"Violet", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(160,30,245)}},
        //            {"Pink", new WhatSaid()             {verb=Verbs.Colorize, color = new Color(255,128,225)}},
        //            {"Gray", new WhatSaid()             {verb=Verbs.Colorize, color = new Color(192,192,192)}},
        //            {"Brown", new WhatSaid()            {verb=Verbs.Colorize, color = new Color(130,80,50)}},
        //            {"Dark", new WhatSaid()             {verb=Verbs.Colorize, color = new Color(40,40,40)}},
        //            {"Black", new WhatSaid()            {verb=Verbs.Colorize, color = new Color(5,5,5)}},
        //            {"Bright", new WhatSaid()           {verb=Verbs.Colorize, color = new Color(240,240,240)}},
        //           {"White", new WhatSaid()            {verb=Verbs.Colorize, color = new Color(255,255,255)}},
        //        };

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
            {"10", new WhatSaid()                {verb=Verbs.Number, angle = 10}},
            {"11", new WhatSaid()                {verb=Verbs.Number, angle = 11}},
            {"12", new WhatSaid()                {verb=Verbs.Number, angle = 12}},
            {"13", new WhatSaid()                {verb=Verbs.Number, angle = 13}},
            {"14", new WhatSaid()                {verb=Verbs.Number, angle = 14}},
            {"15", new WhatSaid()                {verb=Verbs.Number, angle = 15}},
            {"16", new WhatSaid()                {verb=Verbs.Number, angle = 16}},
            {"17", new WhatSaid()                {verb=Verbs.Number, angle = 17}},
            {"18", new WhatSaid()                {verb=Verbs.Number, angle = 18}},
            {"19", new WhatSaid()                {verb=Verbs.Number, angle = 19}},
            {"20", new WhatSaid()                {verb=Verbs.Number, angle = 20}},
            {"21", new WhatSaid()                {verb=Verbs.Number, angle = 21}},
            {"22", new WhatSaid()                {verb=Verbs.Number, angle = 22}},
            {"23", new WhatSaid()                {verb=Verbs.Number, angle = 23}},
            {"24", new WhatSaid()                {verb=Verbs.Number, angle = 24}},
            {"25", new WhatSaid()                {verb=Verbs.Number, angle = 25}},
            {"26", new WhatSaid()                {verb=Verbs.Number, angle = 26}},
            {"27", new WhatSaid()                {verb=Verbs.Number, angle = 27}},
            {"28", new WhatSaid()                {verb=Verbs.Number, angle = 28}},
            {"29", new WhatSaid()                {verb=Verbs.Number, angle = 29}},
            {"30", new WhatSaid()                {verb=Verbs.Number, angle = 30}},
            {"31", new WhatSaid()                {verb=Verbs.Number, angle = 31}},
            {"32", new WhatSaid()                {verb=Verbs.Number, angle = 32}},
            {"33", new WhatSaid()                {verb=Verbs.Number, angle = 33}},
            {"34", new WhatSaid()                {verb=Verbs.Number, angle = 34}},
            {"35", new WhatSaid()                {verb=Verbs.Number, angle = 35}},
            {"36", new WhatSaid()                {verb=Verbs.Number, angle = 36}},
            {"37", new WhatSaid()                {verb=Verbs.Number, angle = 37}},
            {"38", new WhatSaid()                {verb=Verbs.Number, angle = 38}},
            {"39", new WhatSaid()                {verb=Verbs.Number, angle = 39}},
            {"40", new WhatSaid()                {verb=Verbs.Number, angle = 40}},
            {"41", new WhatSaid()                {verb=Verbs.Number, angle = 41}},
            {"42", new WhatSaid()                {verb=Verbs.Number, angle = 42}},
            {"43", new WhatSaid()                {verb=Verbs.Number, angle = 43}},
            {"44", new WhatSaid()                {verb=Verbs.Number, angle = 44}},
            {"45", new WhatSaid()                {verb=Verbs.Number, angle = 45}},
            {"46", new WhatSaid()                {verb=Verbs.Number, angle = 46}},
            {"47", new WhatSaid()                {verb=Verbs.Number, angle = 47}},
            {"48", new WhatSaid()                {verb=Verbs.Number, angle = 48}},
            {"49", new WhatSaid()                {verb=Verbs.Number, angle = 49}},
            {"50", new WhatSaid()                {verb=Verbs.Number, angle = 50}},
            {"51", new WhatSaid()                {verb=Verbs.Number, angle = 51}},
            {"52", new WhatSaid()                {verb=Verbs.Number, angle = 52}},
            {"53", new WhatSaid()                {verb=Verbs.Number, angle = 53}},
            {"54", new WhatSaid()                {verb=Verbs.Number, angle = 54}},
            {"55", new WhatSaid()                {verb=Verbs.Number, angle = 55}},
            {"56", new WhatSaid()                {verb=Verbs.Number, angle = 56}},
            {"57", new WhatSaid()                {verb=Verbs.Number, angle = 57}},
            {"58", new WhatSaid()                {verb=Verbs.Number, angle = 58}},
            {"59", new WhatSaid()                {verb=Verbs.Number, angle = 59}},
            {"60", new WhatSaid()                {verb=Verbs.Number, angle = 60}},
            {"61", new WhatSaid()                {verb=Verbs.Number, angle = 61}},
            {"62", new WhatSaid()                {verb=Verbs.Number, angle = 62}},
            {"63", new WhatSaid()                {verb=Verbs.Number, angle = 63}},
            {"64", new WhatSaid()                {verb=Verbs.Number, angle = 64}},
            {"65", new WhatSaid()                {verb=Verbs.Number, angle = 65}},
            {"66", new WhatSaid()                {verb=Verbs.Number, angle = 66}},
            {"67", new WhatSaid()                {verb=Verbs.Number, angle = 67}},
            {"68", new WhatSaid()                {verb=Verbs.Number, angle = 68}},
            {"69", new WhatSaid()                {verb=Verbs.Number, angle = 69}},
            {"70", new WhatSaid()                {verb=Verbs.Number, angle = 70}},
            {"71", new WhatSaid()                {verb=Verbs.Number, angle = 71}},
            {"72", new WhatSaid()                {verb=Verbs.Number, angle = 72}},
            {"73", new WhatSaid()                {verb=Verbs.Number, angle = 73}},
            {"74", new WhatSaid()                {verb=Verbs.Number, angle = 74}},
            {"75", new WhatSaid()                {verb=Verbs.Number, angle = 75}},
            {"76", new WhatSaid()                {verb=Verbs.Number, angle = 76}},
            {"77", new WhatSaid()                {verb=Verbs.Number, angle = 77}},
            {"78", new WhatSaid()                {verb=Verbs.Number, angle = 78}},
            {"79", new WhatSaid()                {verb=Verbs.Number, angle = 79}},
            {"80", new WhatSaid()                {verb=Verbs.Number, angle = 80}},
            {"81", new WhatSaid()                {verb=Verbs.Number, angle = 81}},
            {"82", new WhatSaid()                {verb=Verbs.Number, angle = 82}},
            {"83", new WhatSaid()                {verb=Verbs.Number, angle = 83}},
            {"84", new WhatSaid()                {verb=Verbs.Number, angle = 84}},
            {"85", new WhatSaid()                {verb=Verbs.Number, angle = 85}},
            {"86", new WhatSaid()                {verb=Verbs.Number, angle = 86}},
            {"87", new WhatSaid()                {verb=Verbs.Number, angle = 87}},
            {"88", new WhatSaid()                {verb=Verbs.Number, angle = 88}},
            {"89", new WhatSaid()                {verb=Verbs.Number, angle = 89}},
            {"90", new WhatSaid()                {verb=Verbs.Number, angle = 90}},
            {"91", new WhatSaid()                {verb=Verbs.Number, angle = 91}},
            {"92", new WhatSaid()                {verb=Verbs.Number, angle = 92}},
            {"93", new WhatSaid()                {verb=Verbs.Number, angle = 93}},
            {"94", new WhatSaid()                {verb=Verbs.Number, angle = 94}},
            {"95", new WhatSaid()                {verb=Verbs.Number, angle = 95}},
            {"96", new WhatSaid()                {verb=Verbs.Number, angle = 96}},
            {"97", new WhatSaid()                {verb=Verbs.Number, angle = 97}},
            {"98", new WhatSaid()                {verb=Verbs.Number, angle = 98}},
            {"99", new WhatSaid()                {verb=Verbs.Number, angle = 99}}
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
            public string Phrase { get; set; }
            public string Matched { get; set; }
            public int Angle { get; set; }
        }

        public event EventHandler<SaidSomethingArgs> SaidSomething;

        private KinectAudioSource kinectSource;
        private SpeechRecognitionEngine sre;
        private const string RecognizerId = "SR_MS_en-US_Kinect_10.0";
        private bool paused = false;
        private bool valid = false;
        GameStateManagementGame thisGameIsAwesome;

        public Vocal(GameStateManagementGame myGame)
        {
            thisGameIsAwesome = myGame;
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

            //            var colors = new Choices();
            //            foreach (var phrase in ColorPhrases)
            //                colors.Add(phrase.Key);

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
            //            objectChoices.Add(colors);
            objectChoices.Add(numbers);
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
            Console.WriteLine("Speech Hypothesized: \t{0}", e.Result.Text);
        }

        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Speech Recognized: \t{0}", e.Result.Text);

            //if (SaidSomething == null)
            //    return;

            var said = new SaidSomethingArgs();
            said.Verb = 0;
            said.Angle = 0;
            said.Phrase = e.Result.Text;

            
            // Look for a match in the order of the lists below, first match wins.
            List<Dictionary<string, WhatSaid>> allDicts = new List<Dictionary<string, WhatSaid>>() { SinglePhrases, GameplayPhrases, NumberPhrases };

            bool found = false;
            for (int i = 0; i < allDicts.Count && !found; ++i)
            {
                //Console.WriteLine("im in dict " + i);
                foreach (var phrase in allDicts[i])
                {
                    if (e.Result.Text.Contains(phrase.Key))
                    {
                        said.Verb = phrase.Value.verb;
                        Console.WriteLine("Found a verb: " + said.Verb);
                        {
                            said.Matched = phrase.Key;
                        }
                        if (said.Verb == Verbs.Number)
                        {
                            said.Angle = phrase.Value.angle;
                            Console.WriteLine("Im a number " + said.Angle);
                        }
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                Console.WriteLine("Nothing in the Dictionary");
                return;
            }


            Console.WriteLine("My verb is " + said.Verb);

            if (said.Verb == Verbs.Pause)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Pause;
            else if (said.Verb == Verbs.Reset)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Reset;
            else if (said.Verb == Verbs.Resume)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Resume;
            else if (said.Verb == Verbs.Ready)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Ready;
            else if (said.Verb == Verbs.Angle)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Angle;
            else if (said.Verb == Verbs.Number)
            {
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Number;
                thisGameIsAwesome.screenManager.input.myAngle = said.Angle;
            }
            else if (said.Verb == Verbs.Fire)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Fire;
            else if (said.Verb == Verbs.Up)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Up;
            else if (said.Verb == Verbs.Down)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Down;
            else if (said.Verb == Verbs.Movement)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Movement;
            else if (said.Verb == Verbs.Enter)
                thisGameIsAwesome.screenManager.input.currentVoiceCommand = InputState.voiceCommandStates.Enter;
            
            
            //SaidSomething(new object(), said);
        }
    }
}
