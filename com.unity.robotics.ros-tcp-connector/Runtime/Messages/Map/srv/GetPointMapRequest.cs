//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Map
{
    [Serializable]
    public class GetPointMapRequest : Message
    {
        public const string k_RosMessageName = "map_msgs/GetPointMap";

        //  Get the map as a sensor_msgs/PointCloud2

        public GetPointMapRequest()
        {
        }
        public static GetPointMapRequest Deserialize(MessageDeserializer deserializer) => new GetPointMapRequest(deserializer);

        private GetPointMapRequest(MessageDeserializer deserializer)
        {
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
        }

        public override string ToString()
        {
            return "GetPointMapRequest: ";
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#else
        [UnityEngine.RuntimeInitializeOnLoadMethod]
#endif
        public static void Register()
        {
            MessageRegistry.Register(k_RosMessageName, Deserialize);
        }
    }
}