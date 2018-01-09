using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Smitair3.Data
{
    public class ImageStore
    {
        public ImageStore()
        {
            _client = new CloudBlobClient(_baseUri, new StorageCredentials("smitair",
                "FOTb/jAUg9xet6Iwr1oEFGiQqcEC+7sxHAKGtTMFtEs3nYe1YveQaiG3eQiNVwJtIryjXPV56qFw+if7eV7M1w=="));
        }

        public async Task<string>SaveImage(Stream stream)
        {
            var id = Guid.NewGuid().ToString();
            var container = _client.GetContainerReference("avatars");
            var blob = container.GetBlockBlobReference(id);
            await blob.UploadFromStreamAsync(stream);
            return id;
        }

        public Uri UriFor(string imageId)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.Now.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(30)
            };
            var container = _client.GetContainerReference("avatars");
            var blob = container.GetBlockBlobReference(imageId);
            var sasToken = blob.GetSharedAccessSignature(sasPolicy);

            return new Uri(_baseUri,$"avatars/{imageId}{sasToken}");
        }

        Uri _baseUri = new Uri("https://smitair.blob.core.windows.net/");
        CloudBlobClient _client;
    }
}
