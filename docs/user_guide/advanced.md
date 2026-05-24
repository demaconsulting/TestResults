# Advanced Usage

## Using TestOutcome Extension Methods

The library provides extension methods for categorizing test outcomes at a glance:

```csharp
using System.Linq;
using DemaConsulting.TestResults;

var results = new TestResults { Name = "Test Analysis" };

results.Results.Add(new TestResult { Name = "Test1", Outcome = TestOutcome.Passed });
results.Results.Add(new TestResult { Name = "Test2", Outcome = TestOutcome.Failed });
results.Results.Add(new TestResult { Name = "Test3", Outcome = TestOutcome.NotExecuted });
results.Results.Add(new TestResult { Name = "Test4", Outcome = TestOutcome.Warning });

int passedCount  = results.Results.Count(r => r.Outcome.IsPassed());   // Passed + Warning + PassedButRunAborted
int failedCount  = results.Results.Count(r => r.Outcome.IsFailed());   // Failed + Error + Timeout + Aborted
int executedCount = results.Results.Count(r => r.Outcome.IsExecuted()); // All except NotExecuted, NotRunnable, Pending

double successRate = executedCount > 0 ? (double)passedCount / executedCount * 100 : 0;
Console.WriteLine($"Passed: {passedCount}, Failed: {failedCount}, Success rate: {successRate:F2}%");
```

## Aggregating Results from Multiple Sources

Combine test results from different test suites or CI stages into a single collection:

```csharp
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

// Deserialize each file — format is detected automatically
var unitTests        = Serializer.Deserialize(File.ReadAllText("unit-tests.xml"));
var integrationTests = Serializer.Deserialize(File.ReadAllText("integration-tests.xml"));
var e2eTests         = Serializer.Deserialize(File.ReadAllText("e2e-tests.trx"));

// Merge all results into one collection
var combined = new TestResults
{
    Name = "All Tests Combined",
    UserName = Environment.UserName
};

combined.Results.AddRange(unitTests.Results);
combined.Results.AddRange(integrationTests.Results);
combined.Results.AddRange(e2eTests.Results);

File.WriteAllText("combined-results.trx", TrxSerializer.Serialize(combined));
File.WriteAllText("combined-results.xml", JUnitSerializer.Serialize(combined));
Console.WriteLine($"Combined {combined.Results.Count} tests from 3 sources");
```

## Filtering Test Results

Save subsets of a result set — for example failed tests or tests belonging to a specific class:

```csharp
using System.Linq;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

var allTests = JUnitSerializer.Deserialize(File.ReadAllText("all-tests.xml"));

// Failed tests only
var failedResults = new TestResults
{
    Name = "Failed Tests Only",
    Results = allTests.Results.Where(r => r.Outcome.IsFailed()).ToList()
};
File.WriteAllText("failed-tests.xml", JUnitSerializer.Serialize(failedResults));

// Tests belonging to a specific class
var databaseTests = new TestResults
{
    Name = "Database Tests",
    Results = allTests.Results.Where(r => r.ClassName.Contains("DatabaseTests")).ToList()
};
File.WriteAllText("database-tests.xml", JUnitSerializer.Serialize(databaseTests));

// Slow tests (duration over 1 second), sorted slowest first
var slowTests = new TestResults
{
    Name = "Slow Tests (>1 second)",
    Results = allTests.Results
        .Where(r => r.Duration.TotalSeconds > 1)
        .OrderByDescending(r => r.Duration)
        .ToList()
};
File.WriteAllText("slow-tests.xml", JUnitSerializer.Serialize(slowTests));

Console.WriteLine($"Failed: {failedResults.Results.Count}, Slow: {slowTests.Results.Count}");
```

## Generating Summary Statistics

Calculate pass/fail/skip counts and timing from a result set:

```csharp
using System.Linq;
using DemaConsulting.TestResults;
using DemaConsulting.TestResults.IO;

var results = JUnitSerializer.Deserialize(File.ReadAllText("test-results.xml"));

var total    = results.Results.Count;
var passed   = results.Results.Count(r => r.Outcome.IsPassed());
var failed   = results.Results.Count(r => r.Outcome.IsFailed());
var skipped  = results.Results.Count(r => !r.Outcome.IsExecuted());
var totalDur = TimeSpan.FromTicks(results.Results.Sum(r => r.Duration.Ticks));
var avgDur   = total > 0
    ? TimeSpan.FromTicks((long)results.Results.Average(r => r.Duration.Ticks))
    : TimeSpan.Zero;

Console.WriteLine($"Total:    {total}");
Console.WriteLine($"Passed:   {passed} ({(total > 0 ? (double)passed / total * 100 : 0):F1}%)");
Console.WriteLine($"Failed:   {failed} ({(total > 0 ? (double)failed / total * 100 : 0):F1}%)");
Console.WriteLine($"Skipped:  {skipped} ({(total > 0 ? (double)skipped / total * 100 : 0):F1}%)");
Console.WriteLine($"Duration: {totalDur:hh\\:mm\\:ss\\.fff} total, {avgDur:mm\\:ss\\.fff} avg");
Console.WriteLine($"Status:   {(failed == 0 ? "SUCCESS" : "FAILURE")}");
```

## Thread Safety

`TestResults` and `TestResult` are simple data-transfer objects (DTOs). They are not thread-safe. If multiple
threads add results concurrently, use appropriate synchronization:

```csharp
var results = new TestResults { Name = "Parallel Tests" };
var lockObject = new object();

Parallel.ForEach(testCases, testCase =>
{
    var result = ExecuteTest(testCase);
    lock (lockObject)
    {
        results.Results.Add(result);
    }
});
```

The serializer classes (`TrxSerializer`, `JUnitSerializer`, and `Serializer`) are stateless and safe to call from
multiple threads simultaneously.

## File I/O Responsibility

The library does not perform any file I/O itself. You control file paths, directory creation, and error handling:

```csharp
var outputPath = Path.Combine("TestResults", "results.trx");
Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

string xml = TrxSerializer.Serialize(results);
File.WriteAllText(outputPath, xml);
```
