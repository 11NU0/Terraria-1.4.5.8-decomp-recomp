#define TRACE
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace Terraria.Testing;

public static class LockstepDebug
{
	private struct ExpensiveStepRateState(bool increaseThreshold)
	{
		public long stepRate = 1L;

		public long remainingSkips = 0L;

		public int stepRateIncreaseCount = 0;

		public TimeSpan stepRateIncreaseThreshold = TimeSpan.FromSeconds(increaseThreshold ? 30 : 20);

		public Stopwatch stepTime = new Stopwatch();

		public long timedStepCount = 0L;

		internal void CheckAndIncreaseStepRate()
		{
			if (!(stepTime.Elapsed < stepRateIncreaseThreshold) && timedStepCount > 1)
			{
				stepRateIncreaseCount++;
				stepRate *= timedStepCount;
				timedStepCount = 0L;
				stepTime.Restart();
				Trace.WriteLine("LockstepDebug is taking too long. Reducing step rate to 1 in " + stepRate);
				WriteControlMessage("StepRate: " + stepRate);
			}
		}
	}

	private static long stepCount;

	private static long lastSuccessfulStep;

	private static bool stopAtLastStep;

	private static ExpensiveStepRateState expensiveStepState;

	private static bool _init;

	private static bool isHost;

	private static string Identifier = "Terraria.LockstepDebug";

	private static int BufSize = 65535;

	private static BinaryReader _reader;

	private static BinaryWriter _writer;

	private static readonly object _lock = new object();

	private static string _controlCode = "ģ4䕧";

	public static bool Enabled { get; private set; }

	private static void Init()
	{
		if (!_init)
		{
			PipeStream pipeStream;
			try
			{
				NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(Identifier);
				namedPipeClientStream.Connect(1);
				pipeStream = namedPipeClientStream;
				Trace.WriteLine("LockstepDebug connected to server.");
			}
			catch (TimeoutException)
			{
				Trace.WriteLine("LockstepDebug waiting for connection from client.");
				isHost = true;
				NamedPipeServerStream namedPipeServerStream = new NamedPipeServerStream(Identifier, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.WriteThrough, BufSize, BufSize);
				namedPipeServerStream.WaitForConnection();
				pipeStream = namedPipeServerStream;
			}
			_reader = new BinaryReader(pipeStream);
			_writer = new BinaryWriter(pipeStream);
			WriteStep("Init");
			if (ReadStep() != "Init")
			{
				throw new Exception("Shared memory communication failed");
			}
			_init = true;
		}
	}

	public static void Enable()
	{
		Init();
		Enabled = true;
		bool increaseThreshold = false;
		if (lastSuccessfulStep == 0L)
		{
			Trace.WriteLine("LockstepDebug enabled.");
		}
		else if (stepCount == lastSuccessfulStep + 1)
		{
			stopAtLastStep = true;
			Trace.WriteLine("LockstepDebug rerun detected. Skipping to and stopping at step " + lastSuccessfulStep);
		}
		else
		{
			Trace.WriteLine($"LockstepDebug rerun detected. Skipping to step {lastSuccessfulStep}. Up to {stepCount - lastSuccessfulStep} steps to find mismatch.");
			if (expensiveStepState.stepRateIncreaseCount == 1)
			{
				increaseThreshold = true;
			}
		}
		WriteStep("Enable");
		if (ReadStep() != "Enable")
		{
			throw new Exception("Enable sync failed");
		}
		stepCount = 0L;
		expensiveStepState = new ExpensiveStepRateState(increaseThreshold);
	}

	public static void ExpensiveStep<T>(Func<T> expensiveArg, params object[] args)
	{
		if (!Enabled)
		{
			return;
		}
		if (stepCount + 1 < lastSuccessfulStep)
		{
			stepCount++;
			return;
		}
		if (expensiveStepState.remainingSkips > 0)
		{
			stepCount++;
			expensiveStepState.remainingSkips--;
			return;
		}
		if (isHost)
		{
			expensiveStepState.CheckAndIncreaseStepRate();
		}
		expensiveStepState.stepTime.Start();
		Step(expensiveArg(), string.Join(", ", args));
		expensiveStepState.stepTime.Stop();
		expensiveStepState.timedStepCount++;
		expensiveStepState.remainingSkips = expensiveStepState.stepRate - 1;
	}

	public static void Step(params object[] args)
	{
		Step(string.Join(", ", args));
	}

	public static void Step(string state)
	{
		if (!Enabled)
		{
			return;
		}
		if (state.Length > BufSize / 2)
		{
			throw new ArgumentException("String too large");
		}
		stepCount++;
		if (stepCount < lastSuccessfulStep)
		{
			return;
		}
		WriteStep(state);
		string text = ReadStep();
		if (text != state)
		{
			Enabled = false;
			Trace.WriteLine($"Lockstep mismatch. Step: {stepCount}\nSent: {state}\nRecv: {text}");
			if (lastSuccessfulStep < stepCount - 1)
			{
				Trace.WriteLine($"Expensive steps were skipped. Rerun to narrow down the mismatch. Last successful step was {stepCount - lastSuccessfulStep} steps ago");
				return;
			}
			if (lastSuccessfulStep == stepCount)
			{
				Trace.WriteLine("Last successful step mismatch. The rerun was not deterministic.");
			}
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
			else
			{
				Debugger.Launch();
			}
			return;
		}
		if (stepCount == lastSuccessfulStep && stopAtLastStep)
		{
			Trace.WriteLine("LockstepDebug reached the last match from the previous run. Debug from here to identify desync");
			if (Debugger.IsAttached)
			{
				Debugger.Break();
			}
			else
			{
				Debugger.Launch();
			}
		}
		lastSuccessfulStep = stepCount;
	}

	private static void WriteStep(string state)
	{
		_writer.Write(state);
	}

	private static string ReadStep()
	{
		string text;
		while (true)
		{
			text = _reader.ReadString();
			if (!text.StartsWith(_controlCode))
			{
				break;
			}
			HandleControlMessage(text.Substring(_controlCode.Length));
		}
		return text;
	}

	private static void WriteControlMessage(string s)
	{
		_writer.Write(_controlCode + s);
	}

	private static void HandleControlMessage(string v)
	{
		if (v.StartsWith("StepRate: "))
		{
			int num = int.Parse(v.Substring("StepRate: ".Length));
			Trace.WriteLine("LockstepDebug control message received. Reducing step rate to 1 in " + num);
			expensiveStepState.stepRateIncreaseCount++;
			expensiveStepState.stepRate = num;
		}
	}
}
