﻿namespace NNSoft.PL.Common
{
    public enum ServiceState
    {
        Stopped = 1,
        StartPending = 2,
        StopPending = 3,
        Running = 4,
        ContinuePending = 5,
        PausePending = 6,
        Paused = 7
    }
}
