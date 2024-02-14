using Microsoft.Extensions.Options;
using Imagekit.Sdk;

namespace Helper;

//HOW TO USE

//Installation (Package Manager)
// Install-Package Imagekit

//FILE "appsettings.json"
// "ImageKitSettings": {
//    "PublicKey": "XXXXX",
//    "PrivateKey": "XXXXX",
//    "UrlEndPoint": "https://ik.imagekit.io/XXXXX"
// },

//FILE "Program.cs"
// string OBJ_IMAGEKIT_SETTING = "ImageKitSettings";
// services.Configure<ImageKitConfiguration>(builder.Configuration.GetSection(OBJ_IMAGEKIT_SETTING));
// services.AddSingleton<ImageKit>();

//FILE "YourService.cs"
// string image = _imageKit.Create(image_base64_param, image_name_param);

public class ImageKit
{
    private readonly ImageKitConfiguration _options;

    public ImageKit(IOptions<ImageKitConfiguration> options)
    {
        _options = options.Value;
    }

    public string? Create(string? imageBase64, string imageName)
    {
        //imagekit setup
        ImagekitClient imagekit = new(_options.PublicKey, _options.PrivateKey, _options.UrlEndPoint);

        if (imageBase64 != null || imageBase64 != "")
        {
            //image upload request
            FileCreateRequest request = new()
            {
                file = imageBase64,
                fileName = imageName,
            };
            Result resp = imagekit.Upload(request);

            //image quality configuration
            Transformation trans = new Transformation()
            .Width(400)
            .Height(300)
            .AspectRatio("4-3")
            .Quality(40)
            .Crop("force")
            .CropMode("extract")
            .Focus("left")
            .Format("jpeg");

            //generate image url
            string imageURL = imagekit.Url(trans).Path(resp.filePath).TransformationPosition("query").Generate();

            return imageURL;
        }

        return null;
    }
}

public class ImageKitConfiguration
{
    public string? PublicKey { get; set; }
    public string? PrivateKey { get; set; }
    public string? UrlEndPoint { get; set; }
}

