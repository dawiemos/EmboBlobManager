using Azure.Storage;
using Azure.Storage.Blobs;

namespace EmboBlobManager.BlobCls
{
    public class AzureBlobService
    {

        private readonly string _storageAccount = "emboinstall";
        private readonly string _accessKey = "Of3fjrFcdX9SO7cHJUbHlr7tYt0v3RsBn9z2jESKHB1ZfmXhM7YfHZW6JJnv/5IugQoRGlMeX6iz3nQvl1aObw==";
        private readonly string _connectionString = "";
        private readonly BlobServiceClient _blobServiceClient;

        public AzureBlobService()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _accessKey);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            _blobServiceClient = new BlobServiceClient(new System.Uri(blobUri), credential);
        }

        public async Task<List<string>> ListBlobContainersAsync()
        {
            var containers = _blobServiceClient.GetBlobContainersAsync();

            List<string> listOfContainers = new List<string>();

            await foreach (var container in containers)
            {
                listOfContainers.Add(container.Name);
            }

            return listOfContainers;
        }

        public async Task<List<Uri>> UploadFilesAsync()
        {

            var blobUris = new List<Uri>();

            //string filepath = @"C:\Embo\TestAzure\hello.txt";
            string filepath = @"hello.txt";
            var blobContainer = _blobServiceClient.GetBlobContainerClient("thewrx");

            var blob = blobContainer.GetBlobClient($"today/{filepath}");
            var tomorrowBlob = blobContainer.GetBlobClient($"tomorrow/{filepath}");

            await blob.UploadAsync(filepath, true);
            blobUris.Add(blob.Uri);
            await tomorrowBlob.UploadAsync(filepath, true);
            blobUris.Add(tomorrowBlob.Uri);

            return blobUris;
        }

        //public async Task<string> UploadSingleFileFromStream(string folderPath, string fileName, string connectionString = "", string containerName = "")
        //{
        //    if (connectionString == "")
        //        connectionString = "DefaultEndpointsProtocol=https;AccountName=emboinstall;AccountKey=Of3fjrFcdX9SO7cHJUbHlr7tYt0v3RsBn9z2jESKHB1ZfmXhM7YfHZW6JJnv/5IugQoRGlMeX6iz3nQvl1aObw==;EndpointSuffix=core.windows.net";
        //    if (containerName == "")
        //        containerName = "thewrx";
        //}

        public async Task<string> UploadSingleFile(string folderPath, string fileName, string connectionString = "", string containerName = "")
        {

            if (connectionString == "")
                connectionString = "DefaultEndpointsProtocol=https;AccountName=emboinstall;AccountKey=Of3fjrFcdX9SO7cHJUbHlr7tYt0v3RsBn9z2jESKHB1ZfmXhM7YfHZW6JJnv/5IugQoRGlMeX6iz3nQvl1aObw==;EndpointSuffix=core.windows.net";
            if (containerName == "")
                containerName = "thewrx";

            //var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

            var file = folderPath + fileName;

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);

            var filePathOverCloud = file.Replace(folderPath, string.Empty);

            using (FileStream stream = File.Open(file, FileMode.Open))
            {
                blobContainerClient.UploadBlob(filePathOverCloud, stream);
            }

            //using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(file)))
            //{
            //    blobContainerClient.UploadBlob(filePathOverCloud, stream);
            //}

            return filePathOverCloud;

            //foreach (var file in files)
            //{
            //    var filePathOverCloud = file.Replace(folderPath, string.Empty);
            //    using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(file)))
            //    {
            //        blobContainerClient.UploadBlob(filePathOverCloud, stream);
            //    }
            //}

        }

        public static void UploadFolderStructure(string folderPath, string connectionString = "", string containerName = "")
        {
            
            if (connectionString == "")
                connectionString = "DefaultEndpointsProtocol=https;AccountName=emboinstall;AccountKey=Of3fjrFcdX9SO7cHJUbHlr7tYt0v3RsBn9z2jESKHB1ZfmXhM7YfHZW6JJnv/5IugQoRGlMeX6iz3nQvl1aObw==;EndpointSuffix=core.windows.net";    
            if (containerName == "")
                containerName = "thewrx";
            
            var files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

            BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);

            foreach (var file in files)
            {
                var filePathOverCloud = file.Replace(folderPath, string.Empty);
                using(MemoryStream stream = new MemoryStream(File.ReadAllBytes(file)))
                {
                    blobContainerClient.UploadBlob(filePathOverCloud, stream);
                }
            }

        }
    }
}
