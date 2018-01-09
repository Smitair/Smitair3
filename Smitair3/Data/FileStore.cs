using Microsoft.AspNetCore.Identity;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Smitair3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace Smitair3.Data
{
    public class FileStore
    {
        Uri _baseUri = new Uri("https://smitair.blob.core.windows.net/");
        CloudBlobClient _client;

        public FileStore()
        {
            _client = new CloudBlobClient(_baseUri, new StorageCredentials("smitair",
                "FOTb/jAUg9xet6Iwr1oEFGiQqcEC+7sxHAKGtTMFtEs3nYe1YveQaiG3eQiNVwJtIryjXPV56qFw+if7eV7M1w=="));
        }

        public async Task<string>SaveImage(Stream stream, string id)
        {
            var container = _client.GetContainerReference("avatars");
            var blob = container.GetBlockBlobReference(id);
            await blob.UploadFromStreamAsync(stream);
            return id;
        }

        public Uri UriForImage(string imageId)
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

        public async Task<string> SaveEffect(Stream stream, string id)
        {
            var container = _client.GetContainerReference("effects");
            var blob = container.GetBlockBlobReference(id);
            await blob.UploadFromStreamAsync(stream);
            return id;
        }

        public Uri UriForEffect(string effectId)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.Now.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.MaxValue
            };
            var container = _client.GetContainerReference("effects");
            var blob = container.GetBlockBlobReference(effectId);
            var sasToken = blob.GetSharedAccessSignature(sasPolicy);

            return new Uri(_baseUri, $"effects/{effectId}{sasToken}");
        }

        public ClaimsPrincipal User { get; private set; }
    }
}
