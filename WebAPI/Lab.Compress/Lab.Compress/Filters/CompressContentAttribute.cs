﻿using System.IO.Compression;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lab.Compress.Filters
{
    public class CompressContentAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///     Override to compress the content that is generated by
        ///     an action method.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            GZipEncodePage();
        }

        private static void GZipEncodePage()
        {
            var Response = HttpContext.Current.Response;

            if (IsGZipSupported())
            {
                var AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

                if (AcceptEncoding.Contains("deflate"))
                {
                    Response.Filter = new DeflateStream(Response.Filter,
                                                        CompressionMode.Compress);

                    Response.AppendHeader("Content-Encoding", "deflate");
                }
                else
                {
                    Response.Filter = new GZipStream(Response.Filter,
                                                     CompressionMode.Compress);

                    //Response.Headers.Remove("Content-Encoding");

                    Response.AppendHeader("Content-Encoding", "gzip");
                }
            }

            // Allow proxy servers to cache encoded and unencoded versions separately
            Response.AppendHeader("Vary", "Content-Encoding");
        }

        /// <summary>
        ///     Determines if GZip is supported
        /// </summary>
        /// <returns></returns>
        private static bool IsGZipSupported()
        {
            var AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(AcceptEncoding) &&
                (AcceptEncoding.Contains("gzip") || AcceptEncoding.Contains("deflate")))
            {
                return true;
            }

            return false;
        }
    }
}