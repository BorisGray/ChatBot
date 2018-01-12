using ZXing;
using OpenCvSharp;

namespace MioBot.BarCode
{
   /// <summary>
   /// interface for a barcode reader class which can be used with the Mat type from OpenCVSharp
   /// </summary>
   public interface IBarcodeReaderImage : IBarcodeReader<Mat>
   {
   }
}
