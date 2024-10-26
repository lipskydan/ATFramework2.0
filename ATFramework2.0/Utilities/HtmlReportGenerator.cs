﻿namespace ATFramework2._0;

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class HtmlReportGenerator
{
    private static HtmlReportGenerator _instance;
    private static readonly object _lock = new object();

    private readonly TestSettings _testSettings;
    private StringBuilder _reportContent;
    private Dictionary<string, StringBuilder> _scenarioReports;
    private int _scenarioCounter = 0;

    private HtmlReportGenerator(TestSettings testSettings)
    {
        _testSettings = testSettings;
        _scenarioReports = new Dictionary<string, StringBuilder>();
        InitializeReport();
    }

    public static HtmlReportGenerator Instance(TestSettings testSettings)
    {
        lock (_lock)
        {
            if (_instance == null)
            {
                _instance = new HtmlReportGenerator(testSettings);
            }
            return _instance;
        }
    }

    private void InitializeReport()
    {
        _reportContent = new StringBuilder();
        _reportContent.AppendLine("<html>");
        _reportContent.AppendLine("<head>");
        _reportContent.AppendLine("<title>Test Report</title>");
        _reportContent.AppendLine("<style>");

        _reportContent.AppendLine(@"
            body {
                font-family: Arial, sans-serif;
                background-color: #f4f4f4;
                margin: 0;
                padding: 20px;
            }
            h1 {
                text-align: center;
                color: #333;
            }
            .collapsible {
                background-color: #4CAF50;
                color: white;
                cursor: pointer;
                padding: 10px;
                width: 100%;
                text-align: left;
                border: none;
                outline: none;
                font-size: 18px;
                margin-bottom: 5px;
            }
            .content {
                padding: 0 15px;
                display: none;
                overflow: hidden;
                background-color: #f9f9f9;
            }
            table {
                width: 100%;
                border-collapse: collapse;
                margin: 15px 0;
            }
            th, td {
                border: 1px solid #ddd;
                padding: 8px;
                text-align: left;
            }
            th {
                background-color: #4CAF50;
                color: white;
            }
            tr:nth-child(even) {
                background-color: #f2f2f2;
            }
            .status-pass {
                color: green;
                font-weight: bold;
            }
            .status-fail {
                color: red;
                font-weight: bold;
            }
            .timestamp {
                color: #888;
                font-style: italic;
            }
        ");

        _reportContent.AppendLine("</style>");
        _reportContent.AppendLine("<script>");

        _reportContent.AppendLine(@"
            document.addEventListener('DOMContentLoaded', function() {
                const coll = document.getElementsByClassName('collapsible');
                for (let i = 0; i < coll.length; i++) {
                    coll[i].addEventListener('click', function() {
                        this.classList.toggle('active');
                        const content = this.nextElementSibling;
                        if (content.style.display === 'block') {
                            content.style.display = 'none';
                        } else {
                            content.style.display = 'block';
                        }
                    });
                }
            });
        ");

        _reportContent.AppendLine("</script>");
        _reportContent.AppendLine("</head>");
        _reportContent.AppendLine("<body>");
        _reportContent.AppendLine("<h1>Automation Test Execution Report</h1>");
    }

    public void StartScenario(string scenarioName)
    {
        if (!_scenarioReports.ContainsKey(scenarioName))
        {
            var scenarioContent = new StringBuilder();

            string scenarioId = $"scenario{_scenarioCounter++}";
            scenarioContent.AppendLine($"<button class='collapsible'>{scenarioName}</button>");
            scenarioContent.AppendLine($"<div class='content' id='{scenarioId}'>");
            scenarioContent.AppendLine("<table>");
            scenarioContent.AppendLine("<tr><th>Step</th><th>Status</th><th>Timestamp</th></tr>");

            _scenarioReports[scenarioName] = scenarioContent;
        }
    }

    public void AddStepResult(string scenarioName, string stepName, string status)
    {
        if (_scenarioReports.ContainsKey(scenarioName))
        {
            var statusClass = status == "Passed" ? "status-pass" : "status-fail";
            _scenarioReports[scenarioName].AppendLine(
                $"<tr><td>{stepName}</td><td class='{statusClass}'>{status}</td><td class='timestamp'>{DateTime.Now}</td></tr>");
        }
    }

    public void EndScenario(string scenarioName)
    {
        if (_scenarioReports.ContainsKey(scenarioName))
        {
            _scenarioReports[scenarioName].AppendLine("</table>");
            _scenarioReports[scenarioName].AppendLine("</div>");
        }
    }

    public void FinalizeReport()
    {
        foreach (var scenario in _scenarioReports.Values)
        {
            _reportContent.Append(scenario.ToString());
        }
        _reportContent.AppendLine("</body></html>");

        File.WriteAllText(_testSettings.PathToSaveReport + $"Report_{DateTime.Now}.html", _reportContent.ToString());
    }
}
