using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace bike_selling_app.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());

            // Create an error string that bubbles up to the client
            var errorString = "";
            foreach (KeyValuePair<string, string[]> kp in Errors)
            {
                errorString += $"Validation Error: {kp.Value.ToString()} on property {kp.Key}\n";
            }
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}