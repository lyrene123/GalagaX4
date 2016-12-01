using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GalagaX4
{
    class GameSound : Window
    {
        public string audiosource;
        bool repeated;
        public SoundPlayer audioSound;
        public MediaPlayer mediaplayer;
        public GameSound()
        {
            this.audiosource = "";
            this.repeated = false;

        }
        public GameSound(string uriPath, bool repeat)
        {
            this.audiosource = uriPath;
            this.repeated = repeat;
            Uri uri = new Uri(audiosource);
            audioSound = new SoundPlayer(Application.GetResourceStream(uri).Stream);

        }
        public GameSound(string uriPath)
        {
            this.audiosource = uriPath;
            mediaplayer = new MediaPlayer();
            Uri uri = new Uri(audiosource);
            mediaplayer.Open(uri);
            audioSound = new SoundPlayer(Application.GetResourceStream(uri).Stream);

        }

        public void playSound()
        {
            audioSound.Play();
        }
        public void playSoundMediaElement(string str)
        {
            GameWindow media = new GameWindow();
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, 1);
            media.mediaElement.Position = ts;
            media.mediaElement.Volume = 4.6;
            media.mediaElement.Source = new Uri(str, UriKind.Relative);
            media.mediaElement.Play();

        }
        public void playSoundLooping()
        {
            audioSound.PlayLooping();
        }
        public void StopSound()
        {
            audioSound.Stop();
        }
        public string getAudio()
        {
            return this.audiosource;
        }
        public void setRepeated(bool repeat)
        {
            this.repeated = repeat;
        }
        public bool isRepeated()
        {
            return this.repeated;
        }
        public void playShootSound()
        {
            GameSound shoot = new GameSound(@"pack://application:,,,/GalagaX4;Component/audio/Firing.wav", true);
            shoot.playSound();
        }
    }
}
