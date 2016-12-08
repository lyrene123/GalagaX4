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
    /// <summary>
    /// The GameSound Class creates SoundPlayer objects
    /// to controls playback of a sound from a .wav file.
    /// It has methods to manage the sounds. This class
    /// was created to control the sound effects of the game.
    /// The IDisposable Interface Provides a mechanism for 
    /// releasing unmanaged resources.
    /// </summary>
    class GameSound : IDisposable
    {
        public string audiosource;
        bool repeated;
        public SoundPlayer audioSound;
        // Dispose method already called? 
        bool disposed = false;
        // The SafeHandle class provides critical finalization of handle resources, preventing handles 
        //from being reclaimed prematurely by garbage collection and from being recycled by Windows to 
        //reference unintended unmanaged objects. (MSDN definition)
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        /// <summary>
        /// Empty Constructor to create a generic GameSound Object
        /// to access some useful methods.
        /// </summary>
        public GameSound()
        {
            this.audiosource = "";
            this.repeated = false;

        }
        /// <summary>
        /// Contructor of the GameSound Class.
        /// It creates a SoundPlayer Object and instantiates it
        /// to be able to ontrol playback of a sound from a .wav file.
        ///</summary>
        /// <param name="uriPath"> The path of the .wav file</param>
        /// <param name="repeat"></param>
        public GameSound(string uriPath, bool repeat)
        {
            this.audiosource = uriPath;
            this.repeated = repeat;
            Uri uri = new Uri(audiosource);
            audioSound = new SoundPlayer(Application.GetResourceStream(uri).Stream);
            audioSound.Load();

        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or 
        /// resetting unmanaged resources. (MSDN definition)
        /// </summary>
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

            }

            disposed = true;
        }
        /// <summary>
        /// The playSound method plays the .wav file using a new thread
        /// and loads the .wav file first if it has not been loaded.
        /// </summary>
        public void playSound()
        {
            audioSound.Play();
        }
        /// <summary>
        /// The playSoundLooping method plays and loops the 
        /// .wav file using a new thread, and loads the .wav 
        /// file first if it has not been loaded.
        /// </summary>
        public void playSoundLooping()
        {
            audioSound.PlayLooping();
        }
        /// <summary>
        /// The StopSound method Stops playback of the sound 
        /// if playback is occurring.
        /// </summary>
        public void StopSound()
        {
            audioSound.Stop();
        }
        /// <summary>
        /// The getAudio method returns the uriPath (audiosource)
        /// of the .wav file.
        /// </summary>
        /// <returns></returns>
        public string getAudio()
        {
            return this.audiosource;
        }
        /// <summary>
        /// The setRepeated method sets the repeated bool
        /// indicating is the sound shoud repeat or not.
        /// </summary>
        /// <param name="repeat"></param>
        public void setRepeated(bool repeat)
        {
            this.repeated = repeat;
        }
        /// <summary>
        /// The isRepeated method returns the bool
        /// repeated indicating if the sound was set to
        /// be repeated or not.
        /// </summary>
        /// <returns></returns>
        public bool isRepeated()
        {
            return this.repeated;
        }

    }
}
