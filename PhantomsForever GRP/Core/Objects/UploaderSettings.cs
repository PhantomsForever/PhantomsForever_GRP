using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhantomsForever_GRP.Core.Objects
{
    public class UploaderSettings
    {
        public UploaderSettings()
        {
        }
        public UploaderSettings(UploaderSettings copy)
        {
            this.name = copy.name;
            this.enabled = copy.enabled;
            this.fileExt = copy.fileExt;
            this.folder = copy.folder;
            this.bucket = copy.bucket;
            this.stopAndWait = copy.stopAndWait;
            this.bundle = copy.bundle;
            this.renameInsteadOfDeletePattern = copy.renameInsteadOfDeletePattern;
        }
        public string name;
        public bool enabled;
        public string fileExt;
        public string folder;
        public string bucket;
        public bool stopAndWait;
        public bool bundle;
        public string renameInsteadOfDeletePattern;
    }
}