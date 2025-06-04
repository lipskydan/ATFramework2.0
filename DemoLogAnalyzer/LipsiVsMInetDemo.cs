using Microsoft.ML;
using Microsoft.ML.Data;
using ATFramework2._0;
using ATFramework2._0.Utilities.Logs;
using System;
using System.Collections.Generic;
using System.Linq;

public class LogEntryML
{
    public string Message { get; set; }
    public string Label { get; set; }
}

public class LogPrediction
{
    [ColumnName("PredictedLabel")]
    public string PredictedLabel;
}

public class LipsiVsMInetDemo
{
    public static void Run()
    {
        var labeledLogs = new Dictionary<LogEntry, string>
        {
            { new LogEntry { Message = "Timeout while waiting for data", Level = LogLevel.Error, Context = "Login", Timestamp = DateTime.Now.AddMinutes(-1), Feature = "Auth" }, "Critical" },
            { new LogEntry { Message = "Retrying after slow response", Level = LogLevel.Warning, Context = "Dashboard", Timestamp = DateTime.Now.AddMinutes(-3), Feature = "UX" }, "High Priority" },
            { new LogEntry { Message = "Page loaded successfully", Level = LogLevel.Info, Context = "Home", Timestamp = DateTime.Now.AddMinutes(-7), Feature = "UI" }, "Low Priority" },
            { new LogEntry { Message = "Unhandled exception: NullReference", Level = LogLevel.Critical, Context = "Login", Timestamp = DateTime.Now, Feature = "Auth" }, "Critical" },
            { new LogEntry { Message = "API returned 500 error", Level = LogLevel.Error, Context = "Profile", Timestamp = DateTime.Now, Feature = "API" }, "High Priority" },
            { new LogEntry { Message = "Slow network detected", Level = LogLevel.Warning, Context = "Network", Timestamp = DateTime.Now.AddMinutes(-2), Feature = "Infra" }, "High Priority" },
            { new LogEntry { Message = "Cache cleared successfully", Level = LogLevel.Info, Context = "Settings", Timestamp = DateTime.Now.AddMinutes(-6), Feature = "Cache" }, "Low Priority" },
            { new LogEntry { Message = "Old API version used", Level = LogLevel.Warning, Context = "API", Timestamp = DateTime.Now.AddMinutes(-8), Feature = "API" }, "High Priority" },
            { new LogEntry { Message = "Login timeout after multiple retries", Level = LogLevel.Error, Context = "Login", Timestamp = DateTime.Now, Feature = "Auth" }, "Critical" },
            { new LogEntry { Message = "Critical failure in authentication", Level = LogLevel.Critical, Context = "Login", Timestamp = DateTime.Now, Feature = "Auth" }, "Critical" },
            { new LogEntry { Message = "Token expired unexpectedly", Level = LogLevel.Warning, Context = "Security", Timestamp = DateTime.Now, Feature = "Session" }, "High Priority" },
            { new LogEntry { Message = "UI rendered with missing components", Level = LogLevel.Warning, Context = "UI", Timestamp = DateTime.Now, Feature = "Render" }, "High Priority" },
            { new LogEntry { Message = "No data returned from API", Level = LogLevel.Warning, Context = "API", Timestamp = DateTime.Now, Feature = "API" }, "High Priority" },
            { new LogEntry { Message = "User logged out successfully", Level = LogLevel.Info, Context = "Session", Timestamp = DateTime.Now, Feature = "Auth" }, "Low Priority" },
            { new LogEntry { Message = "Database rollback complete", Level = LogLevel.Info, Context = "Database", Timestamp = DateTime.Now, Feature = "Rollback" }, "Low Priority" },
            { new LogEntry { Message = "Unexpected token in JSON", Level = LogLevel.Error, Context = "Parsing", Timestamp = DateTime.Now, Feature = "Parser" }, "Critical" },
            { new LogEntry { Message = "Deprecated API call", Level = LogLevel.Warning, Context = "API", Timestamp = DateTime.Now, Feature = "API" }, "High Priority" },
            { new LogEntry { Message = "File not found", Level = LogLevel.Error, Context = "Filesystem", Timestamp = DateTime.Now, Feature = "Files" }, "Critical" },
            { new LogEntry { Message = "Memory usage normal", Level = LogLevel.Info, Context = "Infra", Timestamp = DateTime.Now, Feature = "Monitoring" }, "Low Priority" },
            { new LogEntry { Message = "Network latency acceptable", Level = LogLevel.Info, Context = "Network", Timestamp = DateTime.Now, Feature = "Monitoring" }, "Low Priority" },
            { new LogEntry { Message = "Service crashed due to fatal error", Level = LogLevel.Critical, Context = "Infra", Timestamp = DateTime.Now, Feature = "Crash" }, "Critical" },
            { new LogEntry { Message = "Redirection took too long", Level = LogLevel.Warning, Context = "Routing", Timestamp = DateTime.Now, Feature = "UX" }, "High Priority" },
            { new LogEntry { Message = "Authentication successful", Level = LogLevel.Info, Context = "Login", Timestamp = DateTime.Now, Feature = "Auth" }, "Low Priority" },
            { new LogEntry { Message = "App upgraded to latest version", Level = LogLevel.Info, Context = "Settings", Timestamp = DateTime.Now, Feature = "Update" }, "Low Priority" },
            { new LogEntry { Message = "Deadlock detected in process", Level = LogLevel.Critical, Context = "Concurrency", Timestamp = DateTime.Now, Feature = "Threading" }, "Critical" },
            { new LogEntry { Message = "Warning: high CPU usage", Level = LogLevel.Warning, Context = "Infra", Timestamp = DateTime.Now, Feature = "Monitoring" }, "High Priority" },
            { new LogEntry { Message = "Manual sync required", Level = LogLevel.Info, Context = "Sync", Timestamp = DateTime.Now, Feature = "Manual" }, "Low Priority" },
            { new LogEntry { Message = "Unhandled exception occurred", Level = LogLevel.Error, Context = "Backend", Timestamp = DateTime.Now, Feature = "Runtime" }, "Critical" },
            { new LogEntry { Message = "Retry limit exceeded", Level = LogLevel.Warning, Context = "API", Timestamp = DateTime.Now, Feature = "Retry" }, "High Priority" },
            { new LogEntry { Message = "SSL handshake failed", Level = LogLevel.Critical, Context = "Security", Timestamp = DateTime.Now, Feature = "SSL" }, "Critical" }
        };

        var logs = labeledLogs.Keys.ToList();
        var expectedLabels = labeledLogs.Values.ToList();

        var analyzer = new LipsiLogAnalyzer(logs);
        var lipsiResults = analyzer.Analyze();

        Console.WriteLine("=== LIPSI Results ===");
        int correct = 0;
        var labelGroups = new Dictionary<string, (int correct, int total)>();

        for (int i = 0; i < logs.Count; i++)
        {
            //Console.WriteLine($"Text: {logs[i].Message} | Actual: {expectedLabels[i]} | LIPSI: {lipsiResults[i]}");
            if (!labelGroups.ContainsKey(expectedLabels[i]))
                labelGroups[expectedLabels[i]] = (0, 0);
            var (c, t) = labelGroups[expectedLabels[i]];
            if (lipsiResults[i] == expectedLabels[i]) { correct++; c++; }
            labelGroups[expectedLabels[i]] = (c, t + 1);
        }
        
        Console.WriteLine($"LIPSI MicroAccuracy: {(double)correct / logs.Count:P2}");
        var macro = labelGroups.Select(g => (double)g.Value.correct / g.Value.total).Average();
        Console.WriteLine($"LIPSI MacroAccuracy: {macro:P2}");

        var mlLogs = logs.Select((l, i) => new LogEntryML { Message = l.Message, Label = expectedLabels[i] }).ToList();
        var mlContext = new MLContext();
        var split = mlContext.Data.TrainTestSplit(mlContext.Data.LoadFromEnumerable(mlLogs), testFraction: 0.3);

        RunModel(mlContext, split.TrainSet, split.TestSet, "Logistic Regression",
            mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"));

        RunModel(mlContext, split.TrainSet, split.TestSet, "Naive Bayes",
            mlContext.MulticlassClassification.Trainers.NaiveBayes("Label", "Features"));

        RunModel(mlContext, split.TrainSet, split.TestSet, "FastTree",
            mlContext.MulticlassClassification.Trainers.OneVersusAll(
                mlContext.BinaryClassification.Trainers.FastTree()));

        RunModel(mlContext, split.TrainSet, split.TestSet, "LightGBM",
            mlContext.MulticlassClassification.Trainers.LightGbm("Label", "Features"));

        RunModel(mlContext, split.TrainSet, split.TestSet, "Averaged Perceptron",
            mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.AveragedPerceptron("Label", "Features")));

        RunModel(mlContext, split.TrainSet, split.TestSet, "Linear SVM",
            mlContext.MulticlassClassification.Trainers.OneVersusAll(
                mlContext.BinaryClassification.Trainers.LinearSvm()));

        RunModel(mlContext, split.TrainSet, split.TestSet, "Lbfgs Maximum Entropy",
            mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy("Label", "Features"));

        RunModel(mlContext, split.TrainSet, split.TestSet, "SDCA Non-Calibrated",
            mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated("Label", "Features"));
    }

