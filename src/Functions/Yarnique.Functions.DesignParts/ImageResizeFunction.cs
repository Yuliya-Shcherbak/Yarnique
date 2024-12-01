using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace Yarnique.Functions.DesignParts
{
    public class ImageResizeFunction
    {
        private readonly ILogger<ImageResizeFunction> _logger;

        public ImageResizeFunction(ILogger<ImageResizeFunction> logger)
        {
            _logger = logger;
        }

        [Function(nameof(ImageResizeFunction))]
        [BlobOutput("design-part-preview/{blobName}.jpeg", Connection = "AzureWebJobsStorage")]
        public byte[] Run(
            [BlobTrigger("design-parts/{blobName}.{blobExtension}", Connection = "AzureWebJobsStorage")] byte[] inputBlob,
            string blobName, string blobExtension)
        {
            try
            {
                _logger.LogInformation($"Starting processing the image : {blobName}.{blobExtension}");
                using var originalImage = SKBitmap.Decode(inputBlob);

                const int previewWidth = 150;
                var aspectRatio = (float)originalImage.Height / originalImage.Width;
                var previewHeight = (int)(previewWidth * aspectRatio);

                using var resizedImage = originalImage.Resize(
                    new SKImageInfo(previewWidth, previewHeight),
                    SKFilterQuality.High);

                using var data = resizedImage.Encode(SKEncodedImageFormat.Jpeg, 75);

                if (data == null)
                {
                    _logger.LogInformation($"Encode returned null : {blobName}.{blobExtension}");
                }
                else if (data.Size == 0)
                {
                    _logger.LogInformation($"Encode returned empty array : {blobName}.{blobExtension}");
                }

                _logger.LogInformation($"Resized image saved as preview: {blobName}.{blobExtension}");
  

                return data.ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing image {blobName}.{blobExtension}: {ex.Message}");
                return inputBlob;
            }
        }
    }
}
