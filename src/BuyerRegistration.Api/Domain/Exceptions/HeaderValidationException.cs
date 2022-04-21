﻿using System;

namespace BuyerRegistration.Api.Domain.Exceptions
{
    /// <summary>
    /// Custom exception thrown from HeaderValidationMiddleware in case of a missing request header.
    /// Handled globally in ExceptionMiddleware.
    /// </summary>
    public class HeaderValidationException : Exception
    {
    }
}