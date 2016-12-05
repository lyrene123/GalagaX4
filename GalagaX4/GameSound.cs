using Microsoft.Win32.SafeHandles;
using System;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GalagaX4
{
    class GameSound: IDisposable
    {
        public string audiosource;
        bool repeated;
        public SoundPlayer audioSound;
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

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
            audioSound.Load();

        }
        /*
        public void PlayMediaElement(string uriPath)
        {
            sound.mediaElement.Source = new Uri(uriPath, UriKind.Relative);
            sound.mediaElement.BeginInit();
            sound.mediaElement.Position = TimeSpan.FromMilliseconds(0);
            sound.mediaElement.Play();

        }
        */
        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        public void playSound()
        {
           audioSound.Play();
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
        public void playShootMediaPlayer()
        {
            GameSound shoot = new GameSound(@"pack://application:,,,/GalagaX4;Component/audio/Firing.wav", true);
            shoot.playShootMediaPlayer();
        }
    }
}
