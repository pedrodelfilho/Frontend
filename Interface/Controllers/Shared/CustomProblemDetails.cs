using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Interface.Controllers.Shared
{
    public class CustomProblemDetails : ProblemDetails
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> Errors { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="detail"></param>
        /// <param name="errors"></param>
        public CustomProblemDetails(HttpStatusCode status, string detail = null, IEnumerable<string> errors = null) : this()
        {
            Title = status switch
            {
                HttpStatusCode.BadRequest => "One or more validation errors occurred.",
                HttpStatusCode.InternalServerError => "Internal server error.",
                _ => "An error has occurred."
            };

            Status = (int)status;
            Detail = detail;

            if (errors is not null)
            {
                if (errors.Count() == 1)
                    Detail = errors.First();
                else if (errors.Count() > 1)
                    Detail = "Multiple problems have occurred.";

                Errors.AddRange(errors);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <param name="request"></param>
        /// <param name="detail"></param>
        /// <param name="errors"></param>
        public CustomProblemDetails(HttpStatusCode status, HttpRequest request, string detail = null, IEnumerable<string> errors = null) : this(status, detail, errors) =>
            Instance = request.Path;

        private CustomProblemDetails() =>
            Errors = new List<string>();
    }
}
