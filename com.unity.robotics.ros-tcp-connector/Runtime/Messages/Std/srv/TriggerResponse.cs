//Do not edit! This file was generated by Unity-ROS MessageGeneration.
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;

namespace RosMessageTypes.Std
{
    [Serializable]
    public class TriggerResponse : Message
    {
        public const string k_RosMessageName = "std_srvs/Trigger";

        public bool success;
        //  indicate successful run of triggered service
        public string message;
        //  informational, e.g. for error messages

        public TriggerResponse()
        {
            this.success = false;
            this.message = "";
        }

        public TriggerResponse(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }

        public static TriggerResponse Deserialize(MessageDeserializer deserializer) => new TriggerResponse(deserializer);

        private TriggerResponse(MessageDeserializer deserializer)
        {
            deserializer.Read(out this.success);
            deserializer.Read(out this.message);
        }

        public override void SerializeTo(MessageSerializer serializer)
        {
            serializer.Write(this.success);
            serializer.Write(this.message);
        }

        public override string ToString()
        {
            return "TriggerResponse: " +
            "\nsuccess: " + success.ToString() +
            "\nmessage: " + message.ToString();
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