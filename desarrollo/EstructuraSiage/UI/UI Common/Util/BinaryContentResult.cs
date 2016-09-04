using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SIAGE.UI_Common.Util
{
    public class BinaryContentResult : ActionResult
    {
        private readonly string _contentType;
        private readonly byte[] _contentBytes;

        public BinaryContentResult(byte[] contentBytes, string contentType)
        {
            _contentBytes = contentBytes;
            _contentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = _contentType;

            var stream = new MemoryStream(_contentBytes);
            stream.WriteTo(response.OutputStream);
            stream.Dispose();
        }
    }
}