using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Nacecode
    {
        public string Code { get; private set; }

        private string _beschrijving = string.Empty;
        public string Beschrijving
        {
            get { return _beschrijving; }
            set
            {
                if (value != null && value.Length > 200)
                    throw new ArgumentException("Beschrijving mag niet langer zijn dan 200 tekens.");
                _beschrijving = value ?? string.Empty;
            }
        }

        public string ParentCode { get; set; }

        public Nacecode()
        {
            _beschrijving = string.Empty;
            ParentCode = null;
        }

        public Nacecode(string code, string beschrijving, string parentCode)
        {
            Code = code;
            Beschrijving = beschrijving;
            ParentCode = parentCode;
        }
    }
}
