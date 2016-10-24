using UnityEngine;
using System.Collections;

using UnityEngine.VR.WSA.WebCam;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;

public class ImageCaptureManager : MonoBehaviour {

    PhotoCapture photoCaptureObject = null;
    // Use this for initialization
    void Start () {
        Debug.Log("About to call CreateAsync");
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
        Debug.Log("Called CreateAsync");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;
        photoCaptureObject.TakePhotoAsync((memoryResult, memoryData) =>
        {
            if (memoryResult.resultType == PhotoCapture.CaptureResultType.Success)
            {
                List<byte> buffer = new List<byte>();
                memoryData.CopyRawImageDataIntoBuffer(buffer);
                string bytes = System.Convert.ToBase64String(buffer.ToArray());
                Debug.Log("Image base64:====================\n");
                Debug.Log(bytes);
                // and now you have your bytes as a string, you can upload them or whatever.

            }
        });
    }
}
