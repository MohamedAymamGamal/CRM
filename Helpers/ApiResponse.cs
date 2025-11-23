using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class ApiResponse
    {
        public static object Success(object? data= null , string? message = "Success"){
            return new
            {
                success = true,
                data = data,
                message 
            };
        }

        public static object Error(object? data= null , string? message = "Error"){
            return new
            {
                success = false,
                data = data,
                message 
            };
        }
       
    }
}