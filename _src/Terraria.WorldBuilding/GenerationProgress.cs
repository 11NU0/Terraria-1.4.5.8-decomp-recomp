namespace Terraria.WorldBuilding;

public class GenerationProgress
{
	private string _message = "";

	private double _value;

	private double _totalWeightedProgress;

	public double TotalWeight;

	public double CurrentPassWeight = 1.0;

	public string Message
	{
		get
		{
			return string.Format(_message, Value);
		}
		set
		{
			_message = value.Replace("%", "{0:0.0%}");
		}
	}

	public string MessageNoFormatting
	{
		get
		{
			return _message;
		}
		set
		{
			_message = value;
		}
	}

	public double Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = Utils.Clamp(value, 0.0, 1.0);
		}
	}

	public double TotalWeightedProgress
	{
		set
		{
			_totalWeightedProgress = value;
		}
	}

	public double TotalProgress
	{
		get
		{
			if (TotalWeight == 0.0)
			{
				return 0.0;
			}
			return (Value * CurrentPassWeight + _totalWeightedProgress) / TotalWeight;
		}
	}

	public void Set(double value)
	{
		Value = value;
	}

	public void Set(double value, double min, double max)
	{
		Value = min + value * (max - min);
	}

	public void Start(double weight)
	{
		CurrentPassWeight = weight;
		_value = 0.0;
	}

	public void End()
	{
		_totalWeightedProgress += CurrentPassWeight;
		_value = 0.0;
	}
}
