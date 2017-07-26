using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AlexaSkill.Models
{
    [JsonObject]
    public class Message
    {
        [JsonProperty("MessageType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType messageType { get; set; }
        [JsonProperty("State")]
        public bool state { get; set; }
        [JsonProperty("RequestType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public RequestType requestType { get; set; }
        [JsonProperty("ReturnCode")]
        public int returnCode { get; set; }
    }

    public enum RequestType
    {
        GET,
        SET
    }

    public enum MessageType
    {
        REQUEST,
        RESPONSE
    }
}