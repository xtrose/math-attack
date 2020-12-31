using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlaybackAgent1
{
    class Audio
    {

        public Uri Source { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
 
        public Audio(Uri source)
        {
            Source = source;
        }


    }
}
