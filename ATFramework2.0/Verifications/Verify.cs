﻿namespace ATFramework2._0.Verifications;

public class Verify
{
    public static void StringEquals(string exp, string act) => Assert.That(act, Is.EqualTo(exp));
}
