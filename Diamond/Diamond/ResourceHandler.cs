using CefSharp;
using Diamond.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Diamond
{
    public class ResourceHandler : CefSharp.ResourceHandler
    {
        IRequest request;
        long responseLength;
        Stream stream;
        string mimeType;
        Controller controller;

        public ResourceHandler(IRequest request, Controller controller)
        {
            this.controller = controller;
            this.request = request;
        }

        //public override Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl)
        //{
        //    if (finished)
        //    {
        //        response.MimeType = mimeType;
        //        responseLength = this.responseLength;
        //        redirectUrl = this.redirectUrl;

        //        return stream;
        //    }

        //    Uri uri = new Uri(request.Url);
        //    String file = uri.Authority + uri.AbsolutePath;

        //    Assembly assembly = Assembly.GetExecutingAssembly();
        //    //String resourcePath = assembly.GetName().Name + "." + file.Replace("/", ".");
        //    String resourcePath = "ClassesViewer." + file.Replace("/", ".");

        //    if (assembly.GetManifestResourceInfo(resourcePath) != null)
        //    {
        //        stream = assembly.GetManifestResourceStream(resourcePath);
        //        mimeType = MimeTypes.GetMimeType(Path.GetExtension(file));
        //        responseLength = stream.Length;
        //        redirectUrl = null;
        //        finished = true;
        //        return stream;
        //    }

        //    //Else 404
        //    byte[] error404 = Encoding.UTF8.GetBytes("404");
        //    responseLength = error404.Length;
        //    redirectUrl = null;
        //    stream = new MemoryStream(error404);
        //    finished = true;
        //    return stream;
        //}

        public override bool ProcessRequestAsync(IRequest request, ICallback callback)
        {
            this.request = request;

            string url = this.request.Url;

            Task task = new Task(() =>
            {
                //GetResponse(null, out responseLength, out redirectUrl);
                //StatusCode, StatusText, MimeType, ResponseLength and Stream

                Uri uri = new Uri(url);
                String file = uri.Authority + uri.AbsolutePath;

                if(file.StartsWith("root/"))
                {
                    file = file.Substring(5);
                }

                Assembly assembly = Assembly.GetExecutingAssembly();
                //String resourcePath = assembly.GetName().Name + "." + file.Replace("/", ".");
                String resourcePath = "Diamond.web." + file.Replace("/", ".");

                if(string.IsNullOrEmpty(Path.GetExtension(file)))
                {
                    //Directory browser
                }
                else if(ResourceIdentifier.IsResourceUrl(file))
                {
                    stream = controller.ProcessTemplate(new ResourceIdentifier(file));

                    mimeType = MimeTypes.GetMimeType(".html");

                    responseLength = stream.Length;
                    Stream = stream;
                }
                else if (assembly.GetManifestResourceInfo(resourcePath) != null)
                {
                    stream = assembly.GetManifestResourceStream(resourcePath);
                    mimeType = MimeTypes.GetMimeType(Path.GetExtension(file));

                    responseLength = stream.Length;
                    Stream = stream;
                }
                else
                {
                    //Else 404
                    byte[] error404 = Encoding.UTF8.GetBytes("404");
                    responseLength = error404.Length;
                    Stream = new MemoryStream(error404);
                    mimeType = "text/html";
                }

                StatusText = "OK";
                MimeType = mimeType;
                ResponseLength = responseLength;

                callback.Continue();
            });

            task.Start();

            return true;
        }
    }
}