    static void RunModel(MLContext mlContext, IDataView trainData, IDataView testData, string modelName, IEstimator<ITransformer> trainer)
    {
        var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
            .Append(mlContext.Transforms.Text.FeaturizeText("Features", nameof(LogEntryML.Message)))
            .Append(trainer)
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

        var model = pipeline.Fit(trainData);
        var predictions = model.Transform(testData);
        var metrics = mlContext.MulticlassClassification.Evaluate(predictions);

        Console.WriteLine($"\n-- {modelName} --");

        // MicroAccuracy — частка правильно класифікованих зразків серед усіх прикладів (зважена).
        Console.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy:P2}");

        // MacroAccuracy — середнє арифметичне точності по кожному класу (незважене, однакова вага кожному класу).
        Console.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy:P2}");

        var predEngine = mlContext.Model.CreatePredictionEngine<LogEntryML, LogPrediction>(model);
        var testSamples = mlContext.Data.CreateEnumerable<LogEntryML>(testData, reuseRowObject: false).ToList();

        // foreach (var entry in testSamples)
        // {
        //     var prediction = predEngine.Predict(entry);
        //     Console.WriteLine($"Text: {entry.Message} | Actual: {entry.Label} | {modelName}: {prediction.PredictedLabel}");
        // }
    }
}

// ✅ MicroAccuracy важливіша, коли:
// у тебе всі класи добре збалансовані (тобто, немає рідкісних класів);
// головне — загальна точність передбачень;
// тобі важливо, щоб якомога більше прикладів були класифіковані правильно, незалежно від їхньої категорії.
// Типове використання: бізнес-аналітика, де важливо правильно передбачити якомога більше кейсів загалом.

// ✅ MacroAccuracy важливіша, коли:
// у тебе є дисбаланс класів (наприклад, клас "Critical" рідкісний);
// важливо, щоб модель однаково добре класифікувала всі класи, навіть рідкісні;
// потрібно уникнути ситуації, коли модель "ігнорує" малочастотні, але важливі класи.
// Типове використання: медична діагностика, безпека, баг-трекінг — де важливо не пропустити критичні випадки, навіть якщо їх мало.