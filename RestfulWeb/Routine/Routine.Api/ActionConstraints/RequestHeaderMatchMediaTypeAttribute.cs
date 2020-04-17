using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.ActionConstraints
{
    [AttributeUsage(AttributeTargets.All,AllowMultiple = true,Inherited =true)]
    public class RequestHeaderMatchMediaTypeAttribute : Attribute, IActionConstraint
    {
        private readonly MediaTypeCollection _mediaTypes = new MediaTypeCollection();
        private readonly string requestHeaderToMatch;

        public RequestHeaderMatchMediaTypeAttribute(string requestHeaderToMatch,string mediaType,params string[]othreMediaTypes)
        {
            this.requestHeaderToMatch = requestHeaderToMatch??throw new ArgumentNullException(nameof(requestHeaderToMatch));
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue mediaTypeHeaderValue)) throw new ArgumentException(nameof(mediaType));
            this._mediaTypes.Add(mediaTypeHeaderValue);
            foreach (var othreMediaType in othreMediaTypes)
            {
                if (!MediaTypeHeaderValue.TryParse(othreMediaType, out MediaTypeHeaderValue parsedMediaType)) throw new ArgumentException(nameof(othreMediaType));
                this._mediaTypes.Add(parsedMediaType);
            }
        }
        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var headRequest = context.RouteContext.HttpContext.Request.Headers;
            if (!headRequest.ContainsKey(this.requestHeaderToMatch)) return false;
            var paredMediaType=new MediaType(headRequest[this.requestHeaderToMatch]);
            var debug= this._mediaTypes.Any(x => {
                var itemmediatype = new MediaType(x);
                if (paredMediaType.Equals(itemmediatype)) return true;
                else return false;
            });
            return debug;
        }
    }
}
