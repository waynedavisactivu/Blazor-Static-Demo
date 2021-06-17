using Activu.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorStatic.Shared.Messages
{
    public class AlertMessage : MessageBase
    {        
        [Required(ErrorMessage = "Message body required")]
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [Required(ErrorMessage = "Background color required")]
        [JsonPropertyName("backgroundColor")]
        public string BackgroundColor { get; set; }

        [Required(ErrorMessage = "Text color required")]
        [JsonPropertyName("textColor")]
        public string TextColor { get; set; }

        [JsonPropertyName("connectionId")]
        public string ConnectionId { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        public override Message Serialize()
        {
            Created = DateTime.Now;
            return new Message(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(this)));
        }
    }
}
