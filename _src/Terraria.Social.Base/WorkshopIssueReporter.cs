using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Terraria.Social.Base;

public class WorkshopIssueReporter : IProvideReports
{
	private int _maxReports = 1000;

	private List<IssueReport> _reports = new List<IssueReport>();

	public event Action OnNeedToOpenUI;

	public event Action OnNeedToNotifyUI;

	private void AddReport(string reportText)
	{
		IssueReport item = new IssueReport(reportText);
		_reports.Add(item);
		while (_reports.Count > _maxReports)
		{
			_reports.RemoveAt(0);
		}
	}

	public List<IssueReport> GetReports()
	{
		return _reports;
	}

	private void OpenReportsScreen()
	{
		if (OnNeedToOpenUI != null)
		{
			OnNeedToOpenUI();
		}
	}

	private void NotifyReportsScreen()
	{
		if (OnNeedToNotifyUI != null)
		{
			OnNeedToNotifyUI();
		}
	}

	public void ReportInstantUploadProblem(string textKey)
	{
		string textValue = Language.GetTextValue(textKey);
		AddReport(textValue);
		OpenReportsScreen();
	}

	public void ReportInstantUploadProblem(string textKey, object subs)
	{
		string textValueWith = Language.GetTextValueWith(textKey, subs);
		AddReport(textValueWith);
		OpenReportsScreen();
	}

	public void ReportDelayedUploadProblem(string textKey)
	{
		string textValue = Language.GetTextValue(textKey);
		AddReport(textValue);
		NotifyReportsScreen();
	}

	public void ReportDelayedUploadProblem(string textKey, object subs)
	{
		string textValueWith = Language.GetTextValueWith(textKey, subs);
		AddReport(textValueWith);
		NotifyReportsScreen();
	}

	public void ReportDownloadProblem(string textKey, string path, Exception exception)
	{
		object obj = new
		{
			FilePath = path,
			Reason = exception.ToString()
		};
		string textValueWith = Language.GetTextValueWith(textKey, obj);
		AddReport(textValueWith);
		NotifyReportsScreen();
	}

	public void ReportManifestCreationProblem(string textKey, Exception exception)
	{
		object obj = new
		{
			Reason = exception.ToString()
		};
		string textValueWith = Language.GetTextValueWith(textKey, obj);
		AddReport(textValueWith);
		NotifyReportsScreen();
	}
}
