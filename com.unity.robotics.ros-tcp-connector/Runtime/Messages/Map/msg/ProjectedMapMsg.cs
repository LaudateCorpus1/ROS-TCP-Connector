//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Map
{
    [Serializable]
    public class ProjectedMapMsg : Message
    {
        public const string k_RosMessageName = "map_msgs/ProjectedMap";

        public Nav.OccupancyGridMsg map;
        public double min_z;
        public double max_z;

        public ProjectedMapMsg()
        {
            this.map = new Nav.OccupancyGridMsg();
            this.min_z = 0.0;
            this.max_z = 0.0;
        }

        public ProjectedMapMsg(Nav.OccupancyGridMsg map, double min_z, double max_z)
        {
            this.map = map;
            this.min_z = min_z;
            this.max_z = max_z;
        }

        public static ProjectedMapMsg Deserialize(MessageDeserializer deserializer) => new ProjectedMapMsg(deserializer);

        private ProjectedMapMsg(MessageDeserializer deserializer)
        {
            this.map = Nav.OccupancyGridMsg.Deserialize(deserializer);
            deserializer.Read(out this.min_z);
            deserializer.Read(out this.max_z);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.map);
            serializer.Write(this.min_z);
            serializer.Write(this.max_z);
        }

        public override string ToString()
        {
            return "ProjectedMapMsg: " +
            "\nmap: " + map.ToString() +
            "\nmin_z: " + min_z.ToString() +
            "\nmax_z: " + max_z.ToString();
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