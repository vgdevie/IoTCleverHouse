using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICH
{
    public class HTTPServerFile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Path to file. DO NOT USE "api/" path.</param>
        /// <param name="content">Content of the file.</param>
        public HTTPServerFile(string path, string content)
        {
            Path = path;
            Content = content;
        }

        public string Path { get; set; }
        public string Content { get; set; }
    }
}
