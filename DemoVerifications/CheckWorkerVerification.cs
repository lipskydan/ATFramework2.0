namespace DemoVerifications;

[TestFixture]
public class CheckWorkerVerification
{
    // [OneTimeTearDown]
    // public void DerivedTearDown() 
    // { 
    //     CheckWorker.FinalizeChecks();
    // }

    [Test]
    public void CheckWorker_A()
    {
        int expectedNumber = 10;
        string expectedString = "Hello World";
        DateTime expectedDate = new DateTime(2024, 1, 1);

        CheckWorker.Equals(expectedNumber, () => 12, message: "Numbers do not match"); // Fails
        CheckWorker.Equals(expectedString, () => "hello world", ignoreCase: false, message: "String comparison failed"); // Fails

        CheckWorker.Equals(42, () => 42, message: "This should pass"); // Passes
        CheckWorker.Contains("test", () => "unit testing", message: "This should pass too"); // Passes

        CheckWorker.FinalizeChecks();
    }

    [Test]
    public void CheckWorker_B()
    {
        DateTime expectedDate = new DateTime(2024, 1, 1);
        string expectedSubstring = "test";

        CheckWorker.DateTimeEquals(expectedDate, () => DateTime.Now, TimeSpan.FromSeconds(5), "Date does not match within tolerance"); // Fails
        CheckWorker.Contains(expectedSubstring, () => "automation tessting", message: "Substring not found"); // Fails

        CheckWorker.Equals(42, () => 42, message: "This should pass"); // Passes
        CheckWorker.Contains("test", () => "unit testing", message: "This should pass too"); // Passes

        CheckWorker.FinalizeChecks();
    }
}
