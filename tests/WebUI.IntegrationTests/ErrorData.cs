using System.Collections.Generic;

namespace bike_selling_app.WebUI.IntegrationTests
{
    // This matches the schema
    //         "data": {
    //           "details": "One or more validation failures have occurred.",
    //           "errorType": "validation"
    //         }
    public class ErrorData 
    {
        public string details { get; set; }
        public string errorType { get; set; }
    }
}