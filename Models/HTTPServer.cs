using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace ICH
{
    public class HTTPServer
    {

        #region Variables

        private StreamSocketListener listener;

        private const uint BufferSize = 8192;

        private int _port;

        private List<HTTPServerFile> Files = new List<HTTPServerFile>();

        #endregion

        /// <summary>
        /// Initializes Http server.
        /// </summary>
        /// <param name="files">Files of simple website that to be hosted.</param>
        /// <param name="port">Port used.</param>
        public async void Initialize(int port = 80, List<HTTPServerFile> files = null)
        {
            _port = port;

            //Preparing default file
            if (files == null)
            {
                files = new List<HTTPServerFile>();
                files.Add(new HTTPServerFile("index.html", "<html><body>IoT Clever House</body></html>"));
            }

            try
            {
                listener = new StreamSocketListener();
                
                //Listen port.
                await listener.BindServiceNameAsync(port.ToString());

                //Adding event handler.
                listener.ConnectionReceived += HandleRequest;

                Initialized = true;

                CleverHouse.Log.Add($"WebServer initialized on port {port}.");
            }
            catch (Exception e)
            {
                CleverHouse.Log.Add($"Error initializing Web Server ({e.Message}).");
            }
        }

        /// <summary>
        /// If Http Server initialized.
        /// </summary>
        public bool Initialized { get; set; } = false;

        /// <summary>
        /// Handling request.
        /// </summary>
        private async void HandleRequest(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            StringBuilder request = new StringBuilder();

            // Handle a incoming request
            // First read the request
            using (IInputStream input = args.Socket.InputStream)
            {
                byte[] data = new byte[BufferSize];
                IBuffer buffer = data.AsBuffer();
                uint dataRead = BufferSize;
                while (dataRead == BufferSize)
                {
                    await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                    request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                    dataRead = buffer.Length;
                }
            }

            var response = await PrepareResponse(ParseRequest(request.ToString()));

            // Send a response
            using (IOutputStream output = args.Socket.OutputStream)
            {
                using (Stream ow = output.AsStreamForWrite())
                {
                    // For now we are just going to reply to anything with Hello World!
                    byte[] bodyArray = Encoding.UTF8.GetBytes(response.Text);

                    var bodyStream = new MemoryStream(bodyArray);

                    // Preparing header
                    var header = "HTTP/1.1 " + response.Code + "\r\n" +
                                $"Content-Length: {bodyStream.Length}\r\n" +
                                    "Access-Control-Allow-Credentials: true\r\n" +
                                    "Access-Control-Allow-Origin: *\r\n" +
                                    "Connection: close\r\n\r\n"; 

                    byte[] headerArray = Encoding.UTF8.GetBytes(header);

                    // Send the header with the body included to the client
                    await ow.WriteAsync(headerArray, 0, headerArray.Length);
                    await bodyStream.CopyToAsync(ow);
                    await ow.FlushAsync();
                }
            }


        }

        /// <summary>
        /// Preparing response.
        /// </summary>
        private async Task<HTTPResponse> PrepareResponse(string request)
        {
            try
            {
                //Index
                if (request == "")
                {
                    return new HTTPResponse(Files.Where(f => f.Path.Contains("index")).Single().Content);
                }

                //Api
                if (request.Contains("/") && request.Split('/')[0] == "api")
                {
                    return await CleverHouse.Api.Comand(request.Remove(0, 4));
                }

                //File
                return new HTTPResponse(Files.Where(f => f.Path.Contains(request)).Single().Content);
            }
            catch
            {
                return new HTTPResponse("<h1>404 Not Found</h1>", true, "404");
            }
        }

        
        /// <summary>
        /// Parsing request
        /// </summary>
        private string ParseRequest(string buffer)
        {
            string request = "ERROR";

            string[] tokens = buffer.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            // ensure that this is a GET request
            if (tokens[0] == "GET")
            {
                request = tokens[1];
                request = request.ToLower().Remove(0,1);
            }

            return request;
        }
    }
}