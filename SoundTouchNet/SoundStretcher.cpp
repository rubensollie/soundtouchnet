// This is the main DLL file.

#include "stdafx.h"

#include "SoundTouch.h"

using namespace System;
using namespace System::IO;

namespace SoundTouchNet
{

#define SAMPLETYPE_NET float

	public ref class SoundStretcher : Stream
	{
	public:

		soundtouch::SoundTouch* st;
		
		SoundStretcher(int sampleRate, int channels)
		{
			st = new soundtouch::SoundTouch();
			st->setSampleRate(sampleRate);
			st->setChannels(channels);
		}

		~SoundStretcher()
		{
			delete st;
		}

		void Clear()
		{
			st->clear();
		}

		void PutSamples(array<SAMPLETYPE_NET> ^ samples, int count)
		{
			pin_ptr<SAMPLETYPE_NET> p1 = &samples[0];
			SAMPLETYPE_NET* p2 = p1;
			
			st->putSamples(p2,count);
		}

		int ReceiveSamples(array<SAMPLETYPE_NET> ^ buffer, int count)
		{
			pin_ptr<SAMPLETYPE_NET> p1 = &buffer[0];
			SAMPLETYPE_NET* p2 = p1;
		
			return st->receiveSamples(p2,count);
		}


		virtual void  Flush()  override
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
		

	  property  bool CanRead 
	  {
	   virtual bool get() override { return !Empty; }
	  }

	  property bool CanSeek 
	  {
		virtual bool get() override { return false; }
	  }

	  property bool CanWrite 
	  {
		virtual bool get() override { return true; }
	  }

	  property __int64 Length 
	  {
		virtual __int64 get() override { throw gcnew  NotSupportedException(); }
	  }

	  property __int64 Position 
	  {
		virtual __int64 get() override
		{
		  throw gcnew  NotSupportedException();
		}
		virtual void set(__int64 value) override
		{
		  throw gcnew  NotSupportedException();
		}
	  }
	  
	  virtual int Read(array<SAMPLETYPE_NET> ^ buffer, int offset, int  count) override
	  {
		pin_ptr<SAMPLETYPE_NET> p1 = &buffer[offset];
		SAMPLETYPE_NET* p2 = p1;		
		return st->receiveSamples( (SAMPLETYPE_NET*)(p2),count / sizeof(SAMPLETYPE_NET));
	  }

	  virtual __int64 Seek(__int64 offset, SeekOrigin  origin) override
	  {
		throw gcnew  NotImplementedException();
	  }

	  virtual void SetLength(__int64 value) override
	  {
		throw gcnew  NotImplementedException();
	  }

	  virtual void Write(array<SAMPLETYPE_NET> ^ buffer, int  offset, int   count) override
	  {
		pin_ptr<SAMPLETYPE_NET> p1 = &buffer[offset];
		SAMPLETYPE_NET* p2 = p1;		
		st->putSamples( (SAMPLETYPE_NET*)(p2),count / sizeof(SAMPLETYPE_NET));
	  }
	  

	};
}