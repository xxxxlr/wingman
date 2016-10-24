//using UnityEngine;
//using System.Collections;

//using UnityEngine.VR.WSA.WebCam;
//using System.Collections.Generic;
//using System.Linq;
//#if !UNITY_EDITOR
//using Windows.Storage;
//using Windows.System;
//using System.Collections.Generic;
//using System;
//using System.IO;
//using System.Threading.Tasks;
//#endif

//#if WINDOWS_UWP
//using System.Runtime.InteropServices.WindowsRuntime;
//using Windows.Data.Json;
//using Windows.Web.Http;
//using Windows.Web.Http.Headers;
//#endif

//public class SphereCommand : MonoBehaviour
//{

//    PhotoCapture photoCaptureObject = null;
//    bool haveFolderPath = false;
//#if !UNITY_EDITOR
//    StorageFolder picturesFolder;
//#endif
//    string tempFilePathAndName;
//    string tempFileName;
//    // Use this for initialization
//    GameObject mainCameraObj;
//    void Start()
//    {
//        mainCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    void OnImageUploadRequest()
//    {

//        //List<byte> testbuffer = new List<byte>() ;


//        //for(byte i =0; i < 10; i++)
//        //{
//        //    testbuffer.Add(i);
//        //    Debug.Log("testbuffer:====================\n");
//        //    Debug.Log(testbuffer);
//        //    Debug.Log("!testbuffer.ToArray() base64:====================\n");
//        //    Debug.Log(System.Convert.ToBase64String(testbuffer.ToArray()));
//        //}
//        //return ;

//        Debug.Log("OnImageUploadRequest ->() ");
//        Debug.Log("TakePicture() ");
//        /*
//        getFolderPath();
//        while ( !haveFolderPath)
//        {
//            Debug.Log("Waiting for folder path...");
//        }
//        */

//        TakePicture();
//    }

//    void TakePicture()
//    {
//        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);


//    }

//    void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
//    {
//        if (result.success)
//        {
//            // Create our Texture2D for use and set the correct resolution
//            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).Last();
//            Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);
//            // Copy the raw image data into our target texture
//            photoCaptureFrame.UploadImageDataToTexture(targetTexture);
//            // Do as we wish with the texture such as apply it to a material, etc.
//        }
//        // Clean up
//        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
//    }

//    void OnPhotoCaptureCreated(PhotoCapture captureObject)
//    {
//        photoCaptureObject = captureObject;

//        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

//        CameraParameters c = new CameraParameters();
//        c.hologramOpacity = 0.0f;
//        c.cameraResolutionWidth = cameraResolution.width;
//        c.cameraResolutionHeight = cameraResolution.height;
//        c.pixelFormat = CapturePixelFormat.PNG;


//        photoCaptureObject.StartPhotoModeAsync(c, false, OnPhotoModeStartedForBinary);
//        //
//        //  photoCaptureObject.StartPhotoModeAsync(c, false, OnPhotoModeStartedForDisk);
//    }

//    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
//    {
//        photoCaptureObject.Dispose();
//        photoCaptureObject = null;
//    }

//    private void OnPhotoModeStartedForDisk(PhotoCapture.PhotoCaptureResult result)
//    {
//        if (result.success)
//        {
//            tempFileName = string.Format(@"CapturedImage{0}_n.jpg", Time.time);

//            string filePath = System.IO.Path.Combine(Application.persistentDataPath, tempFileName);
//            tempFilePathAndName = filePath;
//            Debug.Log("Saving photo to " + filePath);

//            try
//            {
//                photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
//            }
//            catch (System.ArgumentException e)
//            {
//                Debug.LogError("System.ArgumentException:\n" + e.Message);
//            }
//        }
//        else
//        {
//            Debug.LogError("Unable to start photo mode!");
//        }
//    }

//    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
//    {
//        if (result.success)
//        {
//            Debug.Log("Saved Photo to disk!");
//            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
//#if !UNITY_EDITOR

//            Debug.Log("moving " + tempFilePathAndName + " to " + picturesFolder.Path + "\\Camera Roll\\" + tempFileName);
//            File.Move(tempFilePathAndName, picturesFolder.Path + "\\Camera Roll\\" + tempFileName);
//#endif
//        }
//        else
//        {
//            Debug.Log("Failed to save Photo to disk " + result.hResult + " " + result.resultType.ToString());
//        }
//    }
//#if !UNITY_EDITOR
//    async void getFolderPath()
//    {
//        StorageLibrary myPictures = await Windows.Storage.StorageLibrary.GetLibraryAsync(Windows.Storage.KnownLibraryId.Pictures);
//        picturesFolder = myPictures.SaveFolder;

