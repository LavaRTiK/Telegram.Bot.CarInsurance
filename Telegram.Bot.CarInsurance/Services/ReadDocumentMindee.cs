using Mindee;
using Mindee.Http;
using Mindee.Input;
using Mindee.Parsing.Common;
using Mindee.Product.Generated;
using Mindee.Product.InternationalId;
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

        public async Task<Document<InternationalIdV2>> ReadPassport(MemoryStream PhotoStream)
        {
            byte[] photoByte = PhotoStream.ToArray();
            var inputSource = new LocalInputSource(fileBytes:photoByte,"telegram_photo.jpg");
            var response = await mindeeClient.EnqueueAndParseAsync<InternationalIdV2>(inputSource);
            //Console.WriteLine(response.Document.ToString());
            return response.Document;
        }
        public async Task<Document<GeneratedV1>> ReadTexPassport(MemoryStream PhotoStream)
        {
            byte[] photoByte = PhotoStream.ToArray();
            var inputSource = new LocalInputSource(fileBytes: photoByte, "telegram_photo.jpg");
            CustomEndpoint endpoint = new CustomEndpoint(endpointName: "texpassportua",accountName: "Lava",version: "1");
            var response = await mindeeClient.EnqueueAndParseAsync<GeneratedV1>(inputSource, endpoint);
            return response.Document;
        }
    }
}
