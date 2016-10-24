//using UnityEngine;

//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Services;
//using Google.Apis.Vision.v1;
//using Google.Apis.Vision.v1.Data;
//using System;
//using System.Collections.Generic;


//namespace GoogleCloudSamples
//{
//    public class GoogleVisionAPI
//    {
//        const string usage = @"Usage:LabelDetectionSample <path_to_image>";
//        /// <summary>
//        /// Creates an authorized Cloud Vision client service using Application 
//        /// Default Credentials.
//        /// </summary>
//        /// <returns>an authorized Cloud Vision client.</returns>
//        public VisionService CreateAuthorizedClient()
//        {
//            GoogleCredential credential =
//                GoogleCredential.GetApplicationDefaultAsync().Result;
//            // Inject the Cloud Vision scopes
//            if (credential.IsCreateScopedRequired)
//            {
//                credential = credential.CreateScoped(new[]
//                {
//                    VisionService.Scope.CloudPlatform
//                });
//            }
//            return new VisionService(new BaseClientService.Initializer
//            {
//                HttpClientInitializer = credential,
//                GZipEnabled = false
//            });
//        }

//        /// <summary>
//        /// Detect labels for an image using the Cloud Vision API.
//        /// </summary>
//        /// <param name="vision">an authorized Cloud Vision client.</param>
//        /// <param name="imagePath">the path where the image is stored.</param>
//        /// <returns>a list of labels detected by the Vision API for the image.
//        /// </returns>
//        public IList<AnnotateImageResponse> DetectLabels(
//            VisionService vision, string imagePath)
//        {
//            Console.WriteLine("Detecting Labels...");
//            // Convert image to Base64 encoded for JSON ASCII text based request   
//            byte[] imageArray = System.IO.File.ReadAllBytes(imagePath);
//            string imageContent = Convert.ToBase64String(imageArray);
//            // Post label detection request to the Vision API
//            var responses = vision.Images.Annotate(
//                new BatchAnnotateImagesRequest()
//                {
//                    Requests = new[] {
//                    new AnnotateImageRequest() {
//                        Features = new [] { new Feature() { Type =
//                          "LABEL_DETECTION"}},
//                        Image = new Image() { Content = imageContent }
//                    }
//               }
//                }).Execute();
//            return responses.Responses;
//        }

//        private static void Main(string[] args)
//        {
//            LabelDetectionSample sample = new LabelDetectionSample();
//            string imagePath;
//            if (args.Length == 0)
//            {
//                Console.WriteLine(usage);
//                return;
//            }
//            imagePath = args[0];
//            // Create a new Cloud Vision client authorized via Application 
//            // Default Credentials
//            VisionService vision = sample.CreateAuthorizedClient();
//            // Use the client to get label annotations for the given image
//            IList<AnnotateImageResponse> result = sample.DetectLabels(
//                vision, imagePath);
//            // Check if label annotations were found
//            if (result != null)
//            {
//                Console.WriteLine("Labels for image: " + imagePath);
//                // Loop through and output label annotations for the image
//                foreach (var response in result)
//                {
//                    foreach (var label in response.LabelAnnotations)
//                    {
//                        Console.WriteLine(label.Description + " (score:"
//                        + label.Score + ")");
//                    }
//                }
//            }
//            else
//            {
//                Console.WriteLine("No labels found.");
//            }
//            Console.WriteLine("Press any key...");
//            Console.ReadKey();
//        }
//    }
//}
