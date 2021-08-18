using System;
using System.Collections.Generic;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Std;
using RosMessageTypes.Tf2;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public interface ITFSystemVisualizer
{
    void OnChanged(TFStream stream);
}

public class TFSystem
{
    static ITFSystemVisualizer s_Visualizer;
    Dictionary<string, Dictionary<string, TFStream>> m_TransformTable = new Dictionary<string, Dictionary<string, TFStream>>();
    Dictionary<string, Transform> m_TrackingTransformTable = new Dictionary<string, Transform>();
    public static TFSystem instance { get; private set; }

    public static TFSystem GetOrCreateInstance(string[] tftopics)
    {
        if (instance == null)
        {
            instance = new TFSystem();
            SubscribeToMultipleTopics<TFMessageMsg>(tftopics, instance.ReceiveTF);
            // foreach (string t in tftopics)
            // {
            //     string topic = t;
            //     Debug.Log($"{topic}");
            // }
        }
        return instance;
    }

    static void SubscribeToMultipleTopics<Msg>(string[] topics, Action<Msg, string> callback) where Msg : Message
    {
        foreach (string t in topics)
        {
            string topic = t;  // C# design flaw
            ROSConnection.GetOrCreateInstance().Subscribe<Msg>(
                topic,
                (Msg msg) => callback(msg, topic));
        }
    }

    public IEnumerable<string> GetTransformNames(string tfTopic = "/tf")
    {
        CheckTFTopicInDictionary(tfTopic);
        return m_TransformTable[tfTopic].Keys;
    }

    public IEnumerable<TFStream> GetTransforms(string tfTopic = "/tf")
    {
        CheckTFTopicInDictionary(tfTopic);
        return m_TransformTable[tfTopic].Values;
    }

    public static void Register(ITFSystemVisualizer visualizer, string tfTopic = "/tf")
    {
        s_Visualizer = visualizer;
        if (instance != null)
            foreach (var stream in instance.m_TransformTable[tfTopic].Values)
                UpdateVisualization(stream);
    }

    public static void UpdateVisualization(TFStream stream)
    {
        if (s_Visualizer != null)
            s_Visualizer.OnChanged(stream);
    }

    public TFFrame GetTransform(HeaderMsg header, string tfTopic = "/tf")
    {
        return GetTransform(header.frame_id, header.stamp.ToLongTime(), tfTopic);
    }

    public TFFrame GetTransform(string frame_id, long time, string tfTopic = "/tf")
    {
        var stream = GetTransformStream(frame_id, tfTopic);
        if (stream != null)
            return stream.GetWorldTF(time);
        return TFFrame.identity;
    }

    public TFFrame GetTransform(string frame_id, TimeMsg time, string tfTopic = "/tf")
    {
        return GetTransform(frame_id, time.ToLongTime(), tfTopic);
    }

    public TFStream GetTransformStream(string frame_id, string tfTopic = "/tf")
    {
        TFStream stream;
        CheckTFTopicInDictionary(tfTopic);
        m_TransformTable[tfTopic].TryGetValue(frame_id, out stream);
        return stream;
    }

    public GameObject GetTransformObject(string frame_id, string tfTopic = "/tf")
    {
        TFStream stream = GetOrCreateTFStream(frame_id, tfTopic);
        return stream.GameObject;
    }

    public void CheckTFTopicInDictionary(string topic)
    {
        Dictionary<string, TFStream> tfDict;
        if (!m_TransformTable.TryGetValue(topic, out tfDict))
        {
            m_TransformTable[topic] = new Dictionary<string, TFStream>();
        }
    }

    TFStream GetOrCreateTFStream(string frame_id, string tfTopic = "/tf")
    {
        TFStream tf;
        while (frame_id.EndsWith("/"))
            frame_id = frame_id.Substring(0, frame_id.Length - 1);

        var slash = frame_id.LastIndexOf('/');
        var singleName = slash == -1 ? frame_id : frame_id.Substring(slash + 1);
        CheckTFTopicInDictionary(tfTopic);
        if (!m_TransformTable[tfTopic].TryGetValue(singleName, out tf) || tf == null)
        {
            if (slash <= 0)
            {
                // there's no slash, or only an initial slash - just create a new root object
                // (set the parent later if and when we learn more)
                tf = new TFStream(null, singleName);
            }
            else
            {
                var parent = GetOrCreateTFStream(frame_id.Substring(0, slash), tfTopic);
                tf = new TFStream(parent, singleName);
            }

            m_TransformTable[tfTopic][singleName] = tf;
            UpdateVisualization(tf);
        }
        else if (slash > 0 && tf.Parent == null)
        {
            tf.SetParent(GetOrCreateTFStream(frame_id.Substring(0, slash), tfTopic));
        }

        return tf;
    }

    void ReceiveTF(TFMessageMsg message, string tfTopic = "/tf")
    // void ReceiveTF(TFMessageMsg message)
    {
        foreach (var tf_message in message.transforms)
        {
            var frame_id = tf_message.header.frame_id + "/" + tf_message.child_frame_id;
            var tf = GetOrCreateTFStream(frame_id, tfTopic);
            tf.Add(
                tf_message.header.stamp.ToLongTime(),
                tf_message.transform.translation.From<FLU>(),
                tf_message.transform.rotation.From<FLU>()
            );
            UpdateVisualization(tf);
        }
    }
}
