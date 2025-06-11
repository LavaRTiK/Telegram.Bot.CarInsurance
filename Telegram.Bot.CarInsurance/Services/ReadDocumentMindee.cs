using Mindee;
using Mindee.Input;
using Mindee.Parsing.Common;
using Mindee.Product.DriverLicense;
using System.Reflection.Metadata;
using System.Text;

namespace Telegram.Bot.CarInsurance.Services
{
    public class ReadDocumentMindee
    {
        MindeeClient mindeeClient;

        public ReadDocumentMindee(MindeeClient mindeeClient)
        {
            this.mindeeClient = mindeeClient;
        }

        public async Task<Document<DriverLicenseV1>> ReadDriveLicense(MemoryStream PhotoStream)
        {
            byte[] photoByte = PhotoStream.ToArray();
            var inputSource = new LocalInputSource(fileBytes:photoByte,"telegram_photo.jpg");
            var response = await mindeeClient.EnqueueAndParseAsync<DriverLicenseV1>(inputSource);
            //Console.WriteLine(response.Document.ToString());
            return response.Document;
        }
    }
}
