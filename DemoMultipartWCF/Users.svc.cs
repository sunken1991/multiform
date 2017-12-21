using DemoMultipartWCF.DataContract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DemoMultipartWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Users" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Users.svc or Users.svc.cs at the Solution Explorer and start debugging.
    public class Users : IUsers
    {
        //Require Namespace: using System.IO
        public bool UpdateUserDetail(Stream stream)
        {

            try
            {
                //Create an Object of byte[]
                byte[] buf = new byte[10000000];

                //Initialise an Object of MultipartParser Class With Requested Stream
                MultipartParser parser = new MultipartParser(stream);

                //Check that we have not null value in requested stream
                if (parser != null && parser.Success)
                {
                    //Fetch Requested Formdata (content) 
                    //(for this example our requested formdata are UserName[String])
                    foreach (var item in parser.MyContents)
                    {
                        //Check our requested fordata
                        if (item.PropertyName == "UserName")
                        {
                            string RequestedName = item.StringData;

                        }
                    }

                    //Create a GUID for Image Name
                    string ImageName = Guid.NewGuid().ToString();

                    //Image Path
                    string SaveImagePath = "D:/DemoProject/DemoMultipartWCF/DemoMultipartWCF/MyFile/";

                    //Ensure That WE have the right path and Directory
                    if (!Directory.Exists(SaveImagePath))
                    {
                        //If Directory Not Exists Then Create a Directory
                        Directory.CreateDirectory(SaveImagePath);
                    }

                    //Fetch File Content & Save that Image HERE (for this example our requested FileContent is ProfilePicture[File])
                    string ImagePathWithImageName = SaveImagePath + ImageName + ".bmp";
                    SaveImageFile(parser.FileContents, ImagePathWithImageName);

                    return true;
                }

                return false;
            }
            catch
            {
                //Do Code For Log or Handle Exception 
                return false;
            }

        }

        //Method For Save Image File 
        public static bool SaveImageFile(byte[] ImageFileContent, string ImagePathWithImageName)
        {
            try
            {
                //Require Namespace: using System.Drawing
                Image image;

                //Read Image File
                using (MemoryStream ms = new MemoryStream(ImageFileContent))
                {
                    image = Image.FromStream(ms);
                }

                Bitmap bmp = new Bitmap(image);

                //Require Namespace: System.Drawing.Imaging
                ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);

                //We need to write Encoder with Root otherwise it is an ambiguous between "System.Drawing.Imaging.Encoder" and "System.Text.Encoder"
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 40L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp.Save(ImagePathWithImageName, jgpEncoder, myEncoderParameters);

                return true;
            }
            catch
            {
                //Do Code For Log or Handle Exception 
                return false;
            }
        }

        //ImageCodecInfo:- It provides the necessary storage members and methods to retrieve all pertinent information about the installed image codecs.
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


    }
}
