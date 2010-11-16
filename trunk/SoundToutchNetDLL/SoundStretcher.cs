using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SoundTouchNet
{

  public class SoundStretcher : IDisposable
  {
    private IntPtr handle;

    public SoundStretcher(int sampleRate, int channels)
    {
      handle = soundtouch_createInstance();
      if (handle == IntPtr.Zero)
        throw new Exception();

      soundtouch_setChannels(handle, (uint)channels);
      soundtouch_setSampleRate(handle, (uint)sampleRate);
    }

    public void Dispose()
    {
      if (handle != IntPtr.Zero)
      {
        soundtouch_destroyInstance(handle);
        handle = IntPtr.Zero;
      }
    }

    public int Samples
    {
      get { return (int)soundtouch_numSamples(handle); }
    }

    public int UnprocessedSamples
    {
      get { return (int)soundtouch_numUnprocessedSamples(handle); }
    }

    public void PutSamples(float[] samples, int count)
    {
      soundtouch_putSamples(handle, samples, (uint)count);
    }

    public void ReceiveSamples(float[] buffer, int count)
    {
      soundtouch_receiveSamples(handle, buffer, (uint)count);
    }

    public void Flush()
    {
      soundtouch_flush(handle);
    }

    public float Tempo
    {
      set
      {
        soundtouch_setTempo(handle, value);
      }
    }
    
    #region DLL import

    const string soundTouchDLL = "soundtouch.dll";

    /// Create a new instance of SoundTouch processor.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern IntPtr soundtouch_createInstance();

    /// Destroys a SoundTouch processor instance.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_destroyInstance(IntPtr h);

    /// Get SoundTouch library version string
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.LPStr)]
    private static extern String soundtouch_getVersionString();

    /// Get SoundTouch library version Id
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern uint soundtouch_getVersionId();

    /// Sets new rate control value. Normal rate = 1.0, smaller values
    /// represent slower rate, larger faster rates.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_setRate(IntPtr h, float newRate);

    /// Sets new tempo control value. Normal tempo = 1.0, smaller values
    /// represent slower tempo, larger faster tempo.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_setTempo(IntPtr h, float newTempo);

    /// Sets new rate control value as a difference in percents compared
    /// to the original rate (-50 .. +100 %);
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_setRateChange(IntPtr h, float newRate);

    /// Sets new tempo control value as a difference in percents compared
    /// to the original tempo (-50 .. +100 %);
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_setTempoChange(IntPtr h, float newTempo);

    /// Sets new pitch control value. Original pitch = 1.0, smaller values
    /// represent lower pitches, larger values higher pitch.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_setPitch(IntPtr h, float newPitch);

    /// Sets pitch change in octaves compared to the original pitch  
    /// (-1.00 .. +1.00);
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_setPitchOctaves(IntPtr h, float newPitch);

    /// Sets pitch change in semi-tones compared to the original pitch
    /// (-12 .. +12);
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_setPitchSemiTones(IntPtr h, float newPitch);


    /// Sets the number of channels, 1 = mono, 2 = stereo
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_setChannels(IntPtr h, uint numChannels);

    /// Sets sample rate.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_setSampleRate(IntPtr h, uint srate);

    /// Flushes the last samples from the processing pipeline to the output.
    /// Clears also the internal processing buffers.
    //
    /// Note: This function is meant for extracting the last samples of a sound
    /// stream. This function may introduce additional blank samples in the end
    /// of the sound stream, and thus it's not recommended to call this function
    /// in the middle of a sound stream.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_flush(IntPtr h);

    /// Adds 'numSamples' pcs of samples from the 'samples' memory position into
    /// the input of the object. Notice that sample rate _has_to_ be set before
    /// calling this function, otherwise throws a runtime_error exception.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_putSamples(IntPtr h,
       [MarshalAs(UnmanagedType.LPArray)] float[] samples,       ///< Pointer to sample buffer.
       uint numSamples      ///< Number of samples in buffer. Notice
                            ///< that in case of stereo-sound a single sample
                            ///< contains data for both channels.
       );

    /// Clears all the samples in the object's output and internal processing
    /// buffers.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern void soundtouch_clear(IntPtr h);

    /// Changes a setting controlling the processing system behaviour. See the
    /// 'SETTING_...' defines for available setting ID's.
    /// 
    /// \return 'TRUE' if the setting was succesfully changed
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern bool soundtouch_setSetting(IntPtr h,
               int settingId,   ///< Setting ID number. see SETTING_... defines.
               int value        ///< New setting value.
               );

    /// Reads a setting controlling the processing system behaviour. See the
    /// 'SETTING_...' defines for available setting ID's.
    ///
    /// \return the setting value.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern int soundtouch_getSetting(IntPtr h,
                         int settingId    ///< Setting ID number, see SETTING_... defines.
               );


    /// Returns number of samples currently unprocessed.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern uint soundtouch_numUnprocessedSamples(IntPtr h);

    /// Adjusts book-keeping so that given number of samples are removed from beginning of the 
    /// sample buffer without copying them anywhere. 
    ///
    /// Used to reduce the number of samples in the buffer when accessing the sample buffer directly
    /// with 'ptrBegin' function.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern uint soundtouch_receiveSamples(IntPtr h,
           [MarshalAs(UnmanagedType.LPArray)] float[] outBuffer,           ///< Buffer where to copy output samples.
           uint maxSamples     ///< How many samples to receive at max.
           );

    /// Returns number of samples currently available.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern uint soundtouch_numSamples(IntPtr h);

    /// Returns nonzero if there aren't any samples available for outputting.
    [DllImport(soundTouchDLL, CallingConvention = CallingConvention.StdCall)]
    private static extern uint soundtouch_isEmpty(IntPtr h);

  }

    #endregion

}
