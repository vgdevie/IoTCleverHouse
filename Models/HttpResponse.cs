namespace ICH
{
    public class HTTPResponse
    {
        /// <summary>
        /// Http Response.
        /// </summary>
        /// <param name="text">Text of response.</param>
        /// <param name="error">If error ocured.</param>
        /// <param name="code">Http Response code.</param>
        public HTTPResponse(string text, bool error = false, string code = "200 OK")
        {
            Text = text;
            Error = error;
            Code = error && code == "200 OK" ? "500" : code;
            //Adding response to log.
            if (error)
            CleverHouse.Log.Add(text, error);
        }
         
        public string Text { get; }

        public bool Error { get; }

        public string Code { get; }
    }
}
