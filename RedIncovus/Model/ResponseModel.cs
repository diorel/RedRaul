using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class ResponseModel
    {
        public bool response { get; set; }
        public List<Object> results { get; set; }
        public Object result { get; set; }
        public string message { get; set; }
        public string function { get; set; }
        public string href { get; set; }

        public ResponseModel() {
            response = false;
            message = "Ocurrio un error inesperado";
        }

        public void SetResponse(bool r, string msg = null)
        {
            if (!r && msg == "") message = "Ocurrio un error inesperado";
            else message = msg;

            response = r;
        }
    }
}
