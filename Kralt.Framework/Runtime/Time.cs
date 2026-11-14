using System;
using System.Diagnostics;

#nullable disable

namespace Kralt.Framework.Runtime;

/// <summary>
/// Stores various time helper functions and properties
/// </summary>
public static class Time
{
    private static readonly Stopwatch timer = Stopwatch.StartNew();

    /// <summary>
    /// Total time (in seconds) since startup
    /// </summary>
    public static double runtime { get; private set; }

    /// <summary>
    /// Amount of time (in seconds) between the previous frame and this one
    /// </summary>
    public static float deltaTime { get; private set; }

    /// <summary>
    /// Number of frames that have passed since startup
    /// </summary>
    public static ulong FrameCount { get; private set; }

    public static void UpdateTime(float delta)
    {
        FrameCount++;
        runtime = timer.Elapsed.TotalSeconds;
        deltaTime = delta;
    }
}
