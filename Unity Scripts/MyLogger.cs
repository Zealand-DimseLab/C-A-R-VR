// using System.Text;
// using System.IO;
// using UnityEngine;
//
// public class MyLogger : MonoBehaviour{
//     [SerializeField] private string filePath;
//     [SerializeField] private BikeController bikeController;
//
//     private float lastLog;
//     void Update(){
//         if(Time.time - lastLog > 1){
//             string toAppend = "Time: " + Time.time + ", pulse: " + bikeController.speed.ToString() + ", distanceToCar: 0.75\n";
//             File.AppendAllText(filePath,toAppend);
//             lastLog = Time.time;
//         }
//     }
// }
using System.Text;
using System.IO;
using UnityEngine;
using System.Threading;
using System.Collections.Concurrent;

public class MyLogger : MonoBehaviour
{
    [SerializeField] private string filePath;
    [SerializeField] private BikeController bikeController;

    private float lastLog;
    private ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();
    private Thread loggingThread;
    private bool isRunning = true;

    void Start()
    {
        // Initialize and start the background thread
        loggingThread = new Thread(ProcessQueue);
        loggingThread.IsBackground = true; // Ensures the thread dies when the app closes
        loggingThread.Start();
    }

    void Update()
    {
        if (Time.time - lastLog > 1f)
        {
            // Prepare the string on the main thread (since we need Unity-specific data)
            string toAppend = $"Time: {Time.time}, pulse: {bikeController.speed}, distanceToCar: 0.75\n";
            
            // Enqueue the string for the background thread to pick up
            logQueue.Enqueue(toAppend);
            
            lastLog = Time.time;
        }
    }

    private void ProcessQueue()
    {
        while (isRunning)
        {
            // If there is data in the queue, write it all out
            if (logQueue.TryDequeue(out string message))
            {
                try 
                {
                    File.AppendAllText(filePath, message);
                }
                catch (System.Exception e) 
                {
                    Debug.LogError($"Logging Thread Error: {e.Message}");
                }
            }
            else
            {
                // If queue is empty, sleep for a bit to save CPU cycles
                Thread.Sleep(100); 
            }
        }
    }

    private void OnApplicationQuit()
    {
        isRunning = false;
        // Optional: Give the thread a moment to finish final writes
        if (loggingThread != null && loggingThread.IsAlive)
        {
            loggingThread.Join(500); 
        }
    }
}
