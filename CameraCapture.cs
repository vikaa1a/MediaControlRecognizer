using System;
using System.Drawing;
using System.Windows.Forms;
using Accord.Video.DirectShow;
using Accord.Video;

namespace MediaControlRecognizer
{
    public class CameraCapture
    {
        private VideoCaptureDevice videoSource;
        private bool isCapturing = false;

        public FilterInfoCollection GetAvailableCameras()
        {
            return new FilterInfoCollection(FilterCategory.VideoInputDevice);
        }

        public void StartCapture(FilterInfo cameraInfo, PictureBox displayBox)
        {
            if (isCapturing) return;

            videoSource = new VideoCaptureDevice(cameraInfo.MonikerString);
            videoSource.NewFrame += (sender, eventArgs) =>
            {
                if (displayBox.InvokeRequired)
                {
                    displayBox.Invoke(new Action(() =>
                    {
                        displayBox.Image = (Bitmap)eventArgs.Frame.Clone();
                    }));
                }
                else
                {
                    displayBox.Image = (Bitmap)eventArgs.Frame.Clone();
                }
            };

            videoSource.Start();
            isCapturing = true;
        }

        public void StopCapture()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }
            isCapturing = false;
        }

        public Bitmap GetCurrentFrame()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                return null;
            }
            return null;
        }
    }
}