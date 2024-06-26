using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message){
            response.Headers.Add("ApplicationError", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}