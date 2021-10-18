﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Logger
{
    public interface ILoggerService
    {
        void LogInformation(string message);
        void LogInformation(string message, Exception ex);
        void LogAdvertencia(string message);
        void LogAdvertencia(string message, Exception ex);
        void LogError(string message);
        void LogError(string message, Exception ex);

    }
}
