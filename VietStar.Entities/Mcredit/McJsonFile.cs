using System;
using System.Collections.Generic;
using System.Text;

namespace VietStar.Entities.Mcredit
{
    public class McJsonFile
    {
        public McJsonFile()
        {
            groups = new List<McJsonFileGroup>();
        }
        public List<McJsonFileGroup> groups { get; set; }
    }
    public class McJsonFileGroup
    {
        public McJsonFileGroup()
        {
            docs = new List<MCJsonFileGroupDoc>();
        }
        public int id { get; set; }
        public List<MCJsonFileGroupDoc> docs { get; set; }
    }
    public class MCJsonFileGroupDoc
    {
        public MCJsonFileGroupDoc()
        {
            files = new List<MCJsonFileGroupDocFile>();
        }
        public string code { get; set; }
        public List<MCJsonFileGroupDocFile> files { get; set; }
    }
    public class MCJsonFileGroupDocFile
    {
        public string name { get; set; }
    }
}
