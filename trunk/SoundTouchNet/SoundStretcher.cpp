// This is the main DLL file.

#include "stdafx.h"

#include "SoundTouch.h"

using namespace System;
using namespace System::IO;

namespace SoundTouchNet
{
	public ref class SoundStretcher  
	{
	public:

		int channels;

		soundtouch::SoundTouch* st;
		
		SoundStretcher(int sampleRate, int channels)
		{
			st = new soundtouch::SoundTouch();
			st->setSampleRate(sampleRate);
			st->setChannels(channels);
	
			this->channels = channels;
		}

		~SoundStretcher()
		{
			delete st;
		}

		void Clear()
		{
			st->clear();
		}


		void PutSamples(array<short> ^ samples)
		{
			pin_ptr<short> p1 = &samples[0];
			short* p2 = p1;
			
			st->putSamples(p2,samples->Length / channels );
		}

		int ReceiveSamples(array<short> ^ samples)
		{
			pin_ptr<short> p1 = &samples[0];
			short* p2 = p1;
		
			return st->receiveSamples(p2,samples->Length / channels) ;
		}

		void PutSamplesFromBuffer(array<unsigned char> ^ buffer, int  offset, int   count) 
	    {
			pin_ptr<unsigned char> p1 = &buffer[offset];
			unsigned char* p2 = p1;		
			st->putSamples( (short*)(p2),count / sizeof(short) / channels);
		}

		int ReceiveSamplesToBuffer(array<unsigned char> ^ buffer, int offset, int  count) 
  	    {
			pin_ptr<unsigned char> p1 = &buffer[offset];
			unsigned char* p2 = p1;		
			return   sizeof(short) * st->receiveSamples( (short*)(p2),count / sizeof(short) / channels ) * channels;
		}


		virtual void  Flush()  
		{
			st->flush();
		}

		property float Pitch
		{
			void set(float value)
			{
				 st->setPitch(value);
			}
		}

		property float Tempo
		{
			void set(float value)
			{
				 st->setTempo(value);
			}
		}

		property float Rate
		{
			void set(float value)
			{
				 st->setRate(value);
			}
		}

		property int AvaiableSamples
		{
			int get()
			{
				return st->numSamples();
			}
		}

		property int UnprocessedSamples
		{
			int get()
			{
				return st->numUnprocessedSamples();
			}
		}

		property bool Empty
		{
			bool get()
			{
				return (st->isEmpty()!=0);
			}
		}

		property int Version
		{
			int get()
			{
				return st->getVersionId();
			}
		}
		
      property String^ VersionString
	  {
		  String^ get()
		  {
			  const char* str = st->getVersionString();
			  String^ managed_str = gcnew String(str);
			  return managed_str;
		  }
	  }
	 
	  	
	  

	};
}