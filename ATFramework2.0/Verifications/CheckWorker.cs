using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

namespace ATFramework2._0.Verifications
{
    public class CheckWorker
    {
        private static readonly List<string> _failures = new List<string>();
        private static readonly LogWorker _logWorker = new LogWorker("CheckWorkerLog.txt");

        private static T ExecuteWithRetry<T>(Func<T> action, Func<T, bool> condition, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
        {
            TimeSpan maxTime = maxInterval ?? TimeSpan.FromSeconds(30);
            TimeSpan interval = checkInterval ?? TimeSpan.FromSeconds(5);
            DateTime endTime = DateTime.Now.Add(maxTime);
            T result;

            while (DateTime.Now < endTime)
            {
                result = action();
                if (condition(result))
                {
                    return result;
                }
                Thread.Sleep(interval);
            }

            return action(); // Last attempt outside the loop
        }

        public static void Equals<T>(T exp, Func<T> act, bool ignoreCase = false, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
        {
            try
            {
                var actualValue = ExecuteWithRetry(act, actual => ignoreCase && typeof(T) == typeof(string)
                    ? string.Equals(exp as string, actual as string, StringComparison.OrdinalIgnoreCase)
                    : EqualityComparer<T>.Default.Equals(exp, actual), maxInterval, checkInterval);

                string finalMessage = message ?? $"Expected: {exp}, Actual: {actualValue}";
                if (ignoreCase && typeof(T) == typeof(string))
                {
                    Assert.That(actualValue as string, Is.EqualTo(exp as string).IgnoreCase, finalMessage);
                }
                else
                {
                    Assert.That(actualValue, Is.EqualTo(exp), finalMessage);
                }

                _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "Equals");
            }
            catch (AssertionException ex)
            {
                _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "Equals");
                throw;
            }
        }

        public static void NotEquals<T>(T exp, Func<T> act, bool ignoreCase = false, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
        {
            try
            {
                var actualValue = ExecuteWithRetry(act, actual => !(ignoreCase && typeof(T) == typeof(string)
                    ? string.Equals(exp as string, actual as string, StringComparison.OrdinalIgnoreCase)
                    : EqualityComparer<T>.Default.Equals(exp, actual)), maxInterval, checkInterval);

                string finalMessage = message ?? $"Expected value not to be: {exp}, Actual: {actualValue}";
                if (ignoreCase && typeof(T) == typeof(string))
                {
                    Assert.That(actualValue as string, Is.Not.EqualTo(exp as string).IgnoreCase, finalMessage);
                }
                else
                {
                    Assert.That(actualValue, Is.Not.EqualTo(exp), finalMessage);
                }

                _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "NotEquals");
            }
            catch (AssertionException ex)
            {
                _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "NotEquals");
                throw;
            }
        }

        public static void Contains(string substring, Func<string> act, bool ignoreCase = false, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
        {
            try
            {
                var actualValue = ExecuteWithRetry(act, actual => ignoreCase
                    ? actual.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0
                    : actual.Contains(substring), maxInterval, checkInterval);

                string finalMessage = message ?? $"Expected: {actualValue} to contain: {substring}";
                if (ignoreCase)
                {
                    Assert.That(actualValue, Does.Contain(substring).IgnoreCase, finalMessage);
                }
                else
                {
                    Assert.That(actualValue, Does.Contain(substring), finalMessage);
                }

                _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "Contains");
            }
            catch (AssertionException ex)
            {
                _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "Contains");
                throw;
            }
        }

        public static void Matches(string pattern, Func<string> act, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
        {
            try
            {
                var actualValue = ExecuteWithRetry(act, actual => Regex.IsMatch(actual, pattern), maxInterval, checkInterval);

                string finalMessage = message ?? $"Expected: {actualValue} to match pattern: {pattern}";
                Assert.That(actualValue, Does.Match(pattern), finalMessage);

                _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "Matches");
            }
            catch (AssertionException ex)
            {
                _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "Matches");
                throw;
            }
        }

        public static void DateTimeEquals(DateTime expected, Func<DateTime> act, TimeSpan tolerance, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
        {
            try
            {
                var actualValue = ExecuteWithRetry(act, actual => (actual - expected).Duration() <= tolerance, maxInterval, checkInterval);

                string finalMessage = message ?? $"Expected: {expected} ± {tolerance.TotalSeconds} seconds, Actual: {actualValue}";
                Assert.That(actualValue, Is.EqualTo(expected).Within(tolerance), finalMessage);

                _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "DateTimeEquals");
            }
            catch (AssertionException ex)
            {
                _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "DateTimeEquals");
                throw;
            }
        }

        public static void DateTimeNotEquals(DateTime expected, Func<DateTime> act, TimeSpan tolerance, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
        {
            try
            {
                var actualValue = ExecuteWithRetry(act, actual => (actual - expected).Duration() > tolerance, maxInterval, checkInterval);

                string finalMessage = message ?? $"Expected: {expected} ± {tolerance.TotalSeconds} seconds NOT to match, Actual: {actualValue}";
                Assert.That(actualValue, Is.Not.EqualTo(expected).Within(tolerance), finalMessage);

                _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "DateTimeNotEquals");
            }
            catch (AssertionException ex)
            {
                _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "DateTimeNotEquals");
                throw;
            }
        }

        public static void ListEquals<T>(IEnumerable<T> expected, Func<IEnumerable<T>> act, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
        {
            try
            {
                var actualValue = ExecuteWithRetry(act, actual => Enumerable.SequenceEqual(expected, actual), maxInterval, checkInterval);

                string finalMessage = message ?? $"Expected: [{string.Join(", ", expected)}], Actual: [{string.Join(", ", actualValue)}]";
                Assert.That(actualValue, Is.EqualTo(expected), finalMessage);

                _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "ListEquals");
            }
            catch (AssertionException ex)
            {
                _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "ListEquals");
                throw;
            }
        }

        public static void ListEqualsIgnoringOrder<T>(IEnumerable<T> expected, Func<IEnumerable<T>> act, string? message = null, TimeSpan? maxInterval = null, TimeSpan? checkInterval = null)
        {
            try
            {
                var actualValue = ExecuteWithRetry(act, actual => new HashSet<T>(expected).SetEquals(actual), maxInterval, checkInterval);

                string finalMessage = message ?? $"Expected (ignoring order): [{string.Join(", ", expected.OrderBy(x => x))}], Actual: [{string.Join(", ", actualValue.OrderBy(x => x))}]";
                Assert.That(actualValue.OrderBy(x => x), Is.EqualTo(expected.OrderBy(x => x)), finalMessage);

                _logWorker.Log($"Verification passed: {finalMessage}", LogLevel.Info, "VerifyWorker", "ListEqualsIgnoringOrder");
            }
            catch (AssertionException ex)
            {
                _logWorker.Log($"Verification failed: {ex.Message}", LogLevel.Error, "VerifyWorker", "ListEqualsIgnoringOrder");
                throw;
            }
        }

        public static void FinalizeChecks()
        {
            if (_failures.Any())
            {
                var combinedMessage = string.Join(Environment.NewLine, _failures.Select((msg, index) => $"{index + 1}. {msg}"));
                _failures.Clear();
                throw new AssertionException($"The following checks failed:{Environment.NewLine}{combinedMessage}");
            }
        }
    }
}