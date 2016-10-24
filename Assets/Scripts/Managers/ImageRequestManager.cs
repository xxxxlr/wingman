using UnityEngine;
using System.Collections;

using UnityEngine.VR.WSA.WebCam;
using System.Collections.Generic;
using System.Linq;
#if !UNITY_EDITOR
using Windows.Storage;
using Windows.System;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;
#endif

#if !UNITY_EDITOR
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
#endif

public class ImageRequestManager : MonoBehaviour
{
    string POST_URL = "http://73.16.255.7:4001/connection/test1";
    string NETWORK_ISSUE_MESSAGE = "Can not identify this. Network issue.";
    string internal_url = "http://18.111.9.5:4001/connection/test1";
    //string POST_URL = "http://10.189.29.138:4001/connection/test1";

    PhotoCapture photoCaptureObject = null;
    bool haveFolderPath = false;
    // Use this for initialization
    GameObject CameraObject;
    void Start()
    {
        //POST_URL = internal_url;
        CameraObject = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void OnImageUploadRequest()
    {
        Debug.Log("OnImageUploadRequest ->() ");
        Debug.Log("TakePicture() ");
        TakePicture();
    }

    void TakePicture()
    {
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
    }

    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;

        //Resolution: 2048x1152 at 0Hz.
        //Resolution: 1408x792 at 0Hz.
        //Resolution: 1344x756 at 0Hz.
        //Resolution: 1280x720 at 0Hz.
        //Resolution: 896x504 at 0Hz.

        //Might effect recognization accuracy, especially there's requirment for different detection. but fast for upload to server. 
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).Skip(2).First();

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.PNG;


        photoCaptureObject.StartPhotoModeAsync(c, false, OnPhotoModeStartedForBinary);
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("In OnStoppedPhotoMode");
        if (result.success)
        {
            photoCaptureObject.Dispose();
            photoCaptureObject = null;
        }
        else
        {
            Debug.Log("Failed to StopPhotoModeAsync");
        }
    }

    void OnPhotoModeStartedForBinary(PhotoCapture.PhotoCaptureResult result)
    {

        if (result.success)
        {
            photoCaptureObject.TakePhotoAsync((memoryResult, memoryData) =>
            {
                if (memoryResult.resultType == PhotoCapture.CaptureResultType.Success)
                {
                    List<byte> buffer = new List<byte>();
                    memoryData.CopyRawImageDataIntoBuffer(buffer);
                    string bytesInString64 = System.Convert.ToBase64String(buffer.ToArray());
#if !UNITY_EDITOR
                    try
                    {
                        Debug.Log("Start SendImageBinary-->");
                        //StartCoroutine(CoSendImageBinary(bytesInString64, buffer.ToArray()));
                        StartCoroutine(CoSendImageBinary(buffer.ToArray()));
                        
                        Debug.Log("<--??Finish SendImageBinary or asynced pass??");

                    }
                    catch (Exception e)
                    {
                        Debug.Log("Exception:Send Image Binary!!!");
                        Debug.Log(e.Message);
                        CameraObject.SendMessage("OnImageAnalysisResult", NETWORK_ISSUE_MESSAGE);
                    }
#endif
                }
                photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
            });
        }
        else
        {
            Debug.LogError("Unable to start photo mode!Maybe causeing by previous not stopped photo mode, so manually close it so next call can pass? need to test...");
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
            Debug.LogError("I just manually closed it StopPhotoModeAsync.");

        }

    }
    IEnumerator CoSendImageBinary (byte[] binaryImageAsArray)
    {
        WWWForm form = new WWWForm();
        form.AddField("dist", "3.3");
        form.AddBinaryData("fileUpload", binaryImageAsArray, "screenShot.png", "image/png");
        WWW www = new WWW(POST_URL, form);
        //while (!www.isDone)
        //{
        //    yield return null;
        //}
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            print(www.error);
            CameraObject.SendMessage("OnImageAnalysisResult", NETWORK_ISSUE_MESSAGE);
        }
        else
        {
            CameraObject.SendMessage("OnImageAnalysisResult", www.text);
            print("Finished Uploading Screenshot");
        }
    }

//#if!UNITY_EDITOR

//    IEnumerator SendImageBinary(string binaryImage, byte[] binaryImageAsArray)
//    {

//        HttpClient client = new HttpClient();
//        HttpStringContent content = null;
//        HttpRequestMessage request = null;
//        HttpResponseMessage response = null;
//        HttpBufferContent contentBuffer = null;
//        HttpBufferContent colorBuffer = null;

//        //Uri uri = new System.Uri("http://10.1.1.27:8080");
//        Uri uri = new System.Uri("http://10.1.1.44:4001/connection/test1");
//        //Uri uri = new System.Uri("http://73.16.255.7:40012/connection/test1");

//        content = new HttpStringContent(binaryImage);
//        request = new HttpRequestMessage(HttpMethod.Post, uri);

//        request.Content = content;

//        //response = client.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead).AsTask().Result;
//        response = client.SendRequestAsync(request, HttpCompletionOption.ResponseContentRead).AsTask().Result;
//        while(!response.completed){
//            Debug.Log("!response.completed");
//            yield return "not completed";
//        }
//        CameraObject.SendMessage("OnImageAnalysisResult", response.Content.ToString());
        
        
//        yield return response.Content.ToString();
//    }
//#endif

}
