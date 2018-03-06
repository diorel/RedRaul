using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Helper
{
    public class ImageHelper
    {
        private string _path = "";
        private string _img;
        private int[] _scales = { 400, 250, 45 };

        private List<string> _images = new List<string>();
        public string Path
        {
            get { return this._path; }
            set { this._path = value; }
        }
        public string Img
        {
            get { return this._img; }
            set { this._img = value; }
        }
        public int[] Scales
        {
            get { return this._scales; }
            set { this._scales = value; }
        }
        public List<string> getNewImages()
        {
            return this._images;
        }

        public void resizes()
        {
            Image objImg = null;
            int i = 0;
            foreach (int scale in this.Scales)
            {
                objImg = Image.FromFile(this.Path + this.Img);
                //Cuando el ancho es mas grande que el alto en base a la medida que deseamos
                if (objImg.Width > scale & objImg.Height < scale)
                {
                    decimal _width = scale;
                    decimal _height = (objImg.Height) * ((decimal)scale / objImg.Width);

                    Bitmap _img = new Bitmap(objImg, new Size(Convert.ToInt32(_width), Convert.ToInt32(_height)));
                    objImg.Dispose();

                    if (i == 0)
                    {
                        _img.Save(this.Path + this.Img, ImageFormat.Jpeg);
                        _images.Add(this.Img);
                    }
                    else
                    {
                        _img.Save(this.Path + i + "-" + this.Img, ImageFormat.Jpeg);
                        _images.Add(i + "-" + this.Img);
                    }
                }
                else if (objImg.Width < scale & objImg.Height > scale)
                {
                    decimal _width = objImg.Width * ((decimal)scale / objImg.Height);
                    decimal _height = scale;

                    Bitmap _img = new Bitmap(objImg, new Size(Convert.ToInt32(_width), Convert.ToInt32(_height)));
                    objImg.Dispose();

                    if (i == 0)
                    {
                        _img.Save(this.Path + this.Img, ImageFormat.Jpeg);
                        _images.Add(this.Img);
                    }
                    else
                    {
                        _img.Save(this.Path + i + "-" + this.Img, ImageFormat.Jpeg);
                        _images.Add(i + "-" + this.Img);
                    }
                }
                else if (objImg.Width > scale & objImg.Height > scale)
                {
                    //Cuando el ancho y el alto son mas grandes que la medida que deseamos
                    decimal _width = 0;
                    decimal _height = 0;
                    if (objImg.Width > objImg.Height)
                    {
                        _width = scale;
                        _height = (objImg.Height) * ((decimal)scale / objImg.Width);
                    }
                    else
                    {
                        _width = objImg.Width * ((decimal)scale / objImg.Height);
                        _height = scale;
                    }
                    Bitmap _img = new Bitmap(objImg, new Size(Convert.ToInt32(_width), Convert.ToInt32(_height)));
                    objImg.Dispose();

                    if (i == 0)
                    {
                        _img.Save(this.Path + this.Img, ImageFormat.Jpeg);
                        _images.Add(this.Img);
                    }
                    else
                    {
                        _img.Save(this.Path + i + "-" + this.Img, ImageFormat.Jpeg);
                        _images.Add(i + "-" + this.Img);
                    }
                }
                else
                {
                    _images.Add(this.Img);
                }
                i += 1;
            }
        }
        public static void resize(string img, int scale)
        {
            Image objImg = null;

            objImg = Image.FromFile(img);
            //Cuando el ancho es mas grande que el alto en base a la medida que deseamos
            if (objImg.Width > scale & objImg.Height < scale)
            {
                decimal _width = scale;
                decimal _height = (objImg.Height) * ((decimal)scale / objImg.Width);

                Bitmap _img = new Bitmap(objImg, new Size(Convert.ToInt32(_width), Convert.ToInt32(_height)));
                objImg.Dispose();
                _img.Save(img, ImageFormat.Jpeg);
                //Cuando el alto es mas grande que el ancho en base a la medida que deseamos
            }
            else if (objImg.Width < scale & objImg.Height > scale)
            {
                decimal _width = objImg.Width * ((decimal)scale / objImg.Height);
                decimal _height = scale;

                Bitmap _img = new Bitmap(objImg, new Size(Convert.ToInt32(_width), Convert.ToInt32(_height)));
                objImg.Dispose();
                _img.Save(img, ImageFormat.Jpeg);
            }
            else if (objImg.Width > scale & objImg.Height > scale)
            {
                //Cuando el ancho y el alto son mas grandes que la medida que deseamos
                decimal _width = 0;
                decimal _height = 0;
                if (objImg.Width > objImg.Height)
                {
                    _width = scale;
                    _height = (objImg.Height) * ((decimal)scale / objImg.Width);
                }
                else
                {
                    _width = objImg.Width * ((decimal)scale / objImg.Height);
                    _height = scale;
                }
                Bitmap _img = new Bitmap(objImg, new Size(Convert.ToInt32(_width), Convert.ToInt32(_height)));
                objImg.Dispose();
                _img.Save(img, ImageFormat.Jpeg);
            }
        }
        public static string TryParse(HttpPostedFileBase file, int kb = 0)
        {
            if (file == null) return "Debe seleccionar un archivo";

            string rpta = "";

            if (file.ContentType.ToLower() == "image/jpg"
                || file.ContentType.ToLower() == "image/jpeg"
                || file.ContentType.ToLower() == "image/pjpeg"
                || file.ContentType.ToLower() == "image/gif"
                || file.ContentType.ToLower() == "image/x-png"
                || file.ContentType.ToLower() == "image/png")
            {
                if (kb > 0)
                {
                    if (Convert.ToInt32(file.ContentLength / 1000) > kb)
                    {
                        rpta = "La imagen no puede ser mayor a los " + kb + "kb";
                    }
                }
            }
            else
            {
                rpta = "El archivo a intentar a subir debe ser una imagen";
            }

            return rpta;
        }
    }
}
