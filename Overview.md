There is only one class in SoundTouchNet namespace. It is named by soundtouch utility soundstretcher.

```
using SoundTouchNet;


SoundStretcher stretcher = 
       new SoundStretcher( 16000 /* Sample rate */,
                           1     /* Number of channels */);


stretcher.Tempo = 2;  // Change tempto, 2 times faster


...
stretcher.PutSamples(shortBuffer); // Sending data 
...


stretcher.Flush(); // Flush buffer


...
int count =  stretcher.ReceiveSamples(shortBuffer); // Reading data
...


stretcher.Dispose(); // Class is IDisposable

```