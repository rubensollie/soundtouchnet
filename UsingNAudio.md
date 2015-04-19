# How to use NAudio with SoundTouch #


```
    /// <summary>
    /// Load wave file and change its tempo, pitch and rate and save it to another file
    /// </summary>
    static void processWave(string fileIn, string fileOut, float newTempo = 1, float newPitch = 1, float newRate = 1)
    {
      WaveFileReader reader = new WaveFileReader(fileIn);

      int numChannels = reader.WaveFormat.Channels;

      if (numChannels > 2)
        throw new Exception("SoundTouch supports only mono or stereo.");

      int sampleRate = reader.WaveFormat.SampleRate;

      int bitPerSample = reader.WaveFormat.BitsPerSample;

      const int BUFFER_SIZE = 1024 * 16;

      SoundStretcher stretcher = new SoundStretcher(sampleRate, numChannels);
      WaveFileWriter writer = new WaveFileWriter(fileOut, new WaveFormat(sampleRate, 16, numChannels));

      stretcher.Tempo = newTempo;
      stretcher.Pitch = newPitch;
      stretcher.Rate = newRate;

      byte[] buffer = new byte[BUFFER_SIZE];
      short[] buffer2 = null;

      if (bitPerSample != 16 && bitPerSample != 8)
      {
        throw new Exception("Not implemented yet.");
      }

      if (bitPerSample == 8)
      {
        buffer2 = new short[BUFFER_SIZE];
      }

      bool finished = false;

      while (true)
      {
        int bytesRead = 0;
        if (!finished)
        {
          bytesRead = reader.Read(buffer, 0, BUFFER_SIZE);

          if (bytesRead == 0)
          {
            finished = true;
            stretcher.Flush();
          }
          else
          {
            if (bitPerSample == 16)
            {
              stretcher.PutSamplesFromBuffer(buffer, 0, bytesRead);
            }
            else if (bitPerSample == 8)
            {
              for (int i = 0; i < BUFFER_SIZE; i++)
                buffer2[i] = (short)((buffer[i] - 128) * 256);
              stretcher.PutSamples(buffer2);
            }
          }
        }
        bytesRead = stretcher.ReceiveSamplesToBuffer(buffer, 0, BUFFER_SIZE);
        writer.WriteData(buffer, 0, bytesRead);

        if (finished && bytesRead == 0)
          break;
      }

      reader.Close();
      writer.Close();
    }
```