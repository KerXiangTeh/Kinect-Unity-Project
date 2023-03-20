using UnityEngine;
using System.Collections;
using Microsoft.Azure.Kinect.Sensor;
using Microsoft.Azure.Kinect.BodyTracking;
using UnityEngine.Playables;

public class BodySourceManager : MonoBehaviour
{
    private Device _Sensor;
    private BodyTracker _Tracker;
    private FrameData[] _Data = null;

    public FrameData[] GetData()
    {
        return _Data;
    }


    void Start()
    {
        _Sensor = Device.Open();
        _Tracker = BodyTracker.Create(_Sensor.GetCalibration(), new BodyTrackerConfiguration());
    }

    void Update()
    {
        var capture = _Sensor.GetCapture();
        var frame = _Tracker.EnqueueCapture(capture);

        if (_Data == null)
        {
            _Data = new FrameData[1];
        }

        if (_Tracker.TryPopResult(out var result))
        {
            result.GetBodySkeletons(_Data);
        }
    }

    void OnApplicationQuit()
    {
        if (_Tracker != null)
        {
            _Tracker.Dispose();
            _Tracker = null;
        }

        if (_Sensor != null)
        {
            if (_Sensor.IsOpen)
            {
                _Sensor.Close();
            }

            _Sensor.Dispose();
            _Sensor = null;
        }
    }
}