//        foreach (StorageFolder fodler in myPictures.Folders)
//        {
//            Debug.Log(fodler.Name);

//        }

//        Debug.Log("savePicturesFolder.Path is " + picturesFolder.Path);
//        haveFolderPath = true;
//    }
//#endif


//    void OnPhotoModeStartedForBinary(PhotoCapture.PhotoCaptureResult result)
//    {

//        if (result.success)
//        {
//            //tempFileName = string.Format(@"CapturedImage{0}_n.jpg", Time.time);

//            //string filePath = System.IO.Path.Combine(Application.persistentDataPath, tempFileName);
//            //tempFilePathAndName = filePath;
//            //Debug.Log("Saving photo to " + filePath);

//            //try
//            //{
//            //    photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);
//            //}
//            //catch (System.ArgumentException e)
//            //{
//            //    Debug.LogError("System.ArgumentException:\n" + e.Message);
//            //}

//            photoCaptureObject.TakePhotoAsync((memoryResult, memoryData) =>
//            {
//                if (memoryResult.resultType == PhotoCapture.CaptureResultType.Success)
//                {
//                    List<byte> buffer = new List<byte>();



//                    memoryData.CopyRawImageDataIntoBuffer(buffer);
//                    string bytesInString64 = System.Convert.ToBase64String(buffer.ToArray());


//                    // Now we could do something with the array such as texture.SetPixels() or run image processing on the list
//                    //Debug.Log("Image base64:====================\n");
//                    //Debug.Log(bytes);
//                    // and now you have your bytes as a string, you can upload them or whatever.
//#if !UNITY_EDITOR
//                    try
//                    {
//                        Debug.Log("Start SendImageBinary-->");
//                        Task<string> imageAnalysisResult = await SendImageBinary(bytesInString64, buffer.ToArray());
//                        imageAnalysisResult.ContinueWith(task =>
//                        {
//                            Debug.Log("Async done doing SendImageBinary, drawing the taks.Result string");
//                            mainCameraObj.OnTextIndicatorReques(task.Result);
//                        });
//                        Debug.Log("<--??Finish SendImageBinary or asynced pass??");

//                    }
//                    catch (Exception e)
//                    {
//                        Debug.Log("Exception:Send Image Binary!!!manually StopPhotoModeAsync");
//                        Debug.Log(e.Message);
//                        //TODO: multiple call...
//                        photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
//                    }
//#endif
//                }
//                photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
//            });
//        }
//        else
//        {
//            Debug.LogError("Unable to start photo mode!Maybe causeing by previous not stopped photo mode, so manually close it so next call can pass? need to test...");
//            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
//            Debug.LogError("I just manually closed it StopPhotoModeAsync.");

//        }

//    }

//    public static async Task<string> SendImageBinary(string binaryImage, byte[] binaryImageAsArray)
//    {
//#if !UNITY_EDITOR
//        HttpClient client = new HttpClient();
//        HttpStringContent content = null;
//        HttpRequestMessage request = null;
//        HttpResponseMessage response = null;
//        HttpBufferContent contentBuffer = null;
//        HttpBufferContent colorBuffer = null;

//        Uri uri = new System.Uri("http://10.1.1.27:8080");

//        content = new HttpStringContent(binaryImage);
//        request = new HttpRequestMessage(HttpMethod.Post, uri);
//        //client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
//        //Debug.Log("content:");
//        //Debug.Log(content);

//        contentBuffer = new HttpBufferContent(binaryImageAsArray.AsBuffer());
//        //colorBuffer = new HttpBufferContent(colorList.AsBuffer());
        
//        request = new HttpRequestMessage(HttpMethod.Post, uri);
//        //contentBuffer.Headers.Add("Content-Type", "application/octet-stream");
//        /*
//        Debug.Log("contentBuffer:");
//        Debug.Log(contentBuffer);
//        Debug.Log("first 8 bytes:");
//        for(int i = 0; i < 8; i++)
//        {
//            Debug.Log(binaryImageAsArray[i]);
//        }
//        */

//        request.Content = content;
//        //response = client.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead).AsTask().Result;
//        /* THE STACK OVERFLOW EXCEPTION IS TRIGGERED HERE */
        
//        response = await client.SendRequestAsync(request);
//        //HttpResponseMessage = await client.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead).AsTask();
//        //GameObject.FindGameObjectWithTag("MainCamera").SendMessage("OnTextIndicatorRequest", response.Content.ToString());
//        //
//        //return response;
//        //Debug.Log("-------------");
//        //Debug.Log(response.Content);
//        response.EnsureSuccessStatusCode();

//        return response.Content.ToString();

//#endif
//    }
//}